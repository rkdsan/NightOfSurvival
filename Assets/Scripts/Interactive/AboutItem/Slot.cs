using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image image;
    public Text countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int itemCount;

    private void Awake()
    {
        DeleteItem();
    }

    public void NewItem(Item _item)
    {
        item = _item;
        image.sprite = _item.itemImage;
        image.color = Color.white;
        itemCount = 1;

        Transform ItemObj = item.gameObject.transform;
        ItemObj.transform.parent = InventoryManager.instance.playerController.hand.transform;
        ItemObj.localPosition = Vector3.zero;
        ItemObj.localRotation = Quaternion.Euler(item.originRotate);
        ItemObj.tag = "Item";

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
        }
        else
        {
            countText.text = "x" + itemCount;
        }
    }

    public void DeleteItem()
    {
        image.color = Color.clear;
        image.sprite = null;
        itemCount = 0;
        SetCountText();
    }

}
