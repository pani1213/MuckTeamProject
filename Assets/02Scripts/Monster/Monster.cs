using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public enum MonsterState
{
    Idle,
    Trace,
    Comeback,
    Attack,
    Die

}

public class Monster : MonoBehaviour, IHitable
{
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    /********************************************************/

    private Transform _target;            // �÷���
    public float MoveSpeed       = 5;     // ���� �ӵ�
    public float FindDistance    = 5f;    // �����Ÿ�
    public Vector3 StartPosition;         // ������ġ
    public float MoveDistance    = 40f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f;  // ��������
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
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform; // Ÿ�ٿ��ٰ� �±� �÷��̾ �־���

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
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Trace()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.Normalize();
       
        // ������̼� �������� Ÿ������ ��ġ
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("���� ��ȯ: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
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

        // �ǽ� ���� 35. Attack ������ �� N�ʿ� �� �� ������ ������ �ֱ�
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            
        }

    }

    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("���ȴ�!");

            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, (int)Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (_currentState == MonsterState.Die)
        {
            return;
        }

        // Todo. ������ Ÿ���� ũ��Ƽ���̸� ���긮��
        if (damage.DamageType == DamageType.Critical)
        {
           
        }
        // Todo. �ǽ� ���� 47: ���带 ���丮�������� �����ϱ� (���� �� Ŭ������: BloodFactory)



        Health -= damage.Amount;
        if (Health <= 0)
        {

            if (Random.Range(0, 2) == 0)
            {
                Debug.Log("���� ��ȯ: Any -> Die1");
                _animator.SetTrigger("Die1");
                _currentState = MonsterState.Die;
            }
            else
            {
                Debug.Log("���� ��ȯ: Any -> Die2");
                _animator.SetTrigger("Die2");
                _currentState = MonsterState.Die;


            }

        }
       
    }
    public void Die()
    {
        
    }
}



