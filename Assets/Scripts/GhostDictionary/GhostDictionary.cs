using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ExplainData
{
    public string title;
    public string contents;

    public ExplainData(string title, string contents)
    {
        this.title = title;
        this.contents = contents;
    }
}

public class GhostDictionary : MonoBehaviour
{
    

    public Image explainImage;
    public Text explainTitle;
    public Text explainContents;

    public List<Sprite> itemExplainSpriteList;
    public List<Sprite> ghostExplainSpriteList;

    private List<ExplainData> itemExplainDataList;
    private List<ExplainData> ghostExplainDataList;

    private List<Sprite> nowSpriteList;
    private List<ExplainData> nowExplainList;
    private int nowIndex = 0;

    

    private void Awake()
    {
        SetItemExplainData();
        SetGhostExplainData();

        nowSpriteList = itemExplainSpriteList;
        nowExplainList = itemExplainDataList;
        SetExplain();
    }

    private void SetExplain()
    {
        explainImage.sprite = nowSpriteList[nowIndex];
        explainTitle.text = nowExplainList[nowIndex].title;
        explainContents.text = nowExplainList[nowIndex].contents;
    }

    private void SetGhostExplainData()
    {
        ghostExplainDataList = new List<ExplainData>();
        TextAsset assetData = Resources.Load("GhostExplainData") as TextAsset;
        SetExplainData(ghostExplainDataList, assetData);

        if(ghostExplainDataList.Count != ghostExplainSpriteList.Count)
        {
            Debug.LogError("귀신도감 귀신 이미지와 설명 데이터 갯수가 맞지않음.");
        }
    }

    private void SetItemExplainData()
    {
        itemExplainDataList = new List<ExplainData>();
        TextAsset assetData = Resources.Load("ItemExplainData") as TextAsset;
        SetExplainData(itemExplainDataList, assetData);

        if (itemExplainDataList.Count != itemExplainSpriteList.Count)
        {
            Debug.LogError("귀신도감 아이템 이미지와 설명 데이터 갯수가 맞지않음.");
        }

    }

    private void SetExplainData(List<ExplainData> list, TextAsset assetData)
    {
        string data = assetData.text;

        foreach (string line in data.Split('\n'))
        {
            string[] str = line.Split(',');
            if (str.Length < 2) return;
            list.Add(new ExplainData(str[0], str[1]));
        }
    }

    public void Button_ItemPostIt()
    {
        nowSpriteList = itemExplainSpriteList;
        nowExplainList = itemExplainDataList;
        nowIndex = 0;
        SetExplain();
    }

    public void Button_GhostPostIt()
    {
        nowSpriteList = ghostExplainSpriteList;
        nowExplainList = ghostExplainDataList;
        nowIndex = 0;
        SetExplain();
    }

    public void Button_Next()
    {
        nowIndex++;
        if (nowIndex >= nowSpriteList.Count) nowIndex = 0;
        SetExplain();
    }

    public void Button_Close()
    {
        gameObject.SetActive(false);
    }
}

