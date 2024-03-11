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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (!isOpenChestPlayed) // ������
            {
                animation.Play("OpenChest");
                MakePercent(transform.position);
                isOpenChestPlayed = true;
            }

            else if (isOpenChestPlayed && !animation.isPlaying && !isItemCreated) // ���� �����
            {
                if (!animation.IsPlaying("OpenChest"))
                {
                    StartCoroutine(Rotate_Coroutine());
                    // ���� ����Ǵ� UI �ԷµǸ� destroy �ǵ��� �߰� ����
                    isItemCreated = true;
                }
            }
                    
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
            Make(BoxItemType.maxhp, ItemPos.position);
        }
        else if (percentage <= 20)
        {
            Make(BoxItemType.stamina, ItemPos.position);
        }
        else if (percentage <= 30)
        {
            Make(BoxItemType.power, ItemPos.position);
        }
        else if (percentage <= 40)
        {
            Make(BoxItemType.hp, ItemPos.position);
        }
        else if (percentage <= 50)
        {
            Make(BoxItemType.speed, ItemPos.position);
        }
        else if (percentage <= 60)
        {
            Make(BoxItemType.attackSpeed, ItemPos.position);
        }
        else if (percentage <= 70)
        {
            Make(BoxItemType.jumpPower, ItemPos.position);
        }
        else if (percentage <= 80)
        {
            Make(BoxItemType.hunger, ItemPos.position);
        }
        else if (percentage <= 90)
        {
            Make(BoxItemType.defense, ItemPos.position);
        }
        else if (percentage <= 100)
        {
            Make(BoxItemType.Lifesteal, ItemPos.position);
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

        if (itemToCreate != null)
        {
            Instantiate(itemToCreate, ItemPos.position, Quaternion.identity);
            Debug.Log(itemType.ToString() + "�� ������ϴ�.");
        }
    }
}