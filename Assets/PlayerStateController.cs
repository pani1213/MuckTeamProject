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
        float PlayerHunger = SurvivalGauge.Instance.PlayerHunger;
        float maxHunger = SurvivalGauge.Instance.Maxhunger;

        float normalizedHunger= Mathf.Clamp(PlayerHunger / maxHunger, 0f, 1f);

        hungry.fillAmount = normalizedHunger;
    }

    public void SetStamina()
    {
        float PlayerStamina = SurvivalGauge.Instance.Stamina;
        float maxStamina = SurvivalGauge.Instance.MaxStamina;

        float normalizedStamina = Mathf.Clamp(PlayerStamina / maxStamina, 0f, 1f);

        stamina.fillAmount = normalizedStamina;
    }

    private void Update()
    {
        SetHp();
        SetHungry();
        SetStamina();
    }
}