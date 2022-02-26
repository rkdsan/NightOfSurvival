using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHandItemManager : MonoBehaviour
{
    public static InHandItemManager instance;
    public InHandItem[] allInHandItems;

    private Dictionary<string, InHandItem> itemDictionary;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("인스턴스 중복");
            Destroy(gameObject);
        }
        instance = this;
        MakeItems();    
    }

    private void Start()
    {
        SetItemDictionary();
    }

    private void MakeItems()
    {
        for(int i = 0; i < allInHandItems.Length; i++)
        {
            allInHandItems[i] = Instantiate(allInHandItems[i]).GetComponent<InHandItem>();
            allInHandItems[i].gameObject.SetActive(false);
        }
    }


    private void SetItemDictionary()
    {
        itemDictionary = new Dictionary<string, InHandItem>();

        foreach(var item in allInHandItems)
        {
            itemDictionary.Add(item.itemName, item);
            item.gameObject.SetActive(false);
        }
    }

    public InHandItem GetItemPrefab(string itemName)
    {
        return itemDictionary[itemName];
    }


}
