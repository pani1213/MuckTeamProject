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

    private ItemSlot _currentSeletItemSlot;
    private ItemSlot _dropItemSlot;
    private int halfCount = 0;

 
    public void InIt()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        UnityEngine.Cursor.visible = gameObject.activeSelf;
        if (UnityEngine.Cursor.visible) UnityEngine.Cursor.lockState = CursorLockMode.None;
        else UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].Refresh_SlotUI();
        mPointerEventData = new PointerEventData(null);
        
    }
    //event_trigger
    public void BeginDragAction()
    {
        
        mPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        mRayCaster.Raycast(mPointerEventData, results);

        if (results.Count <= 0)
            return;

        Debug.Log(results[0].gameObject.name);
        if (results[0].gameObject.TryGetComponent<ItemSlot>(out _currentSeletItemSlot))
        {
            _currentSeletItemSlot.Empty_UI();
            DragSlot.dragInven = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex];
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
        if (results[0].gameObject.TryGetComponent<ItemSlot>(out _dropItemSlot))
        {
            if (DragSlot.dragInven != null)
            {
                ItemInfoManager.instance.InvenSwap(_currentSeletItemSlot.slotIndex, _dropItemSlot.slotIndex);
                if (_currentSeletItemSlot != null)
                    _currentSeletItemSlot.Refresh_SlotUI();
                _dropItemSlot.Refresh_SlotUI();
            }
        }
        EndDrag();
    }
    private void EndDrag()
    {
        _currentSeletItemSlot.Refresh_SlotUI();
        _currentSeletItemSlot = null;
        _dropItemSlot = null;
        DragSlot.dragInven = null;
        DragSlot.InIt_DragSlot();
    }
}
