using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 생존 게이지: 플레이어의 체력,허기, 스태미나
public class SurvivalGauge : MonoBehaviour, IHitable
{
    // 체력
    public int PlayerHealth = 100; // 하트 이미지 S2
    public int Maxhealth = 100;

    // 허기
    public int PlayerHunger = 100; // 치킨 이미지 @
    public int Maxhunger = 100;
    public float HungerTime = 100f;
    public float _hungerTimer = 0f;

    // 스태미나
    public float MoveSpeed = 5;
    public float RunSpeed = 10;
    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량
    public bool _isStamina = true;

    private CharacterController _characterController;


    void Start()
    {
        PlayerHealth = Maxhealth;
        PlayerHunger = Maxhunger;
    }
    public void Hit(DamageInfo damageInfo)
    {
        PlayerHealth -= damageInfo.Amount;

        if (PlayerHealth <= 0)
        {
            gameObject.SetActive(false);          
        }
    }

    private void FastMove()
    {
        float h = Input.GetAxis("Horizontal"); // 좌우(방향키 왼쪽/오른쪽) 
        float v = Input.GetAxis("Vertical"); // 수직(방향키 위/아래) 

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // Shift 누르면 빨리 뛰기
        if (Input.GetKey(KeyCode.LeftShift) && _isStamina == true)
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;
            dir *= RunSpeed;
        }
        else
        {
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
            dir *= MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100); // 값이 넘어가지 않도록
        //_characterController.Move(dir * Time.deltaTime);
    }



    void Update()
    {
        FastMove(); // 스태미나

        // Hunger를 _hungerTimer가 증가함에 따라(100까지) 100에서 0이 되도록 하기
        // && 밥 아이템을 먹으면 Hunger가 늘어나도록
        // Hunger 값이 0이 되면 -> 스태미나 안차도록

        _hungerTimer += Time.deltaTime;

        if( _hungerTimer > HungerTime) // 0에서 시작해서 100을 넘으면
        {
            PlayerHunger = 0;   // 자연스럽게 쭉쭉 0까지 떨어지도록 구현필요
            _isStamina = false; // 뭘 먹으면 올라갈 수 있게 bool 값 쓰자!

        }
        // if(소비 아이템을 먹었을 때)
        {
            _isStamina = true;
            // PlayerHunger 아이템 성능만큼 증가 / _hungerTimer 아이템 성능만큼 감소
        }



    }
}
