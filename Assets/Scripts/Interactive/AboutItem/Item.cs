using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : InteractiveObject
{
    public Sprite itemImage;
    public Vector3 originRotate;
    public bool canCraft;

    private void Reset()
    {
        explainComment = "LB: ащ╠Б";
        canCraft = false;
    }

    public abstract void UseItem();

    public override void Interact()
    {
        GameManager.instance.inventoryManager.AddItem(this);
    }
}
