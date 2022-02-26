using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman_Used : MonoBehaviour
{
    public GameObject explosionParticle;
    public AudioClip boomSound;

    private RaycastHit hit;
    private Collider nowGhost;
    private IEnumerator betweenCheckerEnumerator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            nowGhost = other; 
            betweenCheckerEnumerator = CheckBetweenObstacle();
            StartCoroutine(betweenCheckerEnumerator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            StopCoroutine(betweenCheckerEnumerator);
        }
    }
    
    //고스트와 사이에 장애물 있는지 체크
    private IEnumerator CheckBetweenObstacle()
    {
        while (true)
        {
            if (CanSeeGhost())
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

    private bool CanSeeGhost()
    {
        //귀신이 공중에 떠있어서 up을 더해줌
        Vector3 dir = nowGhost.transform.position + Vector3.up * 0.1f - transform.position;
        Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out hit);

        return hit.collider.CompareTag("Ghost");
    }
}
