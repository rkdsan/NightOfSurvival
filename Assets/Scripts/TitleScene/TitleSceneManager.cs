using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TitleSceneManager : MonoBehaviour
{
    public GameObject OptionWindow;
    public AudioClip buttonClickSound; 
    public Image fadeImage;

    public void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Confined;
        Ani_FadeImage();
    }

    private void Start()
    {
        BGMPlayer.instance.source.Play();
    }

    private void Ani_FadeImage()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.clear, GameData.sceneChangeFadeTime)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void Button_NewGame()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        fadeImage.raycastTarget = true;
        //fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
        //    .OnComplete(()=> SceneManager.LoadScene("GameScene"));
        fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
            .OnComplete(() => LoadingSceneManager.LoadScene("GameScene"));
    }

    public void Button_Option()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        OptionWindow.SetActive(true);
    }

    public void Button_Exit()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        Application.Quit();
    }


}
