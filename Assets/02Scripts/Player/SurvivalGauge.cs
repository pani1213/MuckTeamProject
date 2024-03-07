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
    public float RunSpeed = 15;
    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������
    public bool _isStamina = true;
    private bool _isRunning = false; 

    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        PlayerHealth = Maxhealth;
        PlayerHunger = Maxhunger;
    }
    void Update()
    {
        FastMove(); // ���¹̳�
        UpdateHunger();
    }
    public void Hit(DamageInfo damageInfo)
    {
        
        PlayerHealth -= damageInfo.Amount;

        if (PlayerHealth <= 0)
        {
            gameObject.SetActive(false); // �÷��̾� ���
        }
    }

    // ���� �ʿ�) �����ϸ� ���¹̳� ���, ���¹̳� �ٴ��̸� ������ �ȵ�
    private void FastMove()
    {
        if (!_characterController) return;

        float h = Input.GetAxis("Horizontal"); // �¿�(����Ű ����/������) 
        float v = Input.GetAxis("Vertical"); // ����(����Ű ��/�Ʒ�) 

        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        // Shift ������ ���� �ٱ�
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
                Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
            }
            dir *= MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina); // ���� �Ѿ�� �ʵ���
        _characterController.Move(dir * Time.deltaTime);
    }

    private void UpdateHunger()
    {
        // Hunger�� _hungerTimer�� �����Կ� ����(100����) 100���� 0�� �ǵ��� �ϱ�
        // && �� �������� ������ Hunger�� �þ����
        // Hunger ���� 0�� �Ǹ� -> ���¹̳� ��������

        _hungerTimer += Time.deltaTime;

        if (_hungerTimer > HungerTime) 
        {
            PlayerHunger = Mathf.Max(0, PlayerHunger - 1); // ��� ����
            _hungerTimer = 0; // Ÿ�̸� ����

            if (PlayerHunger <= 0)
            {
                _isStamina = false; // ��Ⱑ 0�� �Ǹ� ���¹̳� ȸ�� ��Ȱ��ȭ
            }
        }
        // if(�Һ� �������� �Ծ��� ��)
        {
            _isStamina = true;
            // PlayerHunger ������ ���ɸ�ŭ ���� / _hungerTimer ������ ���ɸ�ŭ ����
        }
    }
}
