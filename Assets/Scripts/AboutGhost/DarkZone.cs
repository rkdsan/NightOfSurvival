using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkZone : MonoBehaviour
{
    public Lamp[] lamps;
    public LayerMask playerLayer;

    private Collider[] colls;
    private int inDarkTime;
    private bool inPlayer;

    void Start()
    {
        inDarkTime = 0;
        inPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !inPlayer)
        {
            StartCoroutine(CheckInLamp());
        }
    }


    public IEnumerator CheckInLamp()
    {
        inPlayer = true;
        while (true)
        {
            foreach (Lamp lamp in lamps)
            {
                if (lamp.lampLight.enabled)
                {
                    colls = Physics.OverlapSphere(lamp.transform.position, 3f, playerLayer);
                    if (colls.Length > 0)
                    {
                        inDarkTime = -1;
                        break;
                    }
                }
            }

            inDarkTime++;
            yield return WaitTimeManager.WaitForSeconds(1);
        }
    }




}
