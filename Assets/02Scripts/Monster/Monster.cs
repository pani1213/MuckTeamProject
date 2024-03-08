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
    private Transform _target;            // 플레이
    private Vector3 StartPosition;         // 시작위치
    public MonsterType _monsterType;      

    public CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public float MoveSpeed       = 5;     // 몬스터 속도
    public float FindDistance    = 5f;    // 감지거리
    public float MoveDistance    = 40f;   // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;  // 오차범위
    public float AttackDistance  = 2f;    // 공격범위
    public int Damage            = 10;    // 공격력
    private float _attackTimer   = 0f;    // 공격타임
    public const float AttackDelay = 1f;  // 공격딜레이

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

        _target = GameObject.FindGameObjectWithTag("Player").transform; // 타겟에다가 플레이어를 넣어줌
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
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 체력바 0 ~ 1

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
            Debug.Log("상태 전환: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            SetRandomPatrolPoint();
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
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
            Debug.Log("상태 전환: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }

    }
    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; // 반경 내에서 무작위 방향 설정
        randomDirection += transform.position; // 몬스터 위치에 더함
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1); // 네비메쉬 상에 유효한 위치 찾기
        _randomPosition = hit.position;
        _navMeshAgent.SetDestination(_randomPosition); // 몬스터 이동 목표 설정
    }
    public void Trace()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.Normalize();
        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션 목적지를 타겟으로 위치
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }

    }
    public void Comeback()
    {
        Vector3 dir = StartPosition - this.transform.position;
        dir.Normalize();

        // 내비게이션이 접근하는 최소 거리를 오차 범위
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // 내비게이션 목적지를 스타트 지점으로
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
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
            Debug.Log("때렸다!");
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



