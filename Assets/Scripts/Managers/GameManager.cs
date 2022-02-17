using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Kino;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController playerController;
    public InventoryManager inventoryManager;
    public InstallingBar installingBar;
    public AnalogGlitch glitch;

    [Header("GameObject")]
    public GameObject player;
    public GameObject hideWindow;

    [Header("Image")]
    public Image fadeImage;
    public Image[] gameOverImages;

    [Header("Sound")]
    public AudioSource dieBGM;
    public AudioClip dieSFX;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("게임매니저 인스턴스 중복");
        }
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


    public void LoadTitleScene()
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOColor(Color.black, GameData.sceneChangeFadeTime)
            .OnComplete(() => SceneManager.LoadScene("TitleScene"));
    }


    public void GameOver(int index)
    {
        
        
        StartCoroutine(GameOverDelay(index));
        
    }

    

    private IEnumerator GameOverDelay(int index)
    {
        yield return WaitTimeManager.WaitForSeconds(1);

        dieBGM.Play();
        SFXPlayer.instance.Play(dieSFX);

        gameOverImages[index].gameObject.SetActive(true);
        gameOverImages[index].DOColor(Color.grey, 2);

        yield return WaitTimeManager.WaitForSeconds(4);

        //SFXPlayer.instance.StopAllSFX();
        LoadTitleScene();
    }
}
