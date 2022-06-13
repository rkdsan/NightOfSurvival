using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonData
{
    public string typeName;

    public JsonData()
    {
        typeName = GetType().Name;
    }

}

[Serializable]
public class PlayerData : JsonData
{
    public Vector3 pos;

    public PlayerData() : base()
    {
        
    }
}

[Serializable]
public class InventoryData : JsonData
{
    public SlotData[] slotdata;
    public int itemKindCount;

    public InventoryData() : base()
    {
        int length = GameManager.instance.inventoryManager.slots.Length;
        slotdata = new SlotData[length];

        for(int i = 0; i < length; i++)
        {
            slotdata[i] = new SlotData();
        }
    }
}

[Serializable]
public class SlotData : JsonData
{
    public string itemName;
    public int itemCount;

    public SlotData() : base()
    {

    }
}

[Serializable]
public class OnGroundItemManagerData : JsonData
{
    public OnGroundItemData[] groundItemData;

    public OnGroundItemManagerData() : base()
    {
        int length = OnGroundItemManager.instance.allOnGroundItems.Count;
        groundItemData = new OnGroundItemData[length];
        for(int i = 0; i < length; i++)
        {
            groundItemData[i] = new OnGroundItemData();
        }

    }
}

[Serializable]
public class OnGroundItemData : JsonData
{
    public string itemName;
    public Vector3 position;

    public OnGroundItemData() : base()
    {

    }
}

[Serializable]
public class LampData : JsonData
{
    public int[] id;
    public bool[] isOn;

    public LampData() : base()
    {
        int length = LampManager.instance.allLampDictionary.Count;
        id = new int[length];
        isOn = new bool[length];
    }


}