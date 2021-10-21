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
    private bool isCrafting;

    void Start()
    {
        isCrafting = false;
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
        if (isCrafting) return;

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

        
        TryCraft();
    }


    public void TryCraft()
    {
        //조합 칸이 다 채워졌는지 검사
        for (int i = 0; i < craftImages.Length; i++)
        {
            if (craftImages[i].sprite == null)
            {
                return;
            }
        }

        //조합 목록에 아이템이 있는지 검사
        craftResult = CraftRecipe.GetCraftResult(craftSlots[0].item.objectName, craftSlots[1].item.objectName);

        if (craftResult.Equals("")) return;
        foreach (Item item in craftItems)
        {
            if (craftResult.Equals(item.objectName))
            {
                StartCoroutine(MakeItem(item));
            }
        }
    }
    
    IEnumerator MakeItem(Item item)
    {
        isCrafting = true;
        yield return new WaitForSeconds(1);
        Item temp = Instantiate(item.gameObject).GetComponent<Item>();

        for (int i = 0; i < craftImages.Length; i++)
        {
            craftSlots[i].DownCount();
            craftImages[i].sprite = null;
            craftSlots[i] = null;
        }

        inventoryManager.SortInventory();
        inventoryManager.AddItem(temp);
        inventoryManager.SetNowItem();
        isCrafting = false;
    }

    //private void MakeItem(Item item)
    //{
    //    Item temp = Instantiate(item.gameObject).GetComponent<Item>();

    //    for(int i = 0; i < craftImages.Length; i++)
    //    {
    //        craftSlots[i].DownCount();
    //        craftImages[i].sprite = null;
    //        craftSlots[i] = null;
    //    }

    //    inventoryManager.SortInventory();
    //    inventoryManager.AddItem(temp);
    //    inventoryManager.SetNowItem();
    //}



    public void ClickOnCraftItem()
    {
        if (isCrafting) return;
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        clickedObject.GetComponent<Image>().sprite = null;

        for (int i = 0; i < craftImages.Length; i++)
        {
            if (craftImages[i].sprite == null)
            {
                craftSlots[i] = null;
            }
        }
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
        if (isCrafting) return;

        craftWindow.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerController.onTab = false;

        for (int i = 0; i < craftImages.Length; i++)
        {
            craftImages[i].sprite = null;
            craftSlots[i] = null;
        }
    }
    private void OnCraftWindow()
    {
        craftWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerController.onTab = true;
    }



    #endregion
}
