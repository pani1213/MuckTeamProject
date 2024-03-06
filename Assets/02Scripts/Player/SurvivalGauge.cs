using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 생존 게이지: 플레이어의 체력,허기 (+ 스태미나)
public class SurvivalGauge : MonoBehaviour, IHitable
{

    public int PlayerHealth    = 100; // 하트 이미지 S2
    public int Maxhealth = 100;
    public int Hunger    = 100; // 치킨 이미지 @
    public int Maxhunger = 100;
    public float HungerTime = 0f;
    private float _hungerTimer;

    public void Hit(DamageInfo damageInfo)
    {
        
        PlayerHealth -= damageInfo.Amount;

        if (PlayerHealth <= 0)
        {
            gameObject.SetActive(false); // 플레이어 사망
        }
    }

    void Start()
    {
        PlayerHealth = Maxhealth;
        Hunger = Maxhunger;
    }

    void Update()
    {
        // Hunger를 HungerTimer가 증가함에 따라(100정도까지 증가) 100에서 0이 될 수 있도록 하기
        // && 밥 아이템을 먹으면 Hunger가 늘어나도록

        _hungerTimer += Time.deltaTime;

        if( _hungerTimer > Hunger)
        {

        }
        _hungerTimer = 0;
    }
}
