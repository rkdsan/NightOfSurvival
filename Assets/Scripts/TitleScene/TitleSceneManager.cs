using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TitleSceneManager : MonoBehaviour
{
    public static TitleSceneManager instance;
    public GameObject optionWindow;
    public GameObject ghostDictionary;
    public AudioClip buttonClickSound;
    public AudioClip titleBGM;
    public Image fadeImage;
    public GameObject prologueManager;

    public void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Confined;
        Ani_FadeImage();
        
    }

    private void Start()
    {
        BGMPlayer.instance.SetAudioClip(titleBGM);
        BGMPlayer.instance.source.Play();

        optionWindow.SetActive(false);
        ghostDictionary.SetActive(false);
        
    }

    private void Ani_FadeImage()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.clear, GameData.SCREEN_CHANGE_FADE_TIME)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void Button_NewGame()
    {
        prologueManager.SetActive(true);

        //SFXPlayer.instance.Play(buttonClickSound);
        //fadeImage.raycastTarget = true;
        //fadeImage.DOColor(Color.black, GameData.SCREEN_CHANGE_FADE_TIME)
        //    .OnComplete(() => LoadingSceneManager.LoadScene("GameScene"));
    }

    public void LoadNextScene()
    {
        SFXPlayer.instance.Play(buttonClickSound);
        fadeImage.raycastTarget = true;
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
        LoadNextScene();
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
