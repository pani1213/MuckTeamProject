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

    private bool isRightClick = false;
    private void Start()
    {
        InIt();
        GameManager.instance.action = InIt;
        mPointerEventData = new PointerEventData(null);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            InIt();
    }
    public void InIt()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].Refresh_SlotUI();
        
    }
    //event_trigger
    public void BeginDragAction()
    {
        mPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        mRayCaster.Raycast(mPointerEventData, results);

        if (results.Count <= 0)
            return;
        // ������Ŭ�� �巡�� (��ü ����)
        //if (Input.GetMouseButton(0))
        //{
            Debug.Log(results[0].gameObject.name);
            if (results[0].gameObject.TryGetComponent<ItemSlot>(out _currentSeletItemSlot))
            {
                _currentSeletItemSlot.Empty_UI();
                DragSlot.dragInven = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex];
                DragSlot.InIt_DragSlot();
            }
            else
                Debug.Log("result���� itemSlot�� ã���� ����");
        //}
        // ����Ŭ�� �巡�� (���ݼ���)
        // if (Input.GetMouseButton(1))
        // {
        //     isRightClick = true;
        //     Debug.Log(results[0].gameObject.name);
        //     if (results[0].gameObject.TryGetComponent<ItemSlot>(out _currentSeletItemSlot))
        //     {
        //         int count = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].count;
        //         DragSlot.dragInven = new InvenItem()
        //         { item = ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].item, count = count.GetHalf().A };
        //         halfCount = count.GetHalf().B;
        //         ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].count = halfCount;
        //         Debug.Log(ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].count);
        //
        //         DragSlot.InIt_DragSlot();
        //         _currentSeletItemSlot.Refresh_SlotUI();
        //     }
        //     else
        //         Debug.Log("result���� itemSlot�� ã���� ����");
        // }
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
            Debug.Log("���� ��ü ����");
            EndDrag();
            return;
        }
        //if (Input.GetMouseButtonUp(0))
        //{
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
        //}
        //if (Input.GetMouseButtonUp(1))
        //{
        //    Debug.Log(1);
        //    if (isRightClick)
        //    {
        //        if (_dropItemSlot == null)
        //        {
        //            ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].count += DragSlot.dragInven.count;
        //            Debug.Log(ItemInfoManager.instance.itemInventory[_currentSeletItemSlot.slotIndex].count);
        //            _currentSeletItemSlot.Refresh_SlotUI();
        //        }
        //        else
        //        {
        //            ItemInfoManager.instance.itemInventory[_dropItemSlot.slotIndex].count += DragSlot.dragInven.count;
        //            Debug.Log(ItemInfoManager.instance.itemInventory[_dropItemSlot.slotIndex].count);
        //            _dropItemSlot.Refresh_SlotUI();
        //        }
        //    }
        //}
        EndDrag();
    }
    private void EndDrag()
    {
        _currentSeletItemSlot = null;
        _dropItemSlot = null;

        isRightClick = false;
        DragSlot.dragInven = null;
        DragSlot.InIt_DragSlot();
    }
}
