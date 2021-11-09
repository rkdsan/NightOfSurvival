using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : InteractiveObject
{
    public Sprite itemImage;
    public Vector3 originRotate;
    public bool canCraft;
    public bool isConsumable;

    private void Reset()
    {
        explainComment = "LB: ащ╠Б";
        canCraft = false;
        isConsumable = false;
    }

    public abstract bool UseItem();

    public override void Interact()
    {
        GameManager.instance.inventoryManager.AddItem(this);
    }
}
