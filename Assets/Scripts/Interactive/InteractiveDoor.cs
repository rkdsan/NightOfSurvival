using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoor : InteractiveObject
{
    private bool isOpen;
    private bool isMoving;
    private Vector3 movePos;

    public string openString;
    public string closeString;

    void Start()
    {
        isOpen = false;
        isMoving = false;
        movePos = new Vector3(2, 0, 0);
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
        int repeat = 10;
        Vector3 addPos = movePos / repeat;
        if (isOpen) addPos *= -1;

        while (repeat-- > 0)
        {
            transform.position += addPos;
            yield return new WaitForFixedUpdate();
        }
        isMoving = false;
        isOpen = !isOpen;
        SetComment();
    }

    public void SetComment()
    {
        if (isOpen) explainComment = closeString;
        else explainComment = openString;
    }

}
