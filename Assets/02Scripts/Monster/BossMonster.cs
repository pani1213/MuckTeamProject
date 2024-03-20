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
    private Transform _target;            // 플레이
    private Vector3 StartPosition;         // 시작위치
    public MonsterType _monsterType;

    public CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public float MoveSpeed = 5;           // 몬스터 속도
    public float FindDistance = 5f;       // 감지거리
    public float ComeDistance = 5f;       // 감지거리
    public float MoveDistance = 40f;      // 움직일 수 있는 거리
    public float _moveDistanceRemaining;  // 맞았을 때 거리 초기화
    public const float TOLERANCE = 0.1f;  // 오차범위
    public float AttackDistance = 2f;     // 공격범위
    public int Damage = 10;    // 공격력
    private float _attackTimer = 0f;    // 공격타임
    public float AttackDelay = 1f;  // 공격딜레이
    public float HitDistance = 10f;
    private float _bossDestroyTime = 4f;
    public Vector3 FirstPosition;


    public float PatrolRadius = 10f;
    public float IDLE_DURATION = 1f;
    private float _idleTimer;


    // 총알
    public GameObject BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;


    public float rotationSpeed = 5f; // 회전 속도
    private bool isRotating = false; // 회전 상태

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
        HealthSliderUI.value = (float)Health / (float)MaxHealth; // 체력바 0 ~ 1

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
            //Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Trace()
    {

        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션 목적지를 타겟으로 위치
        _navMeshAgent.destination = _target.position;

        //Debug.Log(Vector3.Distance(transform.position, _target.transform.position));

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

        // 공격 가능한 상태이고, 플레이어와의 거리가 공격 가능 범위 내에 있는지 확인
        if (Vector3.Distance(_target.position, transform.position) <= ComeDistance)
        {
            //Debug.Log("상태 전환: Comeback -> Attack");
            Debug.Log($"스타트 바뀸{transform.position}");
            StartPosition = transform.position;
            _navMeshAgent.destination = StartPosition;
            _animator.SetTrigger("ComebackToAttack");
            _currentState = MonsterState.Attack;
        }

        transform.LookAt(StartPosition);

        // 피 회복 타이머 갱신
        HealthRegenTime += Time.deltaTime;
        if (HealthRegenTime >= HealthRegenTimeInterval)
        {
            // 일정 시간마다 피를 회복
            Health = Mathf.Min(MaxHealth, Health + HealthRegen);
            HealthRegenTime = 0.0f;
        }
    }
    public void Attack()
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
         // Attack1에 대한 사운드 재생
        }
        if (a == 2)
        {
            _animator.SetTrigger("Attack2");
           // Attack2에 대한 사운드 재생
        }
        if (a == 0)
        {
            _animator.SetTrigger("Attack3");
          // Attack3에 대한 사운드 재생
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
                SoundManager.instance.PlayAudio("Attack1"); // Attack1에 대한 사운드 재생
                break;
            case 2:
        Debug.Log(1);
                SoundManager.instance.PlayAudio("Attack2"); // Attack2에 대한 사운드 재생
                break;
            case 3:
        Debug.Log(1);
                SoundManager.instance.PlayAudio("Attack3"); // Attack3에 대한 사운드 재생
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

            Debug.Log("때렸다");
        }
    }
    private void RotateAndMoveTowardsTarget(Vector3 directionToTarget)
    {
        // 회전 상태로 변경
        isRotating = true;

        // 플레이어를 정확히 향하도록 방향 보정
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        // 부드러운 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 회전이 끝나면 이동
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            // 이동
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowardsTarget(Vector3 directionToTarget)
    {
        // 회전 상태로 변경
        isRotating = true;

        // 플레이어를 정확히 향하도록 방향 보정
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        // 부드러운 회전
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

        Debug.Log("때렸다!");

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

        // 현재 상태가 트레이스 상태인지 확인하고, 트레이스 상태일 때에만 공격을 수행
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
