using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Ghost : MonoBehaviour
{
    public static Transform playerTransform;
    public AudioClip chasingSoundClip;
    public LayerMask playerMask;
    public Animator animator;
    public Transform[] patrolPoints;

    [HideInInspector] public Transform songPyeon;

    private AudioSource chasingSoundPlayer;
    private NavMeshAgent navMesh;
    private RaycastHit hit;

    private int patrolCount;
    private int hash_chasing;
    private int hash_stun;
    private bool isInsidePlayer;
    private bool isInsideSongPyeon;

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
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
            songPyeon = other.transform;
        }
        if (CheckIsPlayer(other))
        {
            chasingSoundPlayer = SFXPlayer.instance.Play(chasingSoundClip);
            isInsidePlayer = true;
            animator.SetBool(hash_chasing, true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckIsSongPyeon(other))
        {
            isInsideSongPyeon = false;
        }
        if (CheckIsPlayer(other))
        {
            ExitPlayer();
        }
    }

    private void ExitPlayer()
    {
        chasingSoundPlayer.Stop();
        isInsidePlayer = false;
        animator.SetBool(hash_chasing, false);
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
        SongPyeon_Used temp = other.GetComponent<SongPyeon_Used>();
        if (temp != null)
        {
            return true;
        }
        return false;
    }


    IEnumerator MoveStart()
    {
        while (true)
        {
            if (isInsideSongPyeon)
            {
                if (songPyeon == null) isInsideSongPyeon = false;
                else navMesh.SetDestination(songPyeon.transform.position);
            }
            else if (isInsidePlayer)
            {
                if (!playerTransform.gameObject.activeSelf) ExitPlayer();

                navMesh.SetDestination(playerTransform.position);

                if (CheckKillPlayer())
                {
                    GameManager.instance.GameOver(0);
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
                Patrol();
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
