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
    public Image gameOverImage;
    public Sprite[] gameOverSprites;

    [Header("Sound")]
    public AudioSource dieBGM;
    public AudioClip dieSFX;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("게임매니저 인스턴스 중복");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Application.targetFrameRate = 60;

    }


    void Start()
    {
        Talisman_InHand.installingBar = installingBar;
        Slot.inventoryManager = inventoryManager;
        Ghost.playerTransform = player.transform;
        Compass_InHand.playerTransform = player.transform;
        Cursor.lockState = CursorLockMode.Locked;

        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.clear, GameData.SCREEN_CHANGE_FADE_TIME)
                 .OnComplete(() => fadeImage.raycastTarget = false);

        CheckLoadSaveGame();
    }

    private void CheckLoadSaveGame()
    {
        if (SaveManager.instance != null && SaveManager.instance.isLoadSaveGame)
        {
            SaveManager.instance.LoadGame();
        }
    }

    public void LoadTitleScene()
    {
        fadeImage.raycastTarget = true;
        
        fadeImage.DOColor(Color.black, GameData.SCREEN_CHANGE_FADE_TIME)
                 .OnComplete(() => SceneManager.LoadScene("TitleScene"));
    }


    public void GameOver(int index)
    {
        SFXPlayer.instance.Play(dieSFX);
        StartCoroutine(EffectGlitch());
        StartCoroutine(GameOverDelay(index));
        
    }
    
    private IEnumerator EffectGlitch()
    {
        float time = 0;

        while (true)
        {
            glitch.scanLineJitter = time;
            glitch.verticalJump = time;
            glitch.horizontalShake = time;
            glitch.colorDrift = time;

            time += Time.deltaTime;
            if (time > 1.0) 
                break;
            yield return null;
        }
    }

    private IEnumerator GameOverDelay(int index)
    {
        yield return WaitTimeManager.WaitForSeconds(1);

        dieBGM.Play();
        gameOverImage.sprite = gameOverSprites[index];
        gameOverImage.gameObject.SetActive(true);
        gameOverImage.DOColor(Color.grey, 2);
        
        //gameOverImages[index].gameObject.SetActive(true);
        //gameOverImages[index].DOColor(Color.grey, 2);

        yield return WaitTimeManager.WaitForSeconds(4);

        SFXPlayer.instance.StopAllSFX();
        LoadTitleScene();
    }
}
