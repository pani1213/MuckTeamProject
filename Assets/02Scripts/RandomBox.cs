using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BoxItemType
{
    maxhp = 1001,
    stamina,
    power,
    hp,
    speed,
    attackSpeed,
    jumpPower,
    hunger,
    defense,
    Lifesteal
}
public class RandomBox : MonoBehaviour
{
    public UI_BoxItem UI_BoxItem;

    public Animation animation;
    public BoxCollider BoxCollider;
    //public Animator Animator;
    public Transform ItemPos;

    public GameObject Avocado;
    public GameObject Bread;
    public GameObject Carrot;
    public GameObject Broccoli;
    public GameObject Banana;
    public GameObject Garlic;
    public GameObject Fish;
    public GameObject Pumpkin;
    public GameObject Pepper;
    public GameObject Pear;

    private bool isOpenChestPlayed = false;
    private bool isItemCreated = false;
    private bool isPlayerNear = false;
    private bool isOpened = false;

    Monster Monster;

    public int id;
    public string type;
    public float value;

    public void InIt(int _boxItemId, string _type, int _value)
    {
        id = _boxItemId;
        type = _type;
        value = _value;
    }


    void Update()
    {
        if (isPlayerNear && Input.GetKey(KeyCode.E) && !isOpened)
        {
            if (!isOpenChestPlayed) // 열었고
            {
                animation.Play("OpenChest");
                isOpened = true;
            
                MakePercent(transform.position);
                ApplyEffect((BoxItemType)id,(int)JsonParsingManager.instance.boxItemDic[id].value);
                isOpenChestPlayed = true;
            }

            else if (isOpenChestPlayed && !animation.isPlaying && !isItemCreated) // 스탯 적용시
            {
                if (!animation.IsPlaying("OpenChest"))
                {
                    StartCoroutine(Rotate_Coroutine());

                    // 스탯 적용되는 UI 입력되면 destroy 되도록 추가 예정
                    isItemCreated = true;
                }
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isOpened)
        {
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private IEnumerator Rotate_Coroutine()
    {

        animation.Play("SpinObj");
        yield return new WaitForSeconds(2f);

    }


    public void MakePercent(Vector3 position)
    {
         int percentage = UnityEngine.Random.Range(0, 100);
         Debug.Log("Random Percentage: " + percentage);
            if (percentage <= 10)
           {
               Make(BoxItemType.maxhp, position);
           }
           else if (percentage <= 20)
        {
            Make(BoxItemType.stamina, position);
          }
         else if (percentage <= 30)
        {
            Make(BoxItemType.power, position);
         }
        else if (percentage <= 40)
        {
            Make(BoxItemType.hp, position);
        }
         else if (percentage <= 50)
         {
             Make(BoxItemType.speed, position);
         }
         else if (percentage <= 60)
        {
            Make(BoxItemType.attackSpeed, position); 
         }
        else if (percentage <= 70)
        {
            Make(BoxItemType.jumpPower, position);
        }
         else if (percentage <= 80)
        {
            Make(BoxItemType.hunger, position);
       }
        else if (percentage <= 90)
        {
            Make(BoxItemType.defense, position);
    }
    else if (percentage <= 100)
        {
            Make(BoxItemType.Lifesteal, position);
    }
    }

    public void Make(BoxItemType itemType, Vector3 position)
    {
        Debug.Log("Creating item of type: " + itemType.ToString());
        GameObject itemToCreate = null;
        id = (int)itemType;
        switch (itemType)
        {
            case BoxItemType.maxhp:
                itemToCreate = Avocado;
                break;

            case BoxItemType.stamina:
                itemToCreate = Bread;
                break;

            case BoxItemType.power:
                itemToCreate = Carrot;
                break;

            case BoxItemType.hp:
                itemToCreate = Broccoli;
                break;

            case BoxItemType.speed:
                itemToCreate = Banana;
                break;

            case BoxItemType.attackSpeed:
                itemToCreate = Garlic;
                break;

            case BoxItemType.jumpPower:
                itemToCreate = Fish;
                break;

            case BoxItemType.hunger:
                itemToCreate = Pumpkin;
                break;

            case BoxItemType.defense:
                itemToCreate = Pepper;
                break;

            case BoxItemType.Lifesteal:
                itemToCreate = Pear;
                break;
               
        }
        UI_BoxItem.instance.Refresh_TextUI(id);
        if (itemToCreate != null)
        {
            Instantiate(itemToCreate, ItemPos.position, Quaternion.identity);
            Debug.Log(itemType.ToString() + "를 얻었습니다.");
            UI_BoxItem.ShowBoxItem(itemType);

            //ApplyEffect(itemType, value);
        }
    }
    private void ApplyEffect(BoxItemType itemType, float amount)
    {
        switch (itemType)
        {
            case BoxItemType.maxhp:
                ApplymaxHp((int)amount);
                break;

            case BoxItemType.stamina:
                ApplyStamina((int)amount);
                break;

            case BoxItemType.power:
                ApplyPower((int)amount);
                break;

            case BoxItemType.hp:
                SurvivalGauge.Instance.ApplyRegen(1);
                break;

            case BoxItemType.speed:
                ApplySpeed((int)amount);
                break;

            case BoxItemType.attackSpeed:
                ApplyAttackSpeed(amount);
                break;

            case BoxItemType.jumpPower:
                ApplyJumpPower((int)amount);
                break;

            case BoxItemType.hunger:
                ApplyHunger((int)amount);
                break;

            case BoxItemType.defense:
                ApplyDefense((int)amount);
                break;

            case BoxItemType.Lifesteal:
                ApplyLifesteal((int)amount);
                break;
        }
    }
    private void ApplymaxHp(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1001].imageFileName);
        SurvivalGauge.Instance.Maxhealth += amount;
    }
    private void ApplyStamina(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1002].imageFileName);
        SurvivalGauge.Instance.MaxStamina += amount;
    }
    private void ApplyPower(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1003].imageFileName);
        SurvivalGauge.Instance.Damage += amount;
    }

    private void ApplyHp(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1004].imageFileName);
        SurvivalGauge.Instance.RegenAmount += amount;
    }

    private void ApplySpeed(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1005].imageFileName);
        PlayerMoveAbility.Instance.MoveSpeed += amount;
    }
    private void ApplyAttackSpeed(float amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1006].value);
        if(SurvivalGauge.Instance.AttackSpeed <= 0.5 ) 
        {
            return;
        }
        SurvivalGauge.Instance.AttackSpeed -= JsonParsingManager.instance.boxItemDic[1006].value;
    }
    private void ApplyJumpPower(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1007].imageFileName);
        PlayerMoveAbility.Instance.JumpPower += amount;
    }
    private void ApplyHunger(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1008].imageFileName);
        SurvivalGauge.Instance.Maxhunger += amount;
    }
    private void ApplyDefense(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1009].imageFileName);
        SurvivalGauge.Instance.Defense += amount;
    }
    private void ApplyLifesteal(int amount)
    {
        Debug.Log(JsonParsingManager.instance.boxItemDic[1010].imageFileName);
        if (Monster.Health <= Monster.MaxHealth)
        {
            SurvivalGauge.Instance.PlayerHealth += amount;
            Monster.Health -= amount;
        }

    }
}
