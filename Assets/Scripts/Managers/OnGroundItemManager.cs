using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItemManager : MonoBehaviour
{
    public static OnGroundItemManager instance;

    public List<OnGroundItem> allOnGroundItems; //»ý¼ºµÅÀÖ´Â
    public List<OnGroundItem> allOnGroundItemPrefabs;

    private Dictionary<string, OnGroundItem> itemDictionary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        SetItemDictionary();
    }


    public OnGroundItem GetItemPrefab(string itemName)
    {
        return itemDictionary[itemName];
    }

    private void SetItemDictionary()
    {
        itemDictionary = new Dictionary<string, OnGroundItem>();

        foreach(var item in allOnGroundItemPrefabs)
        {
            itemDictionary.Add(item.objectName, item);
        }
    }

    public void DeleteAllItem()
    {
        foreach (var item in allOnGroundItems)
        {
            Destroy(item.gameObject);
        }
        
        allOnGroundItems.Clear();
    }

}
