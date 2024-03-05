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
    private Transform _target;            // 
    public float MoveSpeed       = 5;     // ���� �ӵ�
    public float FindDistance    = 5f;    // �����Ÿ�
    public Vector3 StartPosition;         // ������ġ
    public float MoveDistance    = 40f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f;  // �÷��̾�� ���Ͱ��� �Ÿ�
    public float AttackDistance  = 2f;    // ���ݹ���
    public float Damage          = 10;
    private float _attackTimer   = 0f;    // ����Ÿ��
    public const float AttackDelay = 1f;  // ���ݵ�����
    private float _idleTimer;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private MonsterState _currentState = MonsterState.Idle;
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // Ÿ�ٿ��ٰ� �±� �÷��̾ �־���

        StartPosition = transform.position;
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
        }
    }
    public void Idle()
    {
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Trace()
    {

        Vector3 dir = _target.position - this.transform.position;
        dir.Normalize();
        //Vector3 dir = _target.transform.position - this.transform.position;


        // ������̼� �������� Ÿ������ ��ġ
        _navMeshAgent.destination = _target.position;

        if(Vector3.Distance(_target.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }
    }
}

