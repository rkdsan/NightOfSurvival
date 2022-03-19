using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    public PopupUI pausePopup;
    public PopupUI optionPopup;
    public PopupUI ghostDictionaryPopup;

    private Stack<PopupUI> activePopupList;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePopupList.Count > 0)
                ClosePopup();
            else
                OpenPopup(pausePopup);
        }
    }

    private void Init()
    {
        activePopupList = new Stack<PopupUI>();

        List<PopupUI> allPopupList = new List<PopupUI>()
        {
            pausePopup, optionPopup, ghostDictionaryPopup
        };

        foreach(var popup in allPopupList)
        {
            //클로즈 버튼 누르려면 제일 위에있느 객체만 클릭 가능해서 씀
            popup.closeButton.onClick.AddListener(() => ClosePopup());
        }
    }

    private void ClosePopup()
    {
        activePopupList.Pop().gameObject.SetActive(false);
    }

    private void OpenPopup(PopupUI popup)
    {
        activePopupList.Push(popup);
        popup.gameObject.SetActive(true);

    }

    public void Button_Continue()
    {   //이 버튼을 누르려면 이게 제일 위로 와야함
        ClosePopup();
    }

    public void Button_Stop()
    {
        GameManager.instance.LoadTitleScene();
        gameObject.SetActive(false);
    }

    public void Button_Option()
    {
        OpenPopup(optionPopup);
    }

    public void Button_Dictionary()
    {
        OpenPopup(ghostDictionaryPopup);
    }
}
