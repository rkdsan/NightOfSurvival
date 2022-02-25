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
    public SlotData[] slotdata = new SlotData[20];

    public InventoryData()
    {
        typeName = "InventoryData";
    }
}

[Serializable]
public class SlotData
{

}