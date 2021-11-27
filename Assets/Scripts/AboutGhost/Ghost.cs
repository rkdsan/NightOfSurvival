using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Ghost : MonoBehaviour
{
    public static Transform playerTransform;
    public Transform[] patrolPoints;
    public Text stateText;

    [HideInInspector] public Transform nowTarget;

    private NavMeshAgent navMesh;
    private int patrolCount;
    private bool isPatrol;
    private bool isInsidePlayer;
    private bool isInsideSongPyeon;

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
        isPatrol = true;
        isInsidePlayer = false;
        StartCoroutine(MoveStart());
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (CheckIsSongPyeon(other))
        {
            isInsideSongPyeon = true;
            isPatrol = false;
            nowTarget = other.transform;
        }
        if (CheckIsPlayer(other))
        {
            isInsidePlayer = true;
            isPatrol = false;
            if (!isInsideSongPyeon) nowTarget = other.transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckIsPlayer(other))
        {
            isInsidePlayer = false;
            if (!isInsideSongPyeon) isPatrol = true;
        }
        if (CheckIsSongPyeon(other))
        {
            Debug.Log("송편 나감");
            ExitSongPyeon();   
        }
    }

    private bool CheckIsPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    private bool CheckIsSongPyeon(Collider other)
    {
        SongPyeon temp = other.GetComponent<SongPyeon>();
        if (temp != null)
        {
            return true;
        }
        return false;
    }

    private void ExitSongPyeon()
    {
        isInsideSongPyeon = false;
        if (isInsidePlayer) nowTarget = playerTransform;
        else isPatrol = true;
    }

    IEnumerator MoveStart()
    {
        while (true)
        {
            if (nowTarget == null)
            {
                ExitSongPyeon();
            }

            if (isPatrol)
            {
                yield return new WaitForSeconds(1f);
                Patrol();
            }
            else
            {
                navMesh.SetDestination(nowTarget.position);
            }

            yield return WaitTimeManager.WaitForSeconds(0.2f);
        }
    }


    private void Patrol()
    {
        if (navMesh.velocity == Vector3.zero)
        {
            navMesh.SetDestination(patrolPoints[patrolCount++].position);
            if (patrolCount >= patrolPoints.Length) patrolCount = 0;
        }
    }

    public void Stuned(int stunTime)
    {
        StartCoroutine(StunTimer(stunTime));
    }

    IEnumerator StunTimer(int stunTime)
    {
        navMesh.speed = 0;
        yield return WaitTimeManager.WaitForSeconds(stunTime);
        navMesh.speed = 3;
    }

}
