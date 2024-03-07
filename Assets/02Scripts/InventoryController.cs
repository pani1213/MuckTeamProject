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
    private void Start()
    {
        InIt();
        GameManager.instance.action = InIt;
        mPointerEventData = new PointerEventData(null);
    }
    public void InIt()
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            itemSlots[i].Refresh_SlotUI();
        }
    }
    //event_trigger
    public void BeginDragAction()
    {
        mPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        mRayCaster.Raycast(mPointerEventData, results);
        // 오른쪽클릭 드래그 (전체 선택)
        if (Input.GetMouseButton(0))
        {
            if (results.Count > 0)
            {
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
            else
                Debug.Log("드래그 대상 없음");
        }
        // 왼쪽클릭 드래그 (절반선택)
        if (Input.GetMouseButton(1))
        {

        }
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

        if (results.Count > 0)
        {
            if (results[0].gameObject.TryGetComponent<ItemSlot>(out _dropItemSlot))
            {
                if (DragSlot.dragInven != null)
                {
                    _dropItemSlot.Refresh_SlotUI();
                }


            }
        }
        EndDrag();
    }

    private void EndDrag()
    {
        DragSlot.dragInven = null;
        DragSlot.dragInven = null;
        DragSlot.InIt_DragSlot();
        _currentSeletItemSlot.Refresh_SlotUI();
    }
}
