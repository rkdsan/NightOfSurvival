using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    public Image image;
    public Text countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int itemCount;

    private void Awake()
    {
        //DeleteItem();
    }

    public void NewItem(Item _item)
    {
        item = _item;
        image.sprite = _item.itemImage;
        image.color = Color.white;
        itemCount = 1;
        item.gameObject.layer = 1 << 1;

        Transform ItemObj = item.gameObject.transform;
        ItemObj.transform.parent = GameManager.instance.playerController.hand.transform;
        ItemObj.localPosition = Vector3.zero;
        ItemObj.localRotation = Quaternion.Euler(item.originRotate);
        
        item.gameObject.SetActive(false);

        SetCountText();
    }

    public void UpCount()
    {
        itemCount++;
        SetCountText();
    }

    public void SetCountText()
    {
        if (itemCount == 0)
        {
            countText.text = "";
            DeleteItem();
        }
        else
        {
            countText.text = "x" + itemCount;
        }
        
    }

    public void DownCount()
    {
        itemCount--;
        SetCountText();
    }

    public IEnumerator DownCountQueue()
    {
        yield return null;
        itemCount--;
        SetCountText();
    }

    public void DeleteItem()
    {
        Destroy(item.gameObject);
        ClearSlot();
        inventoryManager.itemKindCount--;
        //inventoryManager.SortInventory();
    }

    public void ClearSlot()
    {
        item = null;
        image.color = Color.clear;
        image.sprite = null;
        itemCount = 0; 
        countText.text = null;
    }

    public void UseSlotItem()
    {
        if (item == null) return;
        item.UseItem();
        DownCount();
        inventoryManager.SortInventory();
        inventoryManager.SetNowItem();
    }

}
