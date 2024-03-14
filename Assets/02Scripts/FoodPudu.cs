using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum FoodPuduState 
{
    Idle,       // ��� (���/���ۺ��� �ȱ�)
    Damaged,    // ���� ���� (�˹�) + ��ȿ��
    Die         // ��� (������ �Ѿ���)
}

public class FoodPudu : MonoBehaviour, IHitable
{
    private CharacterController _characterController;
    private Animator _animator;

    public int FoodPuduHealth;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    public Transform _mainCharacter;     // �÷��̾�
    public float MoveSpeed = 1f;         // �̵� ����
    public Vector3 StartPosition;       // ���� ��ġ
    public float MoveDistance = 1f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f; // ������

    private Vector3 _knockbackStartPosition;    // �˹�
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private FoodPuduState _currentState = FoodPuduState.Idle;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        StartPosition = transform.position;
        FoodPuduHealth = MaxHealth;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)FoodPuduHealth / (float)MaxHealth; // 0 ~ 1

        // ���� ����
        switch (_currentState)
        {
            case FoodPuduState.Idle:
                Idle();
                break;

            case FoodPuduState.Damaged:
                Damaged();
                break;

            case FoodPuduState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        // FoodPudu�� Idle �ִϸ��̼� ���
        _animator.SetTrigger("IdleToDamaged");
        _currentState = FoodPuduState.Damaged;
    }

    private void Damaged()
    {
        // FoodPudu�� Damaged �ִϸ��̼� ���

        // �˹� ����
        if (_knockbackProgress == 0) // �����: 0 (����)
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _mainCharacter.position; // dir: ����
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower; // �˹� ���� �� ��ġ
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // Lerp(��������)�� �̿��� �˹�
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        if (_knockbackProgress > 1)  // �����: 1 (�Ϸ�)
        {
            _knockbackProgress = 0f;
        }

    }

    public void Hit(DamageInfo damage)
    {
        if (_currentState == FoodPuduState.Die)
        {
            return;
        }

        // ������ ������ ���긮��
        //  BloodFactory.Instance.Make(damage.Position, damage.Normal);

        FoodPuduHealth -= damage.Amount;
        if (FoodPuduHealth <= 0)
        {
            //_animator.SetTrigger($"Die{Random.Range(1, 3)}"); // �״� �ִϸ����� ���� 2������ �� 
            _currentState = FoodPuduState.Die;
        }
    }

    private Coroutine _dieCoroutine;

    private void Die()
    {
        if (_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        }
    }
    private IEnumerator Die_Coroutine()
    {

        HealthSliderUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        // ���� �� ������ ����
        //ItemObjectFactory.Instance.Make(transform.position);

        Destroy(gameObject);
    }
}
