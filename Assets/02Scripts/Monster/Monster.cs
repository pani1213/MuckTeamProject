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
    Damaged,
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
    public  float AttackDelay = 1f;  // 공격딜레이
    public const float _rotationSpeed = 100f;

    private float _idleTimer;
    private Vector3 _randomPosition;

    public GameObject[] BulletPrefab;
    public Transform BulletPoint;
    public float BulletSpeed = 10f;

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public MeshRenderer hatRander;
    public Material[] skins;
    public Material[] hats;

    public MonsterRespawner monsterRespawner;

    private MonsterState _currentState = MonsterState.Idle;
    Rigidbody myRigidbody;
    private void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody>();
        if (SkinnedMeshRenderer != null)
        {
            SkinnedMeshRenderer.material = skins[Random.Range(0, skins.Length)];
            hatRander.material = hats[Random.Range(0, hats.Length)];

        }

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
        if (_target != null && Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    public void Trace()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0f;
        dir.Normalize();

        // 몬스터가 플레이어를 바라보도록 회전
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션 목적지를 타겟으로 위치
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            //Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            //Debug.Log("상태 전환: Trace -> Attack");
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

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
           // Debug.Log("상태 전환: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    private bool IsObstacleBetween()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, _target.position, out hit))
        {
            // 장애물이 있으면 true 반환
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
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
            if (_monsterType == MonsterType.Melee)
            {
                //MeleeAttack();
                _animator.SetTrigger("Attack");
            }
            
            if (_monsterType == MonsterType.LongRange)
            {
                // 플레이어가 공격 범위 내에 있고 장애물이 없을 때만 공격
                if (FindDistance <= AttackDistance && !IsObstacleBetween())
                {
                    LongRangeAttack();
                    _animator.SetTrigger("LongAttack");
                }
                // 아니면 플레이어를 향해 이동
                else if (FindDistance <= AttackDistance)
                {
                    _navMeshAgent.SetDestination(_target.position);
                }
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
            Debug.Log("때렸다");

            transform.LookAt(_target);
        }
    }
    
   public void LongRangeAttack()
    {
        Vector3 targetDirection = (_target.position - BulletPoint.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(targetDirection);
        bulletRotation *= Quaternion.Euler(-90f, 0f, 0f); // x값을 -90으로 설정
        transform.LookAt(_target);
        //Quaternion bulletRotation = Quaternion.Euler(-90f, 0, 0f);

        int randomIndex = Random.Range(0, BulletPrefab.Length);
        GameObject bullet = Instantiate(BulletPrefab[randomIndex], BulletPoint.position, bulletRotation);
        Bullet bulletdamage = bullet.GetComponent<Bullet>();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        bulletdamage.info = new DamageInfo(DamageType.Normal, Damage);
        Vector3 bulletDir = new Vector3(_target.position.x, _target.position.y +2f, _target.position.z) - BulletPoint.position;
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
           // monsterRespawner.OnMonsterDeath();
        }
        else
        {
            // 넉백 상태로 전환
            _animator.SetTrigger("Damaged"); // 넉백 애니메이션 실행
            _currentState = MonsterState.Damaged;
        }
    }
    public void Die()
    {
        // iteminstance
        ItemObjectScript item = Instantiate(ItemInfoManager.instance.itemdic[1022]);
        item.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        item.InIt(1022,UnityEngine.Random.Range(1,5),ItemType.Item);

        gameObject.SetActive(false);
        PoolingManager.instance.ReturnToPool(gameObject);
    }


}



