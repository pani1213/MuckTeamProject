using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BossMonster : MonoBehaviour, IHitable
{
    public float Health;
    public float MaxHealth = 100;
    public Slider HealthSliderUI;
    public float HealthRegen = 10f;
    public float HealthRegenTimeInterval = 1f;
    public float HealthRegenTime = 0f;
    /********************************************************/
    private Transform _target;            // �÷���
    private Vector3 StartPosition;         // ������ġ
    public MonsterType _monsterType;

    public CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public float MoveSpeed = 5;           // ���� �ӵ�
    public float FindDistance = 5f;       // �����Ÿ�
    public float ComeDistance = 5f;       // �����Ÿ�
    public float MoveDistance = 40f;      // ������ �� �ִ� �Ÿ�
    public float _moveDistanceRemaining;  // �¾��� �� �Ÿ� �ʱ�ȭ
    public const float TOLERANCE = 0.1f;  // ��������
    public float AttackDistance = 2f;     // ���ݹ���
    public int Damage = 10;    // ���ݷ�
    private float _attackTimer = 0f;    // ����Ÿ��
    public float AttackDelay = 1f;  // ���ݵ�����
    public float HitDistance = 10f;
    private float _bossDestroyTime = 4f;
    public Vector3 FirstPosition;


    public float PatrolRadius = 10f;
    public float IDLE_DURATION = 1f;
    private float _idleTimer;


    // �Ѿ�
    public GameObject BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;


    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    private bool isRotating = false; // ȸ�� ����

    private MonsterState _currentState = MonsterState.Idle;
    private void Start()
    {

        _characterController = GetComponent<CharacterController>();
        _characterController.isTrigger = false;


        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // Ÿ�ٿ��ٰ� �÷��̾ �־���
        Debug.Log(_target.gameObject.name);
        _navMeshAgent.updateRotation = false;
        StartPosition = transform.position;
        FirstPosition = StartPosition;

        Init();
    }

    public void Init()
    {
        _idleTimer = 0f;
        Health = MaxHealth;
        _moveDistanceRemaining = MoveDistance;
    }

    public void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // ü�¹� 0 ~ 1

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
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
            case MonsterState.Damaged:
                Damaged();
                break;
            case MonsterState.Die:
                Die();
                break;
        }

    }

    public void Idle()
    {
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            //Debug.Log("���� ��ȯ: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Trace()
    {

        // ������̼��� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� ����
        _navMeshAgent.stoppingDistance = AttackDistance;

        // ������̼� �������� Ÿ������ ��ġ
        _navMeshAgent.destination = _target.position;

        //Debug.Log(Vector3.Distance(transform.position, _target.transform.position));

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
        transform.LookAt(_target);

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
            // Debug.Log("���� ��ȯ: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            // Debug.Log("���� ��ȯ: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        // ���� ������ �����̰�, �÷��̾���� �Ÿ��� ���� ���� ���� ���� �ִ��� Ȯ��
        if (Vector3.Distance(_target.position, transform.position) <= ComeDistance)
        {
            //Debug.Log("���� ��ȯ: Comeback -> Attack");
            Debug.Log($"��ŸƮ �م�{transform.position}");
            StartPosition = transform.position;
            _navMeshAgent.destination = StartPosition;
            _animator.SetTrigger("ComebackToAttack");
            _currentState = MonsterState.Attack;
        }

        transform.LookAt(StartPosition);

        // �� ȸ�� Ÿ�̸� ����
        HealthRegenTime += Time.deltaTime;
        if (HealthRegenTime >= HealthRegenTimeInterval)
        {
            // ���� �ð����� �Ǹ� ȸ��
            Health = Mathf.Min(MaxHealth, Health + HealthRegen);
            HealthRegenTime = 0.0f;
        }
    }
    public void Attack()
    {
        // ���� ���: �÷��̾�� �Ÿ��� ���� �������� �־����� �ٽ� Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            //Debug.Log("���� ��ȯ: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // Attack ������ �� N�ʿ� �� �� ������ ������ �ֱ�
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {

            if (isSetAni)
                StartCoroutine(setAnimation_Corutine());
            if (_monsterType == MonsterType.Melee)
            {

            }

            if (_monsterType == MonsterType.LongRange)
            {
                LongRangeAttack();
            }
        }
    }

    bool isSetAni = true;
    int a;
    IEnumerator setAnimation_Corutine()
    {
        isSetAni = false;
        a = Random.Range(1, 4);
        if (a == 1)
        {
            _animator.SetTrigger("Attack1");
         // Attack1�� ���� ���� ���
        }
        if (a == 2)
        {
            _animator.SetTrigger("Attack2");
           // Attack2�� ���� ���� ���
        }
        if (a == 0)
        {
            _animator.SetTrigger("Attack3");
          // Attack3�� ���� ���� ���
        }

        yield return new WaitForSeconds(1.2f);
        isSetAni = true;
    }

    void PlayAttackSound(int attackIndex)
    {
        Debug.Log(attackIndex);
        switch (attackIndex)
        {
            case 1:
                
        Debug.Log(1);
                SoundManager.instance.PlayAudio("Attack1"); // Attack1�� ���� ���� ���
                break;
            case 2:
        Debug.Log(1);
                SoundManager.instance.PlayAudio("Attack2"); // Attack2�� ���� ���� ���
                break;
            case 3:
        Debug.Log(1);
                SoundManager.instance.PlayAudio("Attack3"); // Attack3�� ���� ���� ���
                break;
        }
    }

    public void MeleeAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null && Vector3.Distance(_target.position, transform.position) < HitDistance)
        {
       
            PlayAttackSound(a);
            
            transform.LookAt(_target);
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);

            _attackTimer = 0f;

            Debug.Log("���ȴ�");
        }
    }
    private void RotateAndMoveTowardsTarget(Vector3 directionToTarget)
    {
        // ȸ�� ���·� ����
        isRotating = true;

        // �÷��̾ ��Ȯ�� ���ϵ��� ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        // �ε巯�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ȸ���� ������ �̵�
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            // �̵�
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowardsTarget(Vector3 directionToTarget)
    {
        // ȸ�� ���·� ����
        isRotating = true;

        // �÷��̾ ��Ȯ�� ���ϵ��� ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        // �ε巯�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    public void LongRangeAttack()
    {
        transform.LookAt(_target);

        GameObject bullet = Instantiate(BulletPrefab, BulletPoint.position, BulletPoint.rotation);

        Bullet bulletdamage = bullet.GetComponent<Bullet>();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        bulletdamage.info = new DamageInfo(DamageType.Normal, Damage);
        Vector3 bulletDir = new Vector3(_target.position.x, _target.position.y + 1f, _target.position.z) - BulletPoint.position;
        rb.velocity = bulletDir.normalized * BulletSpeed;
        //Debug.Log(rb.velocity);

        Debug.Log("���ȴ�!");

        _attackTimer = 0f;

    }

    public void Damaged()
    {
        _animator.SetTrigger("DamagedToTrace");
        _currentState = MonsterState.Trace;

        StartCoroutine(AttackAfterDelay(AttackDelay));
    }
    private IEnumerator AttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� ���°� Ʈ���̽� �������� Ȯ���ϰ�, Ʈ���̽� ������ ������ ������ ����
        if (_currentState == MonsterState.Trace)
        {
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }
    }
    public void Hit(DamageInfo damage)
    {
        Health -= damage.Amount;
        if (Health <= 0)
        {
            _animator.SetTrigger("Die");
            _currentState = MonsterState.Die;
        }
        if (Vector3.Distance(transform.position, _target.position) >= _moveDistanceRemaining)
        {
            _moveDistanceRemaining = MoveDistance;
        }
    }
    public void Die()
    {
        StartCoroutine(DestroyAfterDeath(_bossDestroyTime));
    }

    IEnumerator DestroyAfterDeath(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
