using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾� ���� ������: �÷��̾��� ü��,���, ���¹̳�
public class SurvivalGauge : MonoBehaviour, IHitable 
{
    public static SurvivalGauge Instance { get; private set; }
    public static bool IsPlayerDead = false;
    public GameObject mainCamera;
    public UI_OptionPopup uiOptionPopup;
    public PlayerMoveAbility PlayerMoveAbility;

    // ü��
    public int PlayerHealth = 100; // ��Ʈ �̹��� S2
    public int Maxhealth = 100;
    public int Damage = 10;        // ���ݷ�
    public float AttackSpeed = 1f;
    public float attackCooldown = 0f;
    public int Defense = 0;        // ����
    public bool hasRegenApplied = false; 
    public int RegenAmount = 1;
    public Image damageEffectUI; // Inspector���� �Ҵ�
    public float damageEffectDuration = 1.0f; // ������ ȿ�� ���� �ð�

    public bool hasLifesteal = false;
    public float lifestealPercentage = 0.1f;
    public GameObject tombstonePrefab;
    public DeathCamera deathCamera;

    // ���
    public int PlayerHunger = 100; // ġŲ �̹��� @
    public int Maxhunger = 100;
    public float _hungerTimer = 0f;
    public float hungerDecayTime = 5f;

    // ���¹̳�
    public float RunSpeed = 15;
    public float Stamina = 100;             // ���¹̳�
    public float MaxStamina = 100;    
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������
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
        deathCamera = Camera.main.GetComponent<DeathCamera>();
        uiOptionPopup = FindObjectOfType<UI_OptionPopup>();
        PlayerHealth = Maxhealth;
        PlayerHunger = Maxhunger;
        Stamina = MaxStamina;
        float MoveSpeed = PlayerMoveAbility.MoveSpeed;

        damageEffectUI.enabled = false;
        
    }
    void Update()
    {
        FastMove(); // ���¹̳�
        UpdateHunger();

        if (!hasRegenApplied && RegenAmount > 0)
        {
            PlayerHealth += RegenAmount;
            hasRegenApplied = true; 

            if (PlayerHealth > Maxhealth)
            {
                PlayerHealth = Maxhealth;
            }
        }

    }

    public void ApplyRegen(int amount)
    {
        RegenAmount = amount; // Regen �� ����
        hasRegenApplied = false; // ���� Regen ȿ���� �������� �ʾ����Ƿ�, false�� ����
    }

    public void Hit(DamageInfo damageInfo)
    {
        PlayerHealth -= damageInfo.Amount - Defense;
        // �÷��̾� ������ ���� ������ ���� ���� ���� Ŀ���Բ� UI
        
        StartCoroutine(DamageEffectCoroutine());
        Camera.main.GetComponent<CameraShake>().Shake();

        if (PlayerHealth <= 0)
        {
            if (!IsPlayerDead)
            {
                SoundManager.instance.bgmSource.clip = SoundManager.instance.GetAudioClip("GameOver");
                SoundManager.instance.bgmSource.Play();
            }
            // ������ ��������� / �������� ī�޶� ���� ������ ����(ī�޶�� DeathCamera ��ũ��Ʈ����)
            // GameOver UI ����

            IsPlayerDead = true;
            GameObject tombstoneInstance = Instantiate(tombstonePrefab, transform.position, Quaternion.identity);
            deathCamera.target = tombstoneInstance.transform;

            mainCamera.transform.parent = null;
            deathCamera.OrbitAroundTarget();

            gameObject.SetActive(false); // �÷��̾� ���
            uiOptionPopup.ShowGameOver();
        }
    }

    private IEnumerator DamageEffectCoroutine()
    {
        float time = 0;
        float maxAlpha = 0.4f;
        damageEffectUI.enabled = true;

        while (time < damageEffectDuration)
        {
            // ������ ȿ�� �� ���� ���� ���� ������Ŵ
            float alpha = Mathf.Lerp(0, maxAlpha, time / damageEffectDuration);
            damageEffectUI.color = new Color(1, 0, 0, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        yield return null;
        // ������ ȿ�� ���� �� ���� ���� �ٽ� 0���� ����
        damageEffectUI.color = new Color(1, 0, 0, 0);
        damageEffectUI.enabled = false;

    }

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
            dir *= PlayerMoveAbility.MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina); // ���� �Ѿ�� �ʵ���
        _characterController.Move(dir * Time.deltaTime);
    }
    private void UpdateHunger()
    {
        // Hunger�� _hungerTimer�� �����Կ� ����(100����) 100���� 0�� �ǵ��� �ϱ�
        // && �� �������� ������ Hunger ��ġ�� �þ����
        // Hunger ���� 0�� �Ǹ� -> ���¹̳� ��������

        _hungerTimer += Time.deltaTime;

        if (_hungerTimer >= hungerDecayTime)
        {
            PlayerHunger = Mathf.Max(0, PlayerHunger - 1); // ��� ����
            _hungerTimer = 0; // Ÿ�̸� ����

            if (PlayerHunger == 0)
            {
                _isStamina = false; // ��Ⱑ 0�� �Ǹ� ���¹̳� ȸ�� ��Ȱ��ȭ
            }
            else 
            {
                _isStamina = true;
            }
        }
        
    }
}
