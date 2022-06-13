using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItemManager : MonoBehaviour
{
    public static OnGroundItemManager instance;

    public List<OnGroundItem> allOnGroundItems; //생성돼있는
    public List<OnGroundItem> allOnGroundItemPrefabs;

    private Dictionary<string, OnGroundItem> itemDictionary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        allOnGroundItems.Capacity = 32;
        SetItemDictionary();
    }


    public OnGroundItem GetItemPrefab(string itemName)
    {
        if (itemDictionary.ContainsKey(itemName))
        {
            return itemDictionary[itemName];
        }

        Debug.LogWarning(itemName + "이(가) On Ground Item Dictionary에 등록돼있지 않습니다.");
        return null;
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
