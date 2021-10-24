using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public InventoryManager inventoryManager;
    public GameObject player;
    public GameObject hideWindow;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 144;
    }

    void Start()
    {
        Slot.inventoryManager = inventoryManager;
        Ghost.playerTransform = player.transform;
    }

}
