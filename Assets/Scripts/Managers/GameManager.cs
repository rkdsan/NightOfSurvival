using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public InventoryManager inventoryManager;
    public InstallingBar installingBar;
    public GameObject player;
    public GameObject hideWindow;
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

        fadeImage.DOColor(Color.clear, 1)
            .OnComplete(() => fadeImage.raycastTarget = false);
    }

}
