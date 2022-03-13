using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainData
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
    [Header("Button")]
    public Button itemButton;
    public Button ghostButton;

    [Header("UI")]
    public Image explainImage;
    public Text explainTitle;
    public Text explainContents;

    [Space(20)]
    public List<DictionaryPart> partList;

    private List<Sprite> nowSpriteList;
    private List<ExplainData> nowExplainDataList;
    private int nowIndex = 0;

    

    private void Awake()
    {
        SetPart();
    }

    private void SetPart()
    {
        foreach(var part in partList)
        {
            part.SetExplaindata();
            part.button.onClick.AddListener(() =>
            {
                nowSpriteList = part.explainSpriteList;
                nowExplainDataList = part.explainDataList;
                nowIndex = 0;
                SetPostItSibling(part);
                SetExplain();
            });
        }

        partList[0].button.onClick.Invoke();

    }

    private void SetExplain()
    {
        explainImage.sprite = nowSpriteList[nowIndex];
        explainTitle.text = partList[0].explainDataList[0].title;
        explainContents.text = nowExplainDataList[nowIndex].contents;
    }

    private void SetPostItSibling(DictionaryPart part)
    {
        foreach (var _part in partList)
        {
            _part.transform.SetAsFirstSibling();
        }

        part.transform.SetAsLastSibling();
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

