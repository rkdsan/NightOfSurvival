using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InHandItem : MonoBehaviour
{
    public Sprite itemImage;
    public Vector3 offsetPosition;
    public Vector3 offsetRotate;
    public string objectName;
    public bool canCraft;
    public bool isConsumable;

    private void Reset()
    {
        canCraft = false;
        isConsumable = false;
    }

    public abstract bool UseItem();

}
