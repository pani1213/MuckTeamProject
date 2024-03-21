using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum FoodPuduState 
{
    Idle,       // 대기 (어슬렁/빙글빙글 걷기)
    Damaged,    // 공격 당함 (넉백) + 피효과
    Die         // 사망 (뒤집어짐)
}

public class FoodPudu : MonoBehaviour, IHitable
{
    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity; // 현재 속도를 저장할 변수
    public float gravity = -100f; // 중력값

    public int FoodPuduHealth;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    private bool isDead = false;

    public Transform _mainCharacter;     // 플레이어
    public float MoveSpeed = 1f;         // 이동 상태
    public Vector3 StartPosition;       // 시작 위치
    public float MoveDistance = 1f;   // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f; // 허용오차

    private float changeDirectionTime = 2f; // 방향 전환 시간
    private float directionChangeInterval = 5.0f; // 방향 전환 간격
    private float smoothRotationTime = 1.0f; // 방향 전환에 소요되는 시간
    private float directionChangeTimer = 0.0f;
    private Quaternion targetRotation;

    private float damagedCooldownTimer = 0f; // 데미지 받은 후 재입력 대기 시간
    private readonly float damagedCooldownDuration = 1f; // 대기 시간 설정
    private Vector3 _knockbackStartPosition;    // 넉백
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
        targetRotation = transform.rotation; // 초기 회전 값

    }


    private void Update()
    {
        HealthSliderUI.value = (float)FoodPuduHealth / (float)MaxHealth; // 0 ~ 1

        bool isGrounded = _characterController.isGrounded;
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f; // 지면에 닿으면 수직 속도를 0으로 리셋
        }

        // 중력 적용
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        // 상태 패턴
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
        // FoodPudu의 Idle 애니메이션 재생
        // 어슬렁/빙글빙글 걸어 transform.position 조금씩 변동주는 코드
/*        Debug.Log("idle");
*/
             directionChangeTimer += Time.deltaTime;

            if (directionChangeTimer >= directionChangeInterval)
            {
                directionChangeTimer = 0;
                float angle = Random.Range(0f, 360f);
                targetRotation = Quaternion.Euler(0, angle, 0); // 새로운 회전 값 설정
            }

        // 부드러운 회전
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / smoothRotationTime);

        // 전방으로만 이동
        Vector3 move = transform.forward * MoveSpeed * Time.deltaTime;
            
            _characterController.Move(move); // 이동

            // 시작 위치로부터 일정 거리 이상 멀어지면 원래 위치로 돌아가기
            if (Vector3.Distance(transform.position, StartPosition) > MoveDistance)
            {
            // 시작 위치로 되돌아가는 로직 추가
            Vector3 returnDirection = (StartPosition - transform.position).normalized;
            
           _characterController.Move(returnDirection * MoveSpeed * Time.deltaTime);

            }
    }

    private void Damaged()
    {
        // FoodPudu의 Damaged 애니메이션 재생

        // 넉백 구현
        if (_knockbackProgress == 0) // 진행률: 0 (시작)
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _mainCharacter.position; // dir: 방향
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower; // 넉백 됐을 때 위치
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // Lerp(선형보간)를 이용해 넉백
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        
        if (_knockbackProgress > 1)  // 진행률: 1 (완료)
        {
            _knockbackProgress = 0f;
            _currentState = FoodPuduState.Idle; // 상태를 Idle로 변경
            _animator.SetTrigger("DamagedToIdle"); // Idle 애니메이션으로 전환
            _animator.Play("Idle", 1);
            damagedCooldownTimer = 0f; // 대기 시간 초기화
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (isDead || damagedCooldownTimer > 0)
        {
            return;
        }
        SoundManager.instance.PlayAudio("Blood1");
        // 데미지 입으면 피흘리기
         BloodFactory.Instance.Make(this.transform.position + Vector3.up, damage.Normal,this.gameObject);

        FoodPuduHealth -= damage.Amount;

        if (FoodPuduHealth <= 0)
        {
            // 체력이 0 이하가 되면 사망 처리
            _animator.SetTrigger("IdleToDie"); // Idle에서 Die로 직접 전환
            _animator.Play("Die", 1);
            isDead = true;
            _currentState = FoodPuduState.Die;
        }
        else
        {
            // 데미지 상태로 전환하고 애니메이션 트리거 설정
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

        // 죽을 때 아이템 생성
        //ItemObjectFactory.Instance.Make(transform.position);

        gameObject.SetActive(false); // Pudu 비활성화

        PuduGroup puduGroup = GetComponentInParent<PuduGroup>();
        if (puduGroup != null)
        {
            puduGroup.OnPuduDead(); // 사망 알림
        }


    }
    public void Reinitialize()
    {
        isDead = false;
        FoodPuduHealth = MaxHealth; // 체력 초기화
        HealthSliderUI.value = 1; // HealthSliderUI를 최대값으로 설정
        _currentState = FoodPuduState.Idle; // 상태를 Idle로 초기화
        damagedCooldownTimer = 0;
        gameObject.SetActive(true); // Pudu 활성화

        HealthSliderUI.gameObject.SetActive(true); // HealthSliderUI를 활성화
    }

}
