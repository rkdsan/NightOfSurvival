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

    [HideInInspector] public int itemKindCount;

    private GameObject nowItemObject;
    private Slot clickedSlot;

    private int nowItemIndex;

    void Awake()
    {
        nowItemIndex = 0;
        itemKindCount = 0;
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
        if(nowItemIndex < itemKindCount)
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
        if (itemKindCount >= slots.Length)
        {
            Debug.Log("ÀÎº¥Åä¸®°¡ ²ËÃ¡½À´Ï´Ù.");
            return;
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemCount == 0)
            {
                slot.NewItem(_item);
                itemKindCount++;
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
        if(nowItemIndex == 0 && itemKindCount > 0)
        {
            nowItemIndex = 1;
            ItemFrame.SetActive(true);
            SetNowItem();
        }
    }

    public void SortInventory()
    {
        for(int i = 0; i < itemKindCount; i++)
        {
            if(slots[i].itemCount == 0)
            {
                for (int j = i; j < itemKindCount - 1; j++)
                {
                    slots[j].NewItem(slots[j + 1].item);
                }
                break;
            }
        }

        //for(int i = itemCount; i < slots.Length; i++)
        //{
        //    slots[i].ClearSlot();
        //}

        SetNowItem();
    }


    public void SetNowItem()
    {
        if (itemKindCount == 0)
        {
            nowItemObject.SetActive(false);
            return;
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
