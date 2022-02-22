using System.Collections;
using UnityEngine;

public class DoorBinder : MonoBehaviour
{
    public Door moveDoorScript;
    public Door targetDoorScript;
    public AudioClip doorSound;

    [HideInInspector] public bool isOpen;
    [HideInInspector] public bool isMoving;

    private Vector3 targetPos;
    private Vector3 addPos;
    private static int repeat = 20;

    void Start()
    {
        isOpen = false;
        isMoving = false;
        targetPos.x = 0;

        moveDoorScript._Interact += () =>{ Interact(); };
        targetDoorScript._Interact += () => { Interact(); };

        SetTargetPos();
    }


    public void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveDoor());
        }
    }

    private IEnumerator MoveDoor()
    {
        isMoving = true;
        SFXPlayer.instance.Play(doorSound);

        int remind = repeat;
        while (remind-- > 0)
        {
            moveDoorScript.transform.position += addPos;
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        isMoving = false;
        isOpen = !isOpen;
        addPos *= -1;

        SetComment();
    }

    private void SetComment()
    {
        moveDoorScript.SetComment(isOpen);
        targetDoorScript.SetComment(isOpen);
    }

    private void SetTargetPos()
    {
        Vector3 gap = targetDoorScript.transform.position - moveDoorScript.transform.position;

        gap.y = 0;
        if (Mathf.Abs(gap.x) > Mathf.Abs(gap.z))
        {
            gap.z = 0;
        }
        else
        {
            gap.x = 0;
        }

        targetPos = gap;
        addPos = targetPos / repeat;
    }
}
