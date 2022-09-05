using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGhost : MonoBehaviour
{
    public static PlayerController playerController;

    private Transform playerTransform;


    private void Start()
    {
        playerTransform = playerController.transform;
    }


    public void StartKillPlayer()
    {
        StartCoroutine(RunToPlayer());
    }

    private IEnumerator RunToPlayer()
    {
        transform.position = playerTransform.position + playerTransform.forward * 30;
        //playerTransform.LookAt(transform);
        playerController.onTab = true;
        playerController.canMove = false;

        transform.LookAt(playerTransform.position + Vector3.up * 0.5f);

        while (GetDistance() > 1)
        {
            transform.position += transform.forward * 0.4f;
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        GameManager.instance.GameOver(1);
    }

    private float GetDistance()
    {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
}
