using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public PlayerController playerController;
    public CraftManager craftManager;
    public GameObject CraftWindow;
    public GameObject ItemFrame;
    public Slot[] slots;

    [HideInInspector] public int itemCount;

    private GameObject nowItemObject;
    private Slot clickedSlot;

    private int nowItemIndex;

    void Awake()
    {
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
            return;
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

    public void SortInventory()
    {
        for(int i = 0; i < itemCount; i++)
        {
            if(slots[i].itemCount == 0 && itemCount < slots.Length)
            {
                slots[i].NewItem(slots[i + 1].item);
            }
        }

        if (itemCount < slots.Length) slots[itemCount].ClearSlot();
        SetNowItem();
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

    public void ClickItem()
    {
        clickedSlot = EventSystem.current.currentSelectedGameObject.GetComponent<Slot>();

        if (clickedSlot.itemCount > 0 && clickedSlot.item.canCraft)
        {
            craftManager.PutOnCrafter(clickedSlot);
        }

    }
    

}
