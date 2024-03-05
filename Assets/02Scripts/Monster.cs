using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum MonsterState
{
    Idle,
    Trace,
    Comeback

}

public class Monster : MonoBehaviour
{
    private Transform _target;            // 플레이어
    public float MoveSpeed       = 5;     // 몬스터 속도
    public float FindDistance    = 5f;    // 감지거리
    public Vector3 StartPosition;         // 시작위치
    public float MoveDistance    = 40f;   // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;  // 오차범위
    public float AttackDistance  = 2f;    // 공격범위
    public float Damage          = 10;
    private float _attackTimer   = 0f;    // 공격타임
    public const float AttackDelay = 1f;  // 공격딜레이
    private float _idleTimer;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private MonsterState _currentState = MonsterState.Idle;
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // 타겟에다가 태그 플레이어를 넣어줌

        StartPosition = transform.position;

        Init();
    }

    public void Init()
    {
        _idleTimer = 0f;
    }

    public void Update()
    {
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
        }
    }
    public void Idle()
    {
        _idleTimer += Time.deltaTime;
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Trace()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.Normalize();
       
        // 내비게이션 목적지를 타겟으로 위치
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
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

    }
}



