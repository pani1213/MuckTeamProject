using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ���� ������: �÷��̾��� ü��,���, ���¹̳�
public class SurvivalGauge : MonoBehaviour, IHitable
{
    // ü��
    public int PlayerHealth = 100; // ��Ʈ �̹��� S2
    public int Maxhealth = 100;

    // ���
    public int PlayerHunger = 100; // ġŲ �̹��� @
    public int Maxhunger = 100;
    public float HungerTime = 100f;
    public float _hungerTimer = 0f;

    // ���¹̳�
    public float MoveSpeed = 5;
    public float RunSpeed = 10;
    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������
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
        float h = Input.GetAxis("Horizontal"); // �¿�(����Ű ����/������) 
        float v = Input.GetAxis("Vertical"); // ����(����Ű ��/�Ʒ�) 

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // Shift ������ ���� �ٱ�
        if (Input.GetKey(KeyCode.LeftShift) && _isStamina == true)
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;
            dir *= RunSpeed;
        }
        else
        {
            Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
            dir *= MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100); // ���� �Ѿ�� �ʵ���
        //_characterController.Move(dir * Time.deltaTime);
    }



    void Update()
    {
        FastMove(); // ���¹̳�

        // Hunger�� _hungerTimer�� �����Կ� ����(100����) 100���� 0�� �ǵ��� �ϱ�
        // && �� �������� ������ Hunger�� �þ����
        // Hunger ���� 0�� �Ǹ� -> ���¹̳� ��������

        _hungerTimer += Time.deltaTime;

        if( _hungerTimer > HungerTime) // 0���� �����ؼ� 100�� ������
        {
            PlayerHunger = 0;   // �ڿ������� ���� 0���� ���������� �����ʿ�
            _isStamina = false; // �� ������ �ö� �� �ְ� bool �� ����!

        }
        // if(�Һ� �������� �Ծ��� ��)
        {
            _isStamina = true;
            // PlayerHunger ������ ���ɸ�ŭ ���� / _hungerTimer ������ ���ɸ�ŭ ����
        }



    }
}
