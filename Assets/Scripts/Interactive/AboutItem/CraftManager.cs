using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftManager : MonoBehaviour
{
    public GameObject craftWindow;
    public Item[] craftItems;
    public Image[] craftImages;

    [HideInInspector] public Slot[] craftSlots;

    private PlayerController playerController;
    private InventoryManager inventoryManager;
    private string craftResult;

    void Start()
    {
        craftSlots = new Slot[craftImages.Length];
        playerController = GameManager.instance.playerController;
        inventoryManager = GameManager.instance.inventoryManager;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTabKey();
    
    }

    public void PutOnCrafter(Slot slot)
    {
        if (craftImages[0].sprite == null)
        {
            if (craftSlots[1] != null && slot.item.objectName.Equals(craftSlots[1].item.objectName))
            {
                return;
            }
            craftSlots[0] = slot;
            craftImages[0].sprite = slot.image.sprite;
        }
        else if (craftImages[1].sprite == null)
        {
            if (craftSlots[0] != null && slot.item.objectName.Equals(craftSlots[0].item.objectName))
            {
                return;
            }
            craftSlots[1] = slot;
            craftImages[1].sprite = slot.image.sprite;
        }

        //for (int i = 0; i < craftImages.Length; i++)
        //{
        //    if (craftImages[i].sprite == null)
        //    { 
        //        craftSlots[i] = slot;
        //        craftImages[i].sprite = slot.image.sprite;
        //        break;
        //    }
        //}
        TryCraft();
    }

    public void TryCraft()
    {
        for (int i = 0; i < craftImages.Length; i++)
        {
            if (craftImages[i].sprite == null)
            {
                return;
            }
        }

        craftResult = CraftRecipe.GetCraftResult(craftSlots[0].item.objectName, craftSlots[1].item.objectName);

        if (craftResult.Equals("")) return;
        foreach (Item item in craftItems)
        {
            if (craftResult.Equals(item.objectName))
            {
                MakeItem(item);
            }
        }
    }
    

    private void MakeItem(Item item)
    {
        Item temp = Instantiate(item.gameObject).GetComponent<Item>();

        for(int i = 0; i < craftImages.Length; i++)
        {
            craftImages[i].sprite = null;
            StartCoroutine(craftSlots[i].DownCountQueue());
            //craftSlots[i].DownCount();
            craftSlots[i] = null;
        }

        //inventoryManager.SortInventory();
        //inventoryManager.SortInventory();
        inventoryManager.AddItem(temp);
    }

    

    public void ClickOnCraftItem()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        clickedObject.GetComponent<Image>().sprite = null;
    }

    #region 창 껐다켜는 기능
    private void CheckTabKey()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (craftWindow.activeSelf)
            {
                OffCraftWindow();
            }
            else
            {
                OnCraftWindow();
            }
        }
    }

    private void OffCraftWindow()
    {
        craftWindow.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerController.onTab = true;
    }
    private void OnCraftWindow()
    {
        craftWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerController.onTab = false;
    }
    #endregion
}
