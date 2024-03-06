using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ���� ������: �÷��̾��� ü��,��� (+ ���¹̳�)
public class SurvivalGauge : MonoBehaviour, IHitable
{

    public int PlayerHealth    = 100; // ��Ʈ �̹��� S2
    public int Maxhealth = 100;
    public int Hunger    = 100; // ġŲ �̹��� @
    public int Maxhunger = 100;
    public float HungerTime = 0f;
    private float _hungerTimer;

    public void Hit(DamageInfo damageInfo)
    {
        
        PlayerHealth -= damageInfo.Amount;

        if (PlayerHealth <= 0)
        {
            gameObject.SetActive(false); // �÷��̾� ���
        }
    }

    void Start()
    {
        PlayerHealth = Maxhealth;
        Hunger = Maxhunger;
    }

    void Update()
    {
        // Hunger�� HungerTimer�� �����Կ� ����(100�������� ����) 100���� 0�� �� �� �ֵ��� �ϱ�
        // && �� �������� ������ Hunger�� �þ����

        _hungerTimer += Time.deltaTime;

        if( _hungerTimer > Hunger)
        {

        }
        _hungerTimer = 0;
    }
}
