using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Ghost : MonoBehaviour
{
    public static Transform playerTransform;
    public AudioSource chasingSound;
    public LayerMask playerMask;
    public Animator animator;
    public Transform[] patrolPoints;

    [HideInInspector] public Transform nowTarget;

    private NavMeshAgent navMesh;
    private RaycastHit hit;
    private int patrolCount;
    private bool isPatrol;
    private bool isInsidePlayer;
    private bool isInsideSongPyeon;
    private int hash_chasing;
    private int hash_stun;

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
        isPatrol = true;
        isInsidePlayer = false;
        SetAniHash();
        StartCoroutine(MoveStart());
    }

    public void SetAniHash()
    {
        hash_chasing = Animator.StringToHash("Chasing");
        hash_stun = Animator.StringToHash("Stun");
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
            chasingSound.Play();
            isInsidePlayer = true;
            isPatrol = false;
            if (!isInsideSongPyeon) nowTarget = other.transform;
            animator.SetBool(hash_chasing, true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckIsPlayer(other))
        {
            chasingSound.Stop();
            isInsidePlayer = false;
            if (!isInsideSongPyeon) isPatrol = true;
            animator.SetBool(hash_chasing, false);
        }
        if (CheckIsSongPyeon(other))
        {
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
                if (CheckKillPlayer())
                {
                    GameManager.instance.GameOver(0);
                }
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
        animator.SetBool(hash_stun, true);
        navMesh.speed = 0;
        yield return WaitTimeManager.WaitForSeconds(stunTime);
        navMesh.speed = 3;
        animator.SetBool(hash_stun, false);
    }

    private bool CheckKillPlayer()
    {
        bool temp;
        temp = Physics.Raycast
            (transform.position + Vector3.up, transform.forward, out hit, 1, playerMask);

        return temp && !hit.collider.isTrigger;
    }
}
