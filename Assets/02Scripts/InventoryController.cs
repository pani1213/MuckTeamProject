using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public GraphicRaycaster mRayCaster;
    public PointerEventData mPointerEventData;
    public DragSlot DragSlot;
    private Slot _currentSeletItemSlot;
    private Slot _dropItemSlot;

    public void InIt()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        UnityEngine.Cursor.visible = gameObject.activeSelf;
        if (UnityEngine.Cursor.visible) UnityEngine.Cursor.lockState = CursorLockMode.None;
        else UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        RefreshAllSlot();
        mPointerEventData = new PointerEventData(null);
    }
    public void RefreshAllSlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].Refresh_SlotUI();
    }
    public void InIt(bool _isOnUI)
    {
        gameObject.SetActive(_isOnUI);
        UnityEngine.Cursor.visible = _isOnUI;
        if (UnityEngine.Cursor.visible) UnityEngine.Cursor.lockState = CursorLockMode.None;
        else UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].Refresh_SlotUI();
        mPointerEventData = new PointerEventData(null);
    }
    //event_trigger
    public void ButtonAction_MakeWorkBench()
    {
        Debug.Log("buttonAction");
        if (ItemInfoManager.instance.TryRemoveItem(1001, 10))
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[1020], 1);
        RefreshAllSlot();

    }
    public void BeginDragAction()
    {   
        mPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        mRayCaster.Raycast(mPointerEventData, results);

        if (results.Count <= 0)
            return;
      //  Debug.Log($" start : {results[0].gameObject.name}");
        if (results[0].gameObject.TryGetComponent<Slot>(out _currentSeletItemSlot))
        {
            _currentSeletItemSlot.Empty_UI();
            if (_currentSeletItemSlot.slotType_UI == SlotType.Item)
            {
                _currentSeletItemSlot.InvenItem = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex];
                DragSlot.dragInven = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex];
            }
            else if (_currentSeletItemSlot.slotType_UI == SlotType.Box)
            {
                _currentSeletItemSlot.InvenItem = ItemInfoManager.instance.boxDictionary[_currentSeletItemSlot.id][_currentSeletItemSlot.slotIndex];
                DragSlot.dragInven = ItemInfoManager.instance.boxDictionary[_currentSeletItemSlot.id][_currentSeletItemSlot.slotIndex];
            }
            else if (_currentSeletItemSlot.slotType_UI == SlotType.Brazier)
            {
                _currentSeletItemSlot.InvenItem = BrazierManager.instance.GetBrazierDic()[_currentSeletItemSlot.slotIndex];
                DragSlot.dragInven = BrazierManager.instance.GetBrazierDic()[_currentSeletItemSlot.slotIndex];
            }
            DragSlot.InIt_DragSlot();
        }
        else
            Debug.Log("result에서 itemSlot을 찾을수 없음");
        
    }

    //event_trigger
    public void DragAction()
    {
        DragSlot.gameObject.SetActive(true);
        DragSlot.transform.position = Input.mousePosition;
    }
    //event_trigger
    public void EndDragAction()
    {
        DragSlot.gameObject.SetActive(false);
        mPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        mRayCaster.Raycast(mPointerEventData, results);
        if (results.Count <= 0)
        {
            Debug.Log("닿은 객체 없음");
            EndDrag();
            return;
        }
        if (results[0].gameObject.TryGetComponent<Slot>(out _dropItemSlot))
        {
           // Debug.Log($" end : {results[0].gameObject.name}");
      
            
            if (DragSlot.dragInven != null)
            {
                if (_dropItemSlot.slotType_UI == SlotType.Item)
                    _dropItemSlot.InvenItem = ItemInfoManager.instance.itemInventory[_dropItemSlot.slotIndex];
                else if (_dropItemSlot.slotType_UI == SlotType.Box)
                    _dropItemSlot.InvenItem = ItemInfoManager.instance.boxDictionary[_dropItemSlot.id][_dropItemSlot.slotIndex];
                else if (_dropItemSlot.slotType_UI == SlotType.Brazier)
                    _dropItemSlot.InvenItem = BrazierManager.instance.GetBrazierDic()[_dropItemSlot.slotIndex];


                if (_currentSeletItemSlot.InvenItem.item != null && _dropItemSlot.InvenItem.item != null &&
                    _currentSeletItemSlot.InvenItem.item.id == _dropItemSlot.InvenItem.item.id)
                {
                    Debug.Log("합침");
                    ItemInfoManager.instance.ItemMerge(_currentSeletItemSlot.InvenItem, _dropItemSlot.InvenItem);
                }
                else
                { 
                    Debug.Log("바꿈");
                    ItemInfoManager.instance.InvenSwap(_currentSeletItemSlot.InvenItem, _dropItemSlot.InvenItem);
                }
                if (_currentSeletItemSlot != null)
                    _currentSeletItemSlot.Refresh_SlotUI();
                _dropItemSlot.Refresh_SlotUI();
            }
        }
        EndDrag();
    }
    private void EndDrag()
    {

        if (_currentSeletItemSlot != null)
            _currentSeletItemSlot.Refresh_SlotUI();
        _currentSeletItemSlot = null;
        _dropItemSlot = null;
        DragSlot.dragInven = null;
        DragSlot.InIt_DragSlot();
    }
}
