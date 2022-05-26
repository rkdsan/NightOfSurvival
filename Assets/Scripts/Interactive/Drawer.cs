using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractiveObject
{
    public Transform targetTransform;

    private static string openString = "LB: 열기";
    private static string closeString = "LB: 닫기";

    private Vector3 originPos;
    private Vector3 targetPos;
    private Vector3 moveVector;
    
    private bool isOpen;
    private bool isMoving;

    private void Awake()
    {
        isOpen = false;
        originPos = transform.position;
        targetPos = targetTransform.position;
        moveVector = (targetPos - originPos)*0.05f;
    }

    private void Reset()
    {
        objectName = "서랍";
        explainComment = "LB: 열기";
    }

    public override void Interact()
    {
        if (isMoving) return;

        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        isMoving = true;

        int repeat = 20;

        while (repeat-- > 0)
        {
            transform.position += moveVector;
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        isMoving = false;
        isOpen = !isOpen;
        moveVector *= -1;

        if (isOpen) explainComment = closeString;
        else explainComment = openString;
    }

}
