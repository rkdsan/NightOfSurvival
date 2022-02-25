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

    public void LoadGame()
    {
        isLoadSaveGame = false;
        if (!HasSaveData())
        {
            Debug.LogError("저장된 데이터가 없습니다.");
            return;
        }

        LoadPlayerData();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("IsSave", 0);
        SavePlayerData();    
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
    #endregion

    #region Load
    private void LoadPlayerData()
    {
        string data = LoadJsonData(GetJsonDataPath("PlayerData"));

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

        GameManager.instance.player.transform.position = playerData.pos;
    }
    #endregion
}
