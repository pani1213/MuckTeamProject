using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
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

    public Animation animations;
    public Animator _animator;
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
    public bool isPlayerNear = false;
    private bool isOpened = false;

    Monster Monster;

    public int id;
    public string type;
    public float value;

    public BoxCollider myCollider;
    public Rigidbody myRigidbody;
    public void InIt(int _boxItemId, string _type, int _value)
    {
        id = _boxItemId;
        type = _type;
        value = _value;
    }
    public void SetKinematic(bool _bool)
    {
        myRigidbody.isKinematic = _bool;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("������ ����");
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[1022]);
        }

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {


            if (25 > ItemInfoManager.instance.GetCoinCount())
            {
                Debug.Log("������");
                return;
            }
            else
                ItemInfoManager.instance.SetCoin(-25);

            StartCoroutine(PlayAnimationsInOrder());

             if (!isOpenChestPlayed) // ������
             {
                 Debug.Log(0);
                 //animations.Play("OpenChest");
                 isOpened = true;
                SoundManager.instance.PlayAudio("BoxOpen");

                MakePercent(transform.position);
                 ApplyEffect((BoxItemType)id, (int)JsonParsingManager.instance.boxItemDic[id].value);
                 isOpenChestPlayed = true;
             }

             else if (isOpenChestPlayed && !animations.isPlaying && !isItemCreated) // ���� �����
             {
                 if (!animations.IsPlaying("OpenChest") && !isItemCreated)
                 {
                     isItemCreated = true;
                 }
             }

            RandomBoxRespawn randomBoxRespawn = GetComponentInParent<RandomBoxRespawn>();
            if (randomBoxRespawn != null)
            {
                randomBoxRespawn.OnBoxDead(); // ��� �˸�
            }
        }
    }

    IEnumerator PlayAnimationsInOrder()
    {
        // "OpenChest" �ִϸ��̼� ���
        animations.Play("OpenChest");
        // ù ��° �ִϸ��̼��� ���̸�ŭ ���
        yield return new WaitForSeconds(animations["OpenChest"].length);

        // "SpinObj" �ִϸ��̼� ���
        animations.Play("SpinObj");
        // �ʿ��� ��� �� ��° �ִϸ��̼��� ���̸�ŭ �߰��� ����� �� �ֽ��ϴ�.
         yield return new WaitForSeconds(animations["SpinObj"].length);

    }

    public void Reinitialize()
    {
        // ������ ����
        foreach (Transform child in ItemPos)
        {
            Destroy(child.gameObject);
        }

        // �ִϸ��̼� �ʱ�ȭ
        AnimationState state = animations["OpenChest"];
        if (state != null)
        {
            state.time = 0; // ù ���������� �ð��� ����
            animations.Play("OpenChest"); // �ִϸ��̼��� ��� - �� �� ù �����ӿ��� ����
            animations.Sample(); // ���� �ִϸ��̼� ���¸� ����
            animations.Stop(); // �ִϸ��̼��� ��� �������� ù �����ӿ��� ����
        }

        // �ʱ�ȭ �� ���� ����
        isOpenChestPlayed = false;
        isItemCreated = false;
        isPlayerNear = false;
        isOpened = false;
        SetKinematic(true);
        gameObject.SetActive(true); // RandomBox Ȱ��ȭ

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

    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        // Debug.Log("Random Percentage: " + percentage);
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
        //Debug.Log("Creating item of type: " + itemType.ToString());
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
            GameObject createdItem = Instantiate(itemToCreate, ItemPos.position, Quaternion.identity, ItemPos);
            //Instantiate(itemToCreate, ItemPos.position, Quaternion.identity);
            //Debug.Log(itemType.ToString() + "�� ������ϴ�.");
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
        //if (Monster.Health <= Monster.MaxHealth)
        //{
        //    SurvivalGauge.Instance.PlayerHealth += amount;
        //    Monster.Health -= amount;
        //}

    }
}
