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
        Cursor.lockState = CursorLockMode.Confined;
        fadeImage.DOColor(Color.clear, GameData.sceneChangeFadeTime)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void Button_NewGame()
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
            .OnComplete(()=> SceneManager.LoadScene("GameScene"));
    }

    public void Button_Option()
    {
        OptionWindow.SetActive(true);
    }


}
