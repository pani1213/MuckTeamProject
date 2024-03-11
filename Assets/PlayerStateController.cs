using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateController : MonoBehaviour
{
    public Image hp, hungry, stamina;
    public void SetHp()
    {
        //SurvivalGauge.Instance.PlayerHealth
    }
    public void SetHungry(float value)
    {
        Debug.Log(value);
    }
    public void SetStamina(float value)
    {
        Debug.Log(value);
    }
}
