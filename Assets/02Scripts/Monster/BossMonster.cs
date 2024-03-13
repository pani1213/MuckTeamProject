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
    private Transform _target;            // 플레이
    private Vector3 StartPosition;         // 시작위치
    public MonsterType _monsterType;

    public CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public float MoveSpeed = 5;     // 몬스터 속도
    public float FindDistance = 5f;    // 감지거리
    public float ComebackDistance = 10f;
    public float MoveDistance = 40f;   // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;  // 오차범위
    public float AttackDistance = 2f;    // 공격범위
    public int Damage = 10;    // 공격력
    private float _attackTimer = 0f;    // 공격타임
    public  float AttackDelay = 1f;  // 공격딜레이

    public float PatrolRadius = 10f;
    public float IDLE_DURATION = 1f;
    private float _idleTimer;
    private Vector3 _randomPosition;

    // 총알
    public GameObject BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;

    // 넉백 타이머
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private float _comebackStartTime; // Comeback 상태 진입 시간 저장 변수

    private MonsterState _currentState = MonsterState.Idle;
    private void Start()
    {

        _characterController = GetComponent<CharacterController>();
        _characterController.isTrigger = false;


        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // 타겟에다가 플레이어를 넣어줌
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
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 체력바 0 ~ 1

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
            //Debug.Log("상태 전환: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            SetRandomPatrolPoint();
        }*/

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            //Debug.Log("상태 전환: Idle -> Trace");
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
            // Debug.Log("상태 전환: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            // Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }

    }*/

    public void Trace()
    {

        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션 목적지를 타겟으로 위치
        _navMeshAgent.destination = _target.position;

        Debug.Log(Vector3.Distance(transform.position, _target.transform.position));

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
            return;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
            return;
        }


        // NavMeshHit hit;

        // targetPoint 위치에서 Raycast를 수행하여 내비게이션 메쉬 히트 검사
        //if (NavMesh.SamplePosition(_target.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        //{
        //    // 히트가 발생했을 경우 내비게이션 메쉬 상에서 이동 가능한 영역임을 나타냄
        //    Debug.Log("이동 가능한 지점입니다.");
        //}
        //else
        //{
        //    // 히트가 발생하지 않았을 경우 내비게이션 메쉬 상에서 이동 불가능한 영역임을 나타냄
        //    Debug.Log("이동 불가능한 지점입니다.");
        //    _animator.SetTrigger("TraceToComeback");
        //    _currentState = MonsterState.Comeback;
        //}


        transform.LookAt(_target);

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
            // Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            // Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }
        

        if (Vector3.Distance(_target.position, transform.position) <= ComebackDistance)
        {
            
            // Debug.Log("상태 전환: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
            
        }

        transform.LookAt(StartPosition);
    }

    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            //Debug.Log("상태 전환: Attack -> Trace");
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
                //MeleeAttack();
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
            transform.LookAt(_target);
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;

            Debug.Log("때렸다");
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

        Debug.Log("때렸다!");

        _attackTimer = 0f;

    }
    private void Damaged()
    {
        // 1. Damage 애니메이션 실행(0.5초)
        // todo: 애니메이션 실행

        // 2. 넉백 구현
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // 2-2. Lerp를 이용해 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            //Debug.Log("상태 전환: Damaged -> Trace");
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
            // 넉백 상태로 전환
            _animator.SetTrigger("Damage"); // 넉백 애니메이션 실행
            _currentState = MonsterState.Damage;
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
}
