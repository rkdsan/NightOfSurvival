using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kino;

public class GhostWoman : Ghost 
{
    public AudioClip chasingSoundClip;
    public AudioClip neckCrackingSoundClip;
    public LayerMask playerMask;
    public Transform[] patrolPoints;

    [HideInInspector] public Transform songPyeon;

    private AudioSource chasingSoundPlayer;
    private AnalogGlitch glitch;
    private RaycastHit hit;
    private IEnumerator betweenCheckerEnumerator;

    private int patrolCount = 0;
    private bool isInsidePlayer = false;
    private bool isInsideSongPyeon;
    

    private void Start()
    {
        glitch = GameManager.instance.glitch;
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
            betweenCheckerEnumerator = CheckBetweenObstacle();
            StartCoroutine(betweenCheckerEnumerator);

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
            StopCoroutine(betweenCheckerEnumerator);
            SetSpeed();
        }
    }


    //������ ������ �� ���̿� ���� �ִ��� üũ
    private IEnumerator CheckBetweenObstacle()
    {
        while (true)
        {
            if (CanSeePlayer())
            {
                animator.SetBool(hash_chasing, true);
                SFXPlayer.instance.Play(neckCrackingSoundClip);

                navMesh.speed = 0;

                //����� �ִϸ��̼����� üũ�ϱ����� 1������ �ǳʶ�
                yield return null;
                while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    yield return null;
                }
                SetSpeed();
                navMesh.SetDestination(playerTransform.position);

                chasingSoundPlayer = SFXPlayer.instance.Play(chasingSoundClip);
                isInsidePlayer = true;
                SetGlitch();
                break;
            }

            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        float cantSeeTime = 0;
        while (isInsidePlayer)
        {
            if (CanSeePlayer())
            {
                cantSeeTime = 0;
            }
            else
            {
                cantSeeTime += Time.fixedDeltaTime;
                if(cantSeeTime > 5)
                {
                    ExitPlayer();
                    betweenCheckerEnumerator = CheckBetweenObstacle();
                    StartCoroutine(betweenCheckerEnumerator);
                }
            }

            yield return WaitTimeManager.WaitForFixedUpdate();
        }

    }


    private bool CanSeePlayer()
    {
        Vector3 dir = playerTransform.position - (transform.position + Vector3.up * 0.1f);
        Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out hit);


        return !hit.collider.isTrigger && hit.collider.CompareTag("Player");
    }


    private void ExitPlayer()
    {
        if (chasingSoundPlayer != null)
            chasingSoundPlayer.Stop();

        isInsidePlayer = false;
        animator.SetBool(hash_chasing, false);
        StopGlitch();
    }

    private void SetGlitch()
    {
        glitch.scanLineJitter = 0.1f;
        glitch.verticalJump = 0.05f;
        glitch.horizontalShake = 0.08f;
        glitch.colorDrift = 0.2f;
    }

    private void StopGlitch()
    {
        glitch.scanLineJitter = 0;
        glitch.verticalJump = 0;
        glitch.horizontalShake = 0;
        glitch.colorDrift = 0;
    }

    private bool CheckIsPlayer(Collider other)
    {
        if (other.CompareTag(GameData.PLAYER_TAG))
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

    protected override IEnumerator MoveStart()
    {
        WaitForSeconds baseTime = WaitTimeManager.WaitForSeconds(0.2f);
        WaitForSeconds applyWaitTime;

        while (true)
        {
            applyWaitTime = baseTime;            
            if (isInsideSongPyeon)
            {
                if (songPyeon == null) isInsideSongPyeon = false;
                else navMesh.SetDestination(songPyeon.transform.position);
            }
            else if (isInsidePlayer)
            {
                //뒤주 들어가면
                if (!playerTransform.gameObject.activeSelf)
                {
                    ExitPlayer();
                }
                else
                {
                    applyWaitTime = null;
                    LookPlayer();
                    navMesh.SetDestination(playerTransform.position);

                    if (CheckKillPlayer())
                    {
                        GameManager.instance.GameOver(0);
                        chasingSoundPlayer.Stop();
                        yield break;
                    }

                }
            }
            else
            {
                yield return WaitTimeManager.WaitForSeconds(1f);
                Patrol();
            }

            yield return applyWaitTime;
        }
    }


    private void Patrol()
    {
        if (navMesh.velocity == Vector3.zero)
        {
            if (patrolPoints.Length < 1 || patrolPoints[patrolCount] == null)
            {
                Debug.LogError("Ghost Patrol Point ���� �����Դϴ�.");
            }

            navMesh.SetDestination(patrolPoints[patrolCount++].position);
            if (patrolCount >= patrolPoints.Length) patrolCount = 0;
        }
    }

    
    private void LookPlayer()
    {
        float dis = (playerTransform.position - transform.position).magnitude;

        if(dis > 2.5f) 
            return;

        Vector3 targetPos = playerTransform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }
    
    private bool CheckKillPlayer()
    {
        bool flag = Physics.Raycast
            (transform.position + Vector3.up, transform.forward, out hit, 1.5f, GameData.PLAYER_LAYER);

        return flag && !hit.collider.isTrigger;
    }

    

}
