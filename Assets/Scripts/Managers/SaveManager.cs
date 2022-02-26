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

    private string LoadJsonData(string path)
    {
        var file = new System.IO.FileInfo(path);

        if(file.Exists == false)
        {
            Debug.LogError("파일 없음: " + path);
            return null;
        }

        return System.IO.File.ReadAllText(file.FullName);

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

    #endregion

    #region Load
    private void LoadPlayerData()
    {
        string data = LoadJsonData(GetJsonDataPath("PlayerData"));
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

        GameManager.instance.player.transform.position = playerData.pos;
    }

    private void LoadInventoryData()
    {
        string data = LoadJsonData(GetJsonDataPath("InventoryData"));
        InventoryManager inventory = GameManager.instance.inventoryManager;
        Slot[] slots = inventory.slots;

        InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(data);
        SlotData[] slotData = inventoryData.slotdata;


        for (int i = 0; i < inventoryData.itemKindCount; i++)
        {
            if (slotData[i].itemCount == 0) break;

            InHandItem nowItem = InHandItemManager.instance.GetItemPrefab(slotData[i].itemName);

            nowItem = Instantiate(nowItem.gameObject).GetComponent<InHandItem>();

            inventory.AddItem(nowItem);
            slots[i].itemCount = slotData[i].itemCount;
        }


        StartCoroutine(LateSetNowItem());
    }

    private IEnumerator LateSetNowItem()
    {
        //아이템 프레임이 포지션에 맞게 조절되는데 프레임이 이상한곳으로 감.
        //포지션이 설정되기전에 실행되는것으로 추측
        //업데이트 한번 끝나고 실행되도록 설정
        yield return null;
        GameManager.instance.inventoryManager.SetNowItem();
    }

    #endregion
}
