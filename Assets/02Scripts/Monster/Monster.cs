using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public enum MonsterState
{
    Idle,
    Patrol,
    Trace,
    Comeback,
    Attack,
    Die
}
public enum MonsterType
{
    Melee,
    LongRange
}
 
public class Monster : MonoBehaviour, IHitable
{
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    /********************************************************/
    private Transform _target;            // �÷���
    private Vector3 StartPosition;         // ������ġ
    public MonsterType _monsterType;      

    public CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public float MoveSpeed       = 5;     // ���� �ӵ�
    public float FindDistance    = 5f;    // �����Ÿ�
    public float MoveDistance    = 40f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f;  // ��������
    public float AttackDistance  = 2f;    // ���ݹ���
    public int Damage            = 10;    // ���ݷ�
    private float _attackTimer   = 0f;    // ����Ÿ��
    public const float AttackDelay = 1f;  // ���ݵ�����

    public float patrolRadius = 10f;
    public float IDLE_DURATION = 3f;
    private float _idleTimer;
    private Vector3 _randomPosition;

    public GameObject BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;
    



    private MonsterState _currentState = MonsterState.Idle;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _characterController.isTrigger = false;


        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // Ÿ�ٿ��ٰ� �÷��̾ �־���
        _navMeshAgent.updateRotation = false;
        StartPosition = transform.position;
        

        Init();
    }

    public void Init()
    {
        _idleTimer = 0f;
        Health = MaxHealth;
    }

    public void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // ü�¹� 0 ~ 1

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Trace:
                Trace();
                break;
            case MonsterState.Comeback:
                Comeback();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Die:
                Die();
                break;
        }
    }
    public void Idle()
    {
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            Debug.Log("���� ��ȯ: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            SetRandomPatrolPoint();
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    public void Patrol()
    {
        if (_navMeshAgent.remainingDistance <= TOLERANCE)
        {
            SetRandomPatrolPoint();
        }
        
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("���� ��ȯ: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }

    }
    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // �ݰ� ������ ������ ���� ����
        randomDirection += transform.position; // ���� ��ġ�� ����
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1); // �׺�޽� �� ��ȿ�� ��ġ ã��
        _randomPosition = hit.position;
        _navMeshAgent.SetDestination(_randomPosition); // ���� �̵� ��ǥ ����
    }
    public void Trace()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.Normalize();
        // ������̼��� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� ����
        _navMeshAgent.stoppingDistance = AttackDistance;

        // ������̼� �������� Ÿ������ ��ġ
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }

    }
    public void Comeback()
    {
        Vector3 dir = StartPosition - this.transform.position;
        dir.Normalize();

        // ������̼��� �����ϴ� �ּ� �Ÿ��� ���� ����
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // ������̼� �������� ��ŸƮ ��������
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("���� ��ȯ: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("���� ��ȯ: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    private void Attack()
    {
        // ���� ���: �÷��̾�� �Ÿ��� ���� �������� �־����� �ٽ� Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("���� ��ȯ: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // Attack ������ �� N�ʿ� �� �� ������ ������ �ֱ�
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            if (_monsterType == MonsterType.Melee)
            {
                MeleeAttack();
            }
            
            if (_monsterType == MonsterType.LongRange)
            {
                LongRangeAttack();
            }
        }
    }

    public void MeleeAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("���ȴ�!");
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;
        }
    }
    
   public void LongRangeAttack()
    {    
        transform.LookAt(_target);
   
        GameObject bullet = Instantiate(BulletPrefab, BulletPoint.position, BulletPoint.rotation);

        Bullet bulletdamage = bullet.GetComponent<Bullet>();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        bulletdamage.info = new DamageInfo(DamageType.Normal, Damage);
        Vector3 bulletDir = _target.position - BulletPoint.position;
        rb.velocity = bulletDir.normalized * BulletSpeed;
        Debug.Log(rb.velocity);

        _attackTimer = 0f;
    }
    public void Hit(DamageInfo damage)
    {
        Health -= damage.Amount;
        if (Health <= 0)
        {
            _animator.SetTrigger("Die");
            _currentState = MonsterState.Die;            
        }
    }
    public void Die()
    { 
        gameObject.SetActive(false);
    }


}



