using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public InventoryManager inventoryManager;
    public InstallingBar installingBar;
    public GameObject player;
    public GameObject hideWindow;
    public GameObject pauseWindow;
    public Image fadeImage;
    public Image[] gameOverImages;
    public AudioSource dieBGM;
    public AudioClip dieSFX;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Talisman_InHand.installingBar = installingBar;
        Slot.inventoryManager = inventoryManager;
        Ghost.playerTransform = player.transform;
        Cursor.lockState = CursorLockMode.Locked;

        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.clear, GameData.sceneChangeFadeTime)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

    private void Update()
    {
        SetPauseWindow();
    }

    private void SetPauseWindow()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseWindow.activeSelf)
        {
            pauseWindow.SetActive(true);
        }
    }

    public void LoadTitleScene()
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
            .OnComplete(() => SceneManager.LoadScene("TitleScene"));
    }


    public void GameOver(int index)
    {
        dieBGM.Play();
        SFXPlayer.instance.Play(dieSFX);
        gameOverImages[index].gameObject.SetActive(true);
        gameOverImages[index].DOColor(Color.grey, 2);
        StartCoroutine(GameOverDelay());
    }

    private IEnumerator GameOverDelay()
    {
        yield return WaitTimeManager.WaitForSeconds(5);
        LoadTitleScene();
    }
}
