using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Text stateText;

    private NavMeshAgent navMesh;

    private int patrolCount;
    private bool isFollowingPlayer;

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
        isFollowingPlayer = false;
        StartCoroutine(MoveToPatrolPoint());
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            stateText.text = "공격중";
        }    
    }

    public void SetTargetPlayer(Transform player)
    {
        isFollowingPlayer = true;
        StartCoroutine(FollowingPlayer(player));
    }

    public void CancelTargetPlayer()
    {
        isFollowingPlayer = false;
        StartCoroutine(MoveToPatrolPoint());
    }

    IEnumerator FollowingPlayer(Transform player)
    {
        stateText.text = "적 발견";
        while (isFollowingPlayer)
        {
            navMesh.SetDestination(player.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator MoveToPatrolPoint()
    {
        stateText.text = "순찰중";
        yield return new WaitForSeconds(0.5f);
        navMesh.SetDestination(patrolPoints[patrolCount].position);
        while (!isFollowingPlayer)
        {
            if (navMesh.velocity == Vector3.zero)
            {
                navMesh.SetDestination(patrolPoints[patrolCount++].position);
                if (patrolCount >= patrolPoints.Length)
                {
                    patrolCount = 0;
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

}
