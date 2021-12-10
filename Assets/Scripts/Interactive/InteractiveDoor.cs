using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoor : InteractiveObject
{
    public GameObject targetDoor;

    private static string openString = "LB: 열기";
    private static string closeString = "LB: 닫기";

    private bool isOpen;
    private bool isMoving;
    private Vector3 originPos;
    private Vector3 targetPos;

    private void Reset()
    {
        objectName = "문";
        explainComment = "LB: 열기";
    }

    void Start()
    {
        isOpen = false;
        isMoving = false;
        originPos = transform.position;
        targetPos.x = 0;
        SetTargetPos();
    }


    public override void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveDoor());
        }
    }

    IEnumerator MoveDoor()
    {
        isMoving = true;
        int repeat = 20;
        Vector3 addPos = targetPos / repeat;
        if (isOpen) addPos *= -1;

        while (repeat-- > 0)
        {
            transform.position += addPos;
            yield return WaitTimeManager.WaitForFixedUpdate();
        }
        isMoving = false;
        isOpen = !isOpen;
        SetComment();
    }

    private void SetComment()
    {
        if (isOpen) explainComment = closeString;
        else explainComment = openString;
    }

    private void SetTargetPos()
    {
        if (targetDoor == null) return;
        Vector3 gap = targetDoor.transform.position - transform.position;

        gap.y = 0;
        if(Mathf.Abs(gap.x) > Mathf.Abs(gap.z))
        {
            gap.z = 0;
        }
        else
        {
            gap.x = 0;
        }

        targetPos = gap;
    }
}
