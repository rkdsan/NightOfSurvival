using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TitleSceneManager : MonoBehaviour
{
    public GameObject optionWindow;
    public GameObject ghostDictionary;
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
        fadeImage.DOColor(Color.clear, GameData.SCREEN_CHANGE_FADE_TIME)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void Button_NewGame()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        fadeImage.raycastTarget = true;
        //fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
        //    .OnComplete(()=> SceneManager.LoadScene("GameScene"));
        fadeImage.DOColor(Color.black, GameData.SCREEN_CHANGE_FADE_TIME)
            .OnComplete(() => LoadingSceneManager.LoadScene("GameScene"));
    }

    public void Button_Option()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        optionWindow.SetActive(true);
    }

    public void Button_Continue()
    {
        SaveManager.instance.isLoadSaveGame = true;
        Button_NewGame();
    }

    public void Button_GhostDictionary()
    {
        ghostDictionary.SetActive(true);
    }

    public void Button_Exit()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        Application.Quit();
    }


}
