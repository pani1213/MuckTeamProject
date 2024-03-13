using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour, IHitable
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

    public float MoveSpeed = 5;     // ���� �ӵ�
    public float FindDistance = 5f;    // �����Ÿ�
    public float ComebackDistance = 10f;
    public float MoveDistance = 40f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f;  // ��������
    public float AttackDistance = 2f;    // ���ݹ���
    public int Damage = 10;    // ���ݷ�
    private float _attackTimer = 0f;    // ����Ÿ��
    public  float AttackDelay = 1f;  // ���ݵ�����

    public float PatrolRadius = 10f;
    public float IDLE_DURATION = 1f;
    private float _idleTimer;
    private Vector3 _randomPosition;

    // �Ѿ�
    public GameObject BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;

    // �˹� Ÿ�̸�
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private float _comebackStartTime; // Comeback ���� ���� �ð� ���� ����

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
            /*            case MonsterState.Patrol:
                            Patrol();
                            break;*/
            case MonsterState.Trace:
                Trace();
                break;
            case MonsterState.Comeback:
                Comeback();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Damage:
                Damaged();
                break;
            case MonsterState.Die:
                Die();
                break;
        }
    }
    public void Idle()
    {
/*        _idleTimer += Time.deltaTime;
        if (_idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            //Debug.Log("���� ��ȯ: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            SetRandomPatrolPoint();
        }*/

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            //Debug.Log("���� ��ȯ: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }
/*    public void Patrol()
    {
        if (_navMeshAgent.remainingDistance <= TOLERANCE)
        {
            SetRandomPatrolPoint();
        }

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            // Debug.Log("���� ��ȯ: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            // Debug.Log("���� ��ȯ: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }

    }*/
    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * PatrolRadius; // �ݰ� ������ ������ ���� ����
        randomDirection += transform.position; // ���� ��ġ�� ����
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, PatrolRadius, 1); // �׺�޽� �� ��ȿ�� ��ġ ã��
        _randomPosition = hit.position;
        _navMeshAgent.SetDestination(_randomPosition); // ���� �̵� ��ǥ ����
    }


    public void Trace()
    {

        // ������̼��� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� ����
        _navMeshAgent.stoppingDistance = AttackDistance;

        // ������̼� �������� Ÿ������ ��ġ
        _navMeshAgent.destination = _target.position;

        Debug.Log(Vector3.Distance(transform.position, _target.transform.position));

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
            return;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
            return;
        }


        NavMeshHit hit;

        // targetPoint ��ġ���� Raycast�� �����Ͽ� ������̼� �޽� ��Ʈ �˻�
        if (NavMesh.SamplePosition(_target.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            // ��Ʈ�� �߻����� ��� ������̼� �޽� �󿡼� �̵� ������ �������� ��Ÿ��
            Debug.Log("�̵� ������ �����Դϴ�.");
        }
        else
        {
            // ��Ʈ�� �߻����� �ʾ��� ��� ������̼� �޽� �󿡼� �̵� �Ұ����� �������� ��Ÿ��
            Debug.Log("�̵� �Ұ����� �����Դϴ�.");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
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
        

        if (Vector3.Distance(_target.position, transform.position) <= ComebackDistance)
        {
            
            // Debug.Log("���� ��ȯ: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
            
        }

        transform.LookAt(StartPosition);
    }

    private void Attack()
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
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;

            Debug.Log("���ȴ�");
        }
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
    private void Damaged()
    {
        // 1. Damage �ִϸ��̼� ����(0.5��)
        // todo: �ִϸ��̼� ����

        // 2. �˹� ����
        // 2-1. �˹� ����/���� ��ġ�� ���Ѵ�.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // 2-2. Lerp�� �̿��� �˹��ϱ�
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            //Debug.Log("���� ��ȯ: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _currentState = MonsterState.Trace;
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
        else
        {
            // �˹� ���·� ��ȯ
            _animator.SetTrigger("Damage"); // �˹� �ִϸ��̼� ����
            _currentState = MonsterState.Damage;
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
}