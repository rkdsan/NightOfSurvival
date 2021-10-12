using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public PlayerController playerController;
    public GameObject InventoryWindow;
    public GameObject ItemFrame;
    public Slot[] slots;

    private GameObject nowItemObject;
    private int nowItemIndex;
    private int itemCount;

    void Awake()
    {
        instance = this;
        nowItemIndex = 0;
        itemCount = 0;
    }

    void Update()
    {
        ChangeItem();
    }

    public void ChangeItem()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if(wheelInput > 0)
        {
            UpItemIndex();
        }
        else if(wheelInput < 0)
        {
            DownItemIndex();
        }
    }

    private void UpItemIndex()
    {
        if(nowItemIndex < itemCount)
        {
            nowItemIndex++;
            SetNowItem();
        }

    }
    private void DownItemIndex()
    {
        if(1 < nowItemIndex)
        {
            nowItemIndex--;
            SetNowItem();
        }
    }

    public void AddItem(Item _item)
    {
        if (itemCount >= slots.Length)
        {
            Debug.Log("ÀÎº¥Åä¸®°¡ ²ËÃ¡½À´Ï´Ù.");
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemCount == 0)
            {
                slot.NewItem(_item);
                itemCount++;
                CheckItemIndex();
                break;
            }
            else if (slot.item.objectName.Equals(_item.objectName))
            {
                slot.UpCount();
                Destroy(_item.gameObject);
                break;
            }
        }
    }

    public void CheckItemIndex()
    {
        if(nowItemIndex == 0 && itemCount > 0)
        {
            nowItemIndex = 1;
            ItemFrame.SetActive(true);
            SetNowItem();
        }
    }

    public void SetNowItem()
    {
        if (nowItemObject != null)
        {
            nowItemObject.SetActive(false);
        }
        nowItemObject = slots[nowItemIndex - 1].item.gameObject;
        nowItemObject.SetActive(true);
        ItemFrame.transform.position = slots[nowItemIndex - 1].transform.position;

    }


    //private void CheckTabKey()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        if (InventoryWindow.activeSelf)
    //        {
    //            OffInventory();
    //        }
    //        else
    //        {
    //            OnInventory();
    //        }
    //    }
    //}

    //private void OffInventory()
    //{
    //    InventoryWindow.SetActive(false);
    //    Cursor.lockState = CursorLockMode.Locked;
    //    playerController.canMove = true;
    //}
    //private void OnInventory()
    //{
    //    InventoryWindow.SetActive(true);
    //    Cursor.lockState = CursorLockMode.None;
    //    playerController.canMove = false;
    //}

}
