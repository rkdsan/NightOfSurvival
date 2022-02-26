using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonData
{
    public string typeName;

}

[Serializable]
public class PlayerData : JsonData
{
    public Vector3 pos;

    public PlayerData()
    {
        typeName = "PlayerData";
    }
}

[Serializable]
public class InventoryData : JsonData
{
    public SlotData[] slotdata;
    public int itemKindCount;

    public InventoryData()
    {
        typeName = "InventoryData";

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

    public SlotData()
    {
        typeName = "SlotData";
    }
}