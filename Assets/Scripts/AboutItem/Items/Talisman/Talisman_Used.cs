using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman_Used : MonoBehaviour
{
    public GameObject explosionParticle;
    public AudioClip boomSound;

    private RaycastHit hit;
    private Collider nowGhost;
    private IEnumerator enumerator;

    private void Awake()
    {
        enumerator = CheckThrough();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            nowGhost = other;
            StartCoroutine(enumerator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            StopCoroutine(enumerator);
        }
    }

    private IEnumerator CheckThrough()
    {
        while (true)
        {
            if (CheckGhostTag())
            {
                Boom();
                break;
            }
            yield return WaitTimeManager.WaitForFixedUpdate();
        }
    }


    private void Boom()
    {
        nowGhost.GetComponent<Ghost>().Stuned(2);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        SFXPlayer.instance.Play(boomSound);
        Destroy(gameObject);
    }

    private bool CheckGhostTag()
    {
        //커신이 공중에 떠있어서 up을 더해줌
        Vector3 dir = nowGhost.transform.position + Vector3.up * 0.1f - transform.position;


        Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out hit);


        return hit.collider.CompareTag("Ghost");
        
    }

}
