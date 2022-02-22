using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    public event Action _Interact;
    private static string openString = "LB: 열기";
    private static string closeString = "LB: 닫기";


    private void Reset()
    {
        objectName = "문";
        explainComment = "LB: 열기";
    }

    public override void Interact()
    {
        _Interact();
    }

    public void SetComment(bool isOpen)
    {
        if (isOpen) explainComment = closeString;
        else explainComment = openString;
    }

}
