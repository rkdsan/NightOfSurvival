using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public PlayerController playerController;
    public CraftManager craftManager;
    public ItemNamePrinter nowItemName;
    public GameObject CraftWindow;
    public GameObject ItemFrame;
    public AudioClip pickUpSound;
    public Slot[] slots;

    [HideInInspector] public int itemKindCount;

    [HideInInspector] public GameObject nowItemObject;
    [HideInInspector] public Slot nowSlot;

    private Slot clickedSlot;

    private int nowItemIndex;

    void Awake()
    {
        nowItemIndex = 0;
        itemKindCount = 0;
        playerController.inventoryManager = this;
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

    public void AddItem(InHandItem _item)
    {
        if (itemKindCount >= slots.Length)
        {
            Debug.Log("인벤토리가 꽉찼습니다.");
            return;
        }

        foreach (Slot slot in slots)
        {
            //빈칸이면
            if (slot.itemCount == 0)
            {
                slot.NewItem(_item);
                itemKindCount++;
                CheckItemIndex();
                break;
            }
            //같은템이면
            else if (slot.item.itemName.Equals(_item.itemName))
            {
                slot.UpCount();
                Destroy(_item.gameObject);
                break;
            }
        }
        SFXPlayer.instance.Play(pickUpSound);
    }

    public void CheckItemIndex()
    {
        if (nowItemIndex == 0 && itemKindCount > 0)
        {
            nowItemIndex = 1;
            ItemFrame.SetActive(true);
            SetNowItem();
        }
    }

    public void TrySortInventory()
    {
        int lastIndex;
        while (true)
        {
            lastIndex = GetLastIndex();
            if (lastIndex + 1 <= itemKindCount) break;

            for (int i = 0; i < lastIndex; i++)
            {
                PullItem(i);
            }

            slots[lastIndex].ClearSlot();
        }
    }

    private void PullItem(int i)
    {
        //빈슬롯이면 당겨옴
        if (slots[i].itemCount == 0)
        {
            if (slots[i + 1].itemCount == 0)
                slots[i].ClearSlot();
            else
            {
                slots[i].NewItem(slots[i + 1].item);
                slots[i].itemCount = slots[i + 1].itemCount;
                slots[i + 1].ClearSlot();
            }
        }
    }

    private int GetLastIndex()
    {
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].itemCount > 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetNowItem()
    {

        if (nowItemObject != null)
        {
            nowItemObject.SetActive(false);
        }

        if (itemKindCount == 0) //아이템이 없으면
        {
            nowItemIndex = 0;
            ItemFrame.SetActive(false);
            return;
        }
        if (nowItemIndex > itemKindCount) nowItemIndex = itemKindCount;

        nowSlot = slots[nowItemIndex - 1];
        nowItemObject = nowSlot.item.gameObject;
        nowItemObject.SetActive(true);
        ItemFrame.transform.position = nowSlot.transform.position;

        //현재 아이템 표시
        nowItemName.OnText(nowSlot.item.itemName);
    }

    public void ClickItem()
    {
        clickedSlot = EventSystem.current.currentSelectedGameObject.GetComponent<Slot>();

        if (clickedSlot.itemCount > 0 && clickedSlot.item.canCraft)
        {
            craftManager.PutOnCrafter(clickedSlot);
        }

    }
    
    public void UseNowItem()
    {
        if (nowItemObject == null) return;
        nowSlot.UseSlotItem();
    }


}
