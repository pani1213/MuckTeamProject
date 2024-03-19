using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BrazierObject : MonoBehaviour
{
    public int id;
    //public Dictionary<int,InvenItem> brazier = new Dictionary<int,InvenItem>();
    public Action brazierAction;
    public int fireTime = 0;
    private int maxFireTime = 0 , maxCookingTime = 0;
    private int cookingTime = 0;
    private bool isRosting = false;
    private float timer = 0;
    //상자를 설치했을때
    public void InIt()
    {
        if (id == 0)
        {
            id = BrazierManager.instance.brazierDictionary.Count + 1000;
            Debug.Log($"barzierID : {id}");
            BrazierManager.instance.brazierDictionary.Add(id, new List<InvenItem>(3));
            for (int i = 0; i < 3; i++)
                BrazierManager.instance.brazierDictionary[id].Add(new InvenItem());
  
        }
    }
    public void OpenBrazierUI()
    {
        BrazierManager.instance.currentBrazierId = id;
        BrazierManager.instance.controller.InIt(id, GetNomalize(fireTime, 0, maxFireTime),GetNomalize((maxCookingTime - cookingTime), 0, maxCookingTime));
        ItemInfoManager.instance.inventoryController.InIt(true);

        if (brazierAction == null)
        {
            brazierAction = BrazierAction;
            BrazierManager.instance.BrazierAction += BrazierAction;
        }
    }
    public void BrazierAction()
    {
        timer += Time.deltaTime;

        if (timer > 1 && fireTime > 0 && isRosting)
        {
            RostingItem();
            fireTime--;
            timer = 0;
        }
        //연료 넣기
        SetFire();
        SetRostItem();

      
    }
    private float GetNomalize(int vlaue, int min, int max)
    {
        float a = (float)(vlaue - min) / (max - min);
        return a;
    }
    private void SetFireFillAmount()
    {
        //Debug.Log(BrazierManager.instance.currentBrazierId);
        if (BrazierManager.instance.currentBrazierId == id)
            BrazierManager.instance.controller.fill_ImageUI.fillAmount = GetNomalize(fireTime, 0, maxFireTime);
    }
    private void SetCreateItemFillAmount()
    {
        if (BrazierManager.instance.currentBrazierId == id)
            BrazierManager.instance.controller.resourceImageUI.fillAmount = GetNomalize((maxCookingTime - cookingTime), 0, maxCookingTime);
    }
    private void SetFire()
    {
        if (BrazierManager.instance.GetBrazierDic(id)[0].item == null)
            return;

        if (BrazierManager.instance.GetBrazierDic(id)[0].item.fireTime > 0 && fireTime <= 0)
        {
            fireTime += BrazierManager.instance.GetBrazierDic(id)[0].item.fireTime;
            maxFireTime = BrazierManager.instance.GetBrazierDic(id)[0].item.fireTime;
            SetFireFillAmount();

            BrazierManager.instance.GetBrazierDic(id)[0].count--;
            if (BrazierManager.instance.GetBrazierDic(id)[0].count <= 0)
                BrazierManager.instance.GetBrazierDic(id)[0].item = null;

            BrazierManager.instance.controller.fuelSlot.Refresh_SlotUI();
        }
    }
    private void SetRostItem()
    {
        if (BrazierManager.instance.GetBrazierDic(id)[1].item == null)
            return;
        if (BrazierManager.instance.GetBrazierDic(id)[1].item.cookingTime > 0 && cookingTime <= 0)
        {
            cookingTime = BrazierManager.instance.GetBrazierDic(id)[1].item.cookingTime;
            maxCookingTime = BrazierManager.instance.GetBrazierDic(id)[1].item.cookingTime;
            SetCreateItemFillAmount();
            isRosting = true;
        }
    }
    public void RostingItem()
    {
        cookingTime--;
        SetCreateItemFillAmount();
        SetFireFillAmount();

        //확인 하는 함수


        // 완성됐을때
        if (cookingTime <= 0)
        {
            if (BrazierManager.instance.GetBrazierDic(id)[2].item == null)
            {
                Debug.Log($"id: {id},  {BrazierManager.instance.GetBrazierDic(id)[2] == null}");
                BrazierManager.instance.GetBrazierDic(id)[2] = new InvenItem()
                {
                    item = JsonParsingManager.instance.ItemDic[BrazierManager.instance.GetBrazierDic(id)[1].item.id + 1],
                    count = JsonParsingManager.instance.ItemDic[BrazierManager.instance.GetBrazierDic(id)[1].item.id].makeCount
                };
            }
            else
            { 
                Debug.Log(id);
                BrazierManager.instance.GetBrazierDic(id)[2].count += JsonParsingManager.instance.ItemDic[BrazierManager.instance.GetBrazierDic(id)[1].item.id].makeCount;
            }

            BrazierManager.instance.GetBrazierDic(id)[1].count--;
            if (BrazierManager.instance.GetBrazierDic(id)[1].count <= 0)
                BrazierManager.instance.GetBrazierDic(id)[1].item = null;


            BrazierManager.instance.controller.resourceSlot.Refresh_SlotUI();
            BrazierManager.instance.controller.finishedSlot.Refresh_SlotUI();
      
            isRosting = false;
        }

    }
    public void DestroyObject()
    {
        BrazierManager.instance.BrazierAction -= BrazierAction;
        Destroy(gameObject);
    }
}
