using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Ghost : MonoBehaviour
{
    public enum GhostTarget
    {
        Patrol,
        Player,
        SongPyeon
    }


    public Transform[] patrolPoints;
    public Text stateText;

    public GhostTarget nowTarget;

    private NavMeshAgent navMesh;
    private int patrolCount;
    

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
        StartCoroutine(MoveToPatrolPoint());
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            stateText.text = "공격중";
        }    
    }

    public void SetTargetSongPyeon(Transform songPyeon)
    {
        nowTarget = GhostTarget.SongPyeon;
        StartCoroutine(FollowingSongPyeon(songPyeon));

    }

    public void SetTargetPlayer(Transform player)
    {
        if (nowTarget == GhostTarget.SongPyeon) return;

        nowTarget = GhostTarget.Player;
        StartCoroutine(FollowingPlayer(player));
    }

    public void SetPatrol()
    {
        if (nowTarget == GhostTarget.SongPyeon) return;

        nowTarget = GhostTarget.Patrol;
        StartCoroutine(MoveToPatrolPoint());
    }

    IEnumerator FollowingSongPyeon(Transform songPyeon)
    {
        stateText.text = "송편 발견";
        while (nowTarget == GhostTarget.SongPyeon)
        {
            navMesh.SetDestination(songPyeon.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowingPlayer(Transform player)
    {
        yield return new WaitForSeconds(0.2f);
        stateText.text = "적 발견";
        while (nowTarget == GhostTarget.Player)
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
        while (nowTarget == GhostTarget.Patrol)
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
