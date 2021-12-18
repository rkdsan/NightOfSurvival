using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    public DoorBinder binder;
    private static string openString = "LB: 열기";
    private static string closeString = "LB: 닫기";


    private void Reset()
    {
        objectName = "문";
        explainComment = "LB: 열기";
    }

    public override void Interact()
    {
        if (!binder.isMoving)
        {
            binder.Interact();
        }
    }

    public void SetComment()
    {
        if (binder.isOpen) explainComment = closeString;
        else explainComment = openString;
    }

}
