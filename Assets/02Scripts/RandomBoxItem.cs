using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxItem : MonoBehaviour
{

    public SurvivalGauge SurvivalGauge;
    public PlayerFireAbility PlayerFireAbility;
    public PlayerMoveAbility PlayerMoveAbility;
    public Monster Monster;

    public int id;
    public string type;
    public int value;


    public void InIt(int _boxItemId, string _type, int _value)
    {
        id = _boxItemId;
        type = _type;
        value = _value;
    }
    public void GetItem()
    {
        switch (type)
        {
            case "maxhp":
                ApplymaxHp(value);
                break;

            case "stamina":
                ApplyStamina(value);
                break;

            case "power":
                ApplyPower(value);
                break;

            case "hp":
                ApplyHp(value);
                break;

            case "speed":
                ApplySpeed(value);
                break;

            case "attackSpeed":
                ApplyAttackSpeed(value);
                break;

            case "jumpPower":
                ApplyJumpPower(value);
                break;

            case "hunger":
                ApplyHunger(value);
                break;

            case "defense":
                ApplyDefense(value);
                break;

            case "Lifesteal":
                ApplyLifesteal(value);
                break;

        }

        gameObject.SetActive(false);
    }

    private void ApplymaxHp(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1001].imageFileName);
        SurvivalGauge.Maxhealth += value;
    }
    private void ApplyStamina(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1002].imageFileName);
        SurvivalGauge.Stamina += value;
    }
    private void ApplyPower(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1003].imageFileName);
        PlayerFireAbility.Damage += value;
    }

    private void ApplyHp(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1004].imageFileName);
        SurvivalGauge.PlayerHealth += value;
    }

    private void ApplySpeed(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1005].imageFileName);
        PlayerMoveAbility.MoveSpeed += value;
    }
    private void ApplyAttackSpeed(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1006].imageFileName);
        PlayerFireAbility.AttackSpeed += value;
    }
    private void ApplyJumpPower(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1007].imageFileName);
        PlayerMoveAbility.JumpPower += value;
    }
    private void ApplyHunger(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1008].imageFileName);
        SurvivalGauge.Maxhunger += value;
    }
    private void ApplyDefense(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1009].imageFileName);
        SurvivalGauge.Defense += value;
    }
    private void ApplyLifesteal(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1010].imageFileName);
        if(Monster.Health <= Monster.MaxHealth) 
        {
            SurvivalGauge.PlayerHealth += value;
            Monster.Health -= value;
        }
        
    }
}
