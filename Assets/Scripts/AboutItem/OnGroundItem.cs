using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnGroundItem : InteractiveObject
{
    public InHandItem inHandItem;

    private void Reset()
    {
        explainComment = "LB: ащ╠Б";
    }

    public override void Interact()
    {
        GameManager.instance.inventoryManager.AddItem(inHandItem);
        Destroy(gameObject);
    }
}
