using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DangerArea : MonoBehaviour
{
    public Ghost ghost;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayer(other))
        {
            ghost.SetTargetPlayer(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayer(other))
        {
            ghost.CancelTargetPlayer();
        }
    }

    private bool isPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

}
