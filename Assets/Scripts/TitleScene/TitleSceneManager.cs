using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TitleSceneManager : MonoBehaviour
{
    public GameObject OptionWindow;
    public Image fadeImage;

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Button_NewGame()
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOColor(Color.black, 1)
            .OnComplete(()=> SceneManager.LoadScene("GameScene"));
    }

    public void Button_Option()
    {
        OptionWindow.SetActive(true);
    }


}
