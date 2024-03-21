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
    Die         // ��� (��������)
}

public class FoodPudu : MonoBehaviour, IHitable
{
    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity; // ���� �ӵ��� ������ ����
    public float gravity = -100f; // �߷°�

    public int FoodPuduHealth;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    private bool isDead = false;

    public Transform _mainCharacter;     // �÷��̾�
    public float MoveSpeed = 1f;         // �̵� ����
    public Vector3 StartPosition;       // ���� ��ġ
    public float MoveDistance = 1f;   // ������ �� �ִ� �Ÿ�
    public const float TOLERANCE = 0.1f; // ������

    private float changeDirectionTime = 2f; // ���� ��ȯ �ð�
    private float directionChangeInterval = 5.0f; // ���� ��ȯ ����
    private float smoothRotationTime = 1.0f; // ���� ��ȯ�� �ҿ�Ǵ� �ð�
    private float directionChangeTimer = 0.0f;
    private Quaternion targetRotation;

    private float damagedCooldownTimer = 0f; // ������ ���� �� ���Է� ��� �ð�
    private readonly float damagedCooldownDuration = 1f; // ��� �ð� ����
    private Vector3 _knockbackStartPosition;    // �˹�
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private FoodPuduState _currentState = FoodPuduState.Idle;
    public GameObject bloodEffectPrefab;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        StartPosition = transform.position;
        FoodPuduHealth = MaxHealth;
        targetRotation = transform.rotation; // �ʱ� ȸ�� ��

    }


    private void Update()
    {
        HealthSliderUI.value = (float)FoodPuduHealth / (float)MaxHealth; // 0 ~ 1

        bool isGrounded = _characterController.isGrounded;
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f; // ���鿡 ������ ���� �ӵ��� 0���� ����
        }

        // �߷� ����
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

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
        // ���/���ۺ��� �ɾ� transform.position ���ݾ� �����ִ� �ڵ�
/*        Debug.Log("idle");
*/
             directionChangeTimer += Time.deltaTime;

            if (directionChangeTimer >= directionChangeInterval)
            {
                directionChangeTimer = 0;
                float angle = Random.Range(0f, 360f);
                targetRotation = Quaternion.Euler(0, angle, 0); // ���ο� ȸ�� �� ����
            }

        // �ε巯�� ȸ��
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / smoothRotationTime);

        // �������θ� �̵�
        Vector3 move = transform.forward * MoveSpeed * Time.deltaTime;
            
            _characterController.Move(move); // �̵�

            // ���� ��ġ�κ��� ���� �Ÿ� �̻� �־����� ���� ��ġ�� ���ư���
            if (Vector3.Distance(transform.position, StartPosition) > MoveDistance)
            {
            // ���� ��ġ�� �ǵ��ư��� ���� �߰�
            Vector3 returnDirection = (StartPosition - transform.position).normalized;
            
           _characterController.Move(returnDirection * MoveSpeed * Time.deltaTime);

            }
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
            _currentState = FoodPuduState.Idle; // ���¸� Idle�� ����
            _animator.SetTrigger("DamagedToIdle"); // Idle �ִϸ��̼����� ��ȯ
            _animator.Play("Idle", 1);
            damagedCooldownTimer = 0f; // ��� �ð� �ʱ�ȭ
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (isDead || damagedCooldownTimer > 0)
        {
            return;
        }
        SoundManager.instance.PlayAudio("Blood1");
        // ������ ������ ���긮��
         BloodFactory.Instance.Make(this.transform.position + Vector3.up, damage.Normal,this.gameObject);

        FoodPuduHealth -= damage.Amount;

        if (FoodPuduHealth <= 0)
        {
            // ü���� 0 ���ϰ� �Ǹ� ��� ó��
            _animator.SetTrigger("IdleToDie"); // Idle���� Die�� ���� ��ȯ
            _animator.Play("Die", 1);
            isDead = true;
            _currentState = FoodPuduState.Die;
        }
        else
        {
            // ������ ���·� ��ȯ�ϰ� �ִϸ��̼� Ʈ���� ����
            _currentState = FoodPuduState.Damaged;
            _animator.SetTrigger("IdleToDamaged");
            _animator.Play("Damaged", 1);
            
        }

        damagedCooldownTimer = damagedCooldownDuration;
    }

    private Coroutine _dieCoroutine;

    private void Die()
    {
        
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        
    }
    private IEnumerator Die_Coroutine()
    {
        HealthSliderUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        // ���� �� ������ ����
        //ItemObjectFactory.Instance.Make(transform.position);

        gameObject.SetActive(false); // Pudu ��Ȱ��ȭ

        PuduGroup puduGroup = GetComponentInParent<PuduGroup>();
        if (puduGroup != null)
        {
            puduGroup.OnPuduDead(); // ��� �˸�
        }


    }
    public void Reinitialize()
    {
        isDead = false;
        FoodPuduHealth = MaxHealth; // ü�� �ʱ�ȭ
        HealthSliderUI.value = 1; // HealthSliderUI�� �ִ밪���� ����
        _currentState = FoodPuduState.Idle; // ���¸� Idle�� �ʱ�ȭ
        damagedCooldownTimer = 0;
        gameObject.SetActive(true); // Pudu Ȱ��ȭ

        HealthSliderUI.gameObject.SetActive(true); // HealthSliderUI�� Ȱ��ȭ
    }

}
