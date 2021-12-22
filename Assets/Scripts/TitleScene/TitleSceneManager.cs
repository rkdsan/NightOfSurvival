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
        Ani_FadeImage();
    }


    private void Ani_FadeImage()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.clear, GameData.sceneChangeFadeTime)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void Button_NewGame()
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
            .OnComplete(()=> SceneManager.LoadScene("GameScene"));
        //LoadingSceneManager.LoatScene("GameScene")
    }

    public void Button_Option()
    {
        OptionWindow.SetActive(true);
    }

    public void Button_Exit()
    {
        Application.Quit();
    }


}
