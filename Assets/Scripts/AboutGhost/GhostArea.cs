using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostArea : MonoBehaviour
{
    public Ghost ghost;

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (CheckIsSongPyeon(other))
    //    {
    //        isInsideSongPyeon = true;
    //        isPatrol = false;
    //        nowTarget = other.transform;
    //    }
    //    if (CheckIsPlayer(other))
    //    {
    //        isInsidePlayer = true;
    //        isPatrol = false;
    //        if (!isInsideSongPyeon) nowTarget = other.transform;
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (CheckIsPlayer(other))
    //    {
    //        isInsidePlayer = false;
    //        if (!isInsideSongPyeon) isPatrol = true;
    //    }
    //    if (CheckIsSongPyeon(other))
    //    {
    //        isInsideSongPyeon = false;
    //        if (isInsidePlayer) nowTarget = playerTransform;
    //        else isPatrol = true;
    //    }
    //}

}


