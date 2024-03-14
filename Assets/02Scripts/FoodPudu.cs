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
    Die         // 사망 (옆으로 넘어짐)
}

public class FoodPudu : MonoBehaviour, IHitable
{
    private CharacterController _characterController;
    private Animator _animator;

    public int FoodPuduHealth;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    public Transform _mainCharacter;     // 플레이어
    public float MoveSpeed = 1f;         // 이동 상태
    public Vector3 StartPosition;       // 시작 위치
    public float MoveDistance = 1f;   // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f; // 허용오차

    private Vector3 _knockbackStartPosition;    // 넉백
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
        _animator.SetTrigger("IdleToDamaged");
        _currentState = FoodPuduState.Damaged;
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
        }

    }

    public void Hit(DamageInfo damage)
    {
        if (_currentState == FoodPuduState.Die)
        {
            return;
        }

        // 데미지 입으면 피흘리기
        //  BloodFactory.Instance.Make(damage.Position, damage.Normal);

        FoodPuduHealth -= damage.Amount;
        if (FoodPuduHealth <= 0)
        {
            //_animator.SetTrigger($"Die{Random.Range(1, 3)}"); // 죽는 애니메이터 종류 2가지일 때 
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

        // 죽을 때 아이템 생성
        //ItemObjectFactory.Instance.Make(transform.position);

        Destroy(gameObject);
    }
}
