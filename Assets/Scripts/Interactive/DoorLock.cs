using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : InteractiveObject
{
    public const string BEED_NAME = "����";
    public GameObject[] beeds;
    public GameObject BlockObject;
    public GameObject Clasp;

    private InventoryManager inven;

    private int lastIdx = 0;

    private void Start()
    {
        inven = GameManager.instance.inventoryManager;
        foreach(GameObject beed in beeds)
        {
            beed.SetActive(false);
        }
    }

    public override void Interact()
    {
        Slot slot = inven.FindItem(BEED_NAME);
        if (slot != null)
        {
            slot.ConsumeItem();

            if (lastIdx < 3)
            {
                beeds[lastIdx++].SetActive(true);
            }

            if(lastIdx == 3)
            {
                OpenDoor();
            }

        }
    }

    private void OpenDoor()
    {
        Destroy(BlockObject);
        Clasp.SetActive(false);

    }


}
