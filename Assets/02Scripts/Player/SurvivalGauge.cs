using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 생존 게이지: 플레이어의 체력,허기, 스태미나
public class SurvivalGauge : MonoBehaviour, IHitable
{
    public static SurvivalGauge Instance { get; private set; }

    // 체력
    public int PlayerHealth = 100; // 하트 이미지 S2
    public int Maxhealth = 100;
    public int Defense = 0;        // 방어력

    // 허기
    public float PlayerHunger = 100; // 치킨 이미지 @
    public float Maxhunger = 100;
    public float _hungerTimer = 0f;
    public float hungerDecayTime = 10f;

    // 스태미나
    public float MoveSpeed = 5;
    public float RunSpeed = 15;
    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량
    public bool _isStamina = true;
    private bool _isRunning = false;

    private CharacterController _characterController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        PlayerHealth = Maxhealth;
        PlayerHunger = Maxhunger;
    }
    void Update()
    {
        FastMove(); // 스태미나
        UpdateHunger();
    }
    public void Hit(DamageInfo damageInfo)
    {
        
        PlayerHealth -= damageInfo.Amount - Defense;

        if (PlayerHealth <= 0)
        {
            gameObject.SetActive(false); // 플레이어 사망
        }
    }

    // 구현 필요) 점프하면 스태미나 닳고, 스태미나 바닥이면 점프도 안됨
    private void FastMove()
    {
        if (!_characterController) return;

        float h = Input.GetAxis("Horizontal"); // 좌우(방향키 왼쪽/오른쪽) 
        float v = Input.GetAxis("Vertical"); // 수직(방향키 위/아래) 

        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        // Shift 누르면 빨리 뛰기
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && _isStamina)
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;
            dir *= RunSpeed;
            _isRunning = true;
        }
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift)) 
            {
                _isRunning = false; 
            }
            if (!_isRunning && _isStamina)
            {
                Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
            }
            dir *= MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina); // 값이 넘어가지 않도록
        _characterController.Move(dir * Time.deltaTime);
    }

    private void UpdateHunger()
    {
        // Hunger를 _hungerTimer가 증가함에 따라(100까지) 100에서 0이 되도록 하기
        // && 밥 아이템을 먹으면 Hunger 수치가 늘어나도록
        // Hunger 값이 0이 되면 -> 스태미나 안차도록

        _hungerTimer += Time.deltaTime;
        if (_hungerTimer >= hungerDecayTime) // _hungerTimer가 10초 증가함에 따라 허기가 감소하는 코드 
        {
            PlayerHunger = Mathf.Max(0, PlayerHunger - 1); // 허기 감소
            _hungerTimer = 0;
        }

        if (PlayerHunger <= 0)
        {
                _isStamina = false; // 허기가 0이 되면 스태미나 회복 비활성화
        }
        
        // if(소비 아이템을 먹었을 때)
        {
            _isStamina = true;
            //PlayerHunger += 아이템 성능; // 아이템 성능만큼 허기 채워짐 증가
            //_hungerTimer -= 아이템 성능; // 아이템 성능만큼 허기 타이머 감소
        }
    }
}
