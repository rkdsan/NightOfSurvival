using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItemManager : MonoBehaviour
{
    public static OnGroundItemManager instance;

    public OnGroundItem allOnGroundItems;

    private Dictionary<string, OnGroundItem> itemDictionary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    public OnGroundItem GetItemPrefab(string itemName)
    {
        return itemDictionary[itemName];
    }

}
