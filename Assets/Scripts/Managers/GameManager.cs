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

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Talisman.installingBar = installingBar;
        Slot.inventoryManager = inventoryManager;
        Ghost.playerTransform = player.transform;
        Cursor.lockState = CursorLockMode.Locked;

        fadeImage.DOColor(Color.clear, 1)
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
        fadeImage.DOColor(Color.black, 1)
            .OnComplete(() => SceneManager.LoadScene("TitleScene"));
    }

}
