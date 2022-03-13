using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryPart : MonoBehaviour
{
    public string fileName;
    public Button button;
    public List<Sprite> explainSpriteList;
    public List<ExplainData> explainDataList;


    public void SetExplaindata()
    {
        explainDataList = new List<ExplainData>();
        TextAsset assetData = Resources.Load(fileName) as TextAsset;
        string data = assetData.text;

        foreach(string line in data.Split('\n'))
        {
            string[] str = line.Split(',');
            if (str.Length < 2) break;
            explainDataList.Add(new ExplainData(str[0], str[1]));
        }


        if(explainDataList.Count != explainSpriteList.Count)
        {
            Debug.LogError("귀신도감 데이터 개수가 맞지 않습니다. 파일이름: " + fileName
                + "\n Sprite개수: " + explainSpriteList.Count + ", 데이터 개수: " + explainDataList.Count);
        }
    }

}
