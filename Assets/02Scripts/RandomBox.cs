using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BoxItemType
{
    maxhp,
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

    public int id;


    void Update()
    {
        if (isPlayerNear && Input.GetKey(KeyCode.E) && !isOpened)
        {
            if (!isOpenChestPlayed) // 열었고
            {
                animation.Play("OpenChest");
                isOpened = true;
            
                MakePercent(transform.position);
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

        switch (itemType)
        {
            case BoxItemType.maxhp:
                itemToCreate = Avocado;
                id = 1001;
                break;

            case BoxItemType.stamina:
                itemToCreate = Bread;
                id = 1002;
                break;

            case BoxItemType.power:
                itemToCreate = Carrot;
                id = 1003;
                break;

            case BoxItemType.hp:
                itemToCreate = Broccoli;
                id = 1004;
                break;

            case BoxItemType.speed:
                itemToCreate = Banana;
                id = 1005;
                break;

            case BoxItemType.attackSpeed:
                itemToCreate = Garlic;
                id = 1006;
                break;

            case BoxItemType.jumpPower:
                itemToCreate = Fish;
                id = 1007;
                break;

            case BoxItemType.hunger:
                itemToCreate = Pumpkin;
                id = 1008;
                break;

            case BoxItemType.defense:
                itemToCreate = Pepper;
                id = 1009;
                break;

            case BoxItemType.Lifesteal:
                itemToCreate = Pear;
                Debug.Log(itemToCreate.name);
                id = 1010;
                break;
               
        }
        UI_BoxItem.instance.Refresh_TextUI(id);
        if (itemToCreate != null)
        {
            Instantiate(itemToCreate, ItemPos.position, Quaternion.identity);
            Debug.Log(itemType.ToString() + "를 얻었습니다.");
            UI_BoxItem.ShowBoxItem(itemType);
        }
    }
}
