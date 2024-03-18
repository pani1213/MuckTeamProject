using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateController : MonoBehaviour
{
    public Image hp, hungry, stamina;

    public void SetHp()
    {
        float PlayerHealth = SurvivalGauge.Instance.PlayerHealth;
        float maxHealth = SurvivalGauge.Instance.Maxhealth; 

        float normalizedHealth = Mathf.Clamp(PlayerHealth / maxHealth, 0f, 1f);

        hp.fillAmount = normalizedHealth;
    }

    public void SetHungry()
    {
        hungry.fillAmount = SurvivalGauge.Instance.PlayerHunger * 0.01f;
    }

    public void SetStamina()
    {
        stamina.fillAmount = SurvivalGauge.Instance.Stamina * 0.01f;
    }

    private void Update()
    {
        SetHp();
        SetHungry();
        SetStamina();
    }
}