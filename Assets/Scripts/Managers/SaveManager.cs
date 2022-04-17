using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [HideInInspector] public bool isLoadSaveGame;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        isLoadSaveGame = false;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey("IsSave");
    }
    public void Save()
    {
        PlayerPrefs.SetInt("IsSave", 0);
        SavePlayerData();
        SaveInventoryData();
        SaveOnGroundItemData();

        GC.Collect();
    }

    public void LoadGame()
    {
        isLoadSaveGame = false;
        if (!HasSaveData())
        {
            Debug.LogError("저장된 데이터가 없습니다.");
            return;
        }

        LoadPlayerData();
        LoadInventoryData();
        LoadOnGroundItemData();

        GC.Collect();
    }


    private void SaveJsonData(JsonData data) 
    {
        string jsonData = JsonUtility.ToJson(data);

        string path = GetJsonDataPath(data.typeName);
        var file = new System.IO.FileInfo(path);
        file.Directory.Create();
        System.IO.File.WriteAllText(file.FullName, jsonData);
    }

    private T LoadJsonData<T>(string typeName)
    {
        string path = GetJsonDataPath(typeName);
        var file = new System.IO.FileInfo(path);

        if(file.Exists == false)
        {
            Debug.LogError("파일 없음: " + path);
            return default(T);
        }

        string jsonData = System.IO.File.ReadAllText(file.FullName);
        return JsonUtility.FromJson<T>(jsonData);
    }

    private string GetJsonDataPath(string typeName)
    {
        return string.Format("Assets/JsonResource/{0}.json", typeName);
    }

    #region Save
    private void SavePlayerData()
    {
        var data = new PlayerData();

        data.pos = GameManager.instance.player.transform.position;

        SaveJsonData(data);
    }

    private void SaveInventoryData()
    {
        var data = new InventoryData();
        InventoryManager inventory = GameManager.instance.inventoryManager;
        Slot[] slots = inventory.slots;
        data.itemKindCount = inventory.itemKindCount;

        for(int i = 0; i < data.itemKindCount; i++)
        {
            data.slotdata[i].itemName = slots[i].item.itemName;
            data.slotdata[i].itemCount = slots[i].itemCount;
        }

        SaveJsonData(data);
    }

    private void SaveOnGroundItemData()
    {
        var data = new OnGroundItemManagerData();
        var items = OnGroundItemManager.instance.allOnGroundItems;

        //Debug.Log("onGround Item 갯수: " + items.Count);

        for(int i = 0; i < data.groundItemData.Length; i++)
        {
            data.groundItemData[i].itemName = items[i].objectName;
            data.groundItemData[i].position = items[i].transform.position;
        }

        SaveJsonData(data);
    }

    #endregion

    #region Load
    private void LoadPlayerData()
    {
        PlayerData playerData = new PlayerData();
        playerData = LoadJsonData<PlayerData>(playerData.typeName);

        GameManager.instance.player.transform.position = playerData.pos;
    }

    private void LoadInventoryData()
    {
        InventoryManager inventory = GameManager.instance.inventoryManager;
        Slot[] slots = inventory.slots;

        InventoryData inventoryData = new InventoryData();
        inventoryData = LoadJsonData<InventoryData>(inventoryData.typeName);
        SlotData[] slotData = inventoryData.slotdata;


        for (int i = 0; i < inventoryData.itemKindCount; i++)
        {
            if (slotData[i].itemCount == 0) break;

            InHandItem nowItem = InHandItemManager.instance.GetItemPrefab(slotData[i].itemName);

            nowItem = Instantiate(nowItem.gameObject).GetComponent<InHandItem>();

            inventory.AddItem(nowItem);
            slots[i].itemCount = slotData[i].itemCount;
            slots[i].SetCountText();
        }


        StartCoroutine(LateSetNowItem());
    }

    private IEnumerator LateSetNowItem()
    {
        //현재 아이템 표시해주는 프레임이 이상한곳으로 가서
        //한프레임 쉬고 설정되도록 함.
        yield return null;
        GameManager.instance.inventoryManager.SetNowItem();
    }

    private void LoadOnGroundItemData()
    {
        var manager = OnGroundItemManager.instance;
        manager.DeleteAllItem();

        var managerData = new OnGroundItemManagerData();
        managerData = LoadJsonData<OnGroundItemManagerData>(managerData.typeName);
        var itemData = managerData.groundItemData;

        for(int i = 0; i < managerData.groundItemData.Length; i++)
        {
            OnGroundItem itemPrefab = manager.GetItemPrefab(itemData[i].itemName);
            if (itemPrefab == null) continue;
            GameObject item = Instantiate(itemPrefab.gameObject);
            item.transform.position = itemData[i].position;
        }

    }


    #endregion
}
