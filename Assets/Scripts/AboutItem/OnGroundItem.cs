using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnGroundItem : InteractiveObject
{
    public GameObject inHandPrefab;

    private InHandItem inHandItem;

    private void Awake()
    {
        inHandItem = Instantiate(inHandPrefab, transform.position, Quaternion.identity)
            .GetComponent<InHandItem>();
        inHandItem.transform.parent = transform;
        inHandItem.gameObject.SetActive(false);
        OnGroundItemManager.instance.allOnGroundItems.Add(this);
    }

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
