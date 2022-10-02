using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kino;


public class Ghost : MonoBehaviour
{
    public static List<Ghost> allGhostList;

    public static Transform playerTransform;

    public AudioClip chasingSoundClip;
    public AudioClip neckCrackingSoundClip;
    public AudioSource baseSound;
    public LayerMask playerMask;
    public Transform[] patrolPoints;

    public Animator animator;
    public NavMeshAgent navMesh;

    public float walkSpeed;
    public int deathImageIndex;

    [HideInInspector] public Transform songPyeon;

    protected AudioSource chasingSoundPlayer;
    protected AnalogGlitch glitch;
    protected RaycastHit hit;
    protected IEnumerator betweenCheckerEnumerator;

    protected int hash_chasing;
    protected int hash_stun;
    protected bool _isSlow;

    protected int patrolCount = 0;
    protected bool isInsidePlayer = false;
    protected bool isInsideSongPyeon;
    protected float _returnTime = 5;

    void Awake()
    {
        if (allGhostList == null) 
            allGhostList = new List<Ghost>();

        allGhostList.Add(this);
        SetAniHash();
        StartCoroutine(MoveStart());
    }

    protected virtual void Start()
    {
        glitch = GameManager.instance.glitch;
        _returnTime = 5;
        if (baseSound != null)
        {
            SFXPlayer.instance.managedPlayers.Add(baseSound);
            baseSound.volume = SFXPlayer.instance.GetVolume();
        }
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
        if (CheckIsSongPyeon(other) && songPyeon == null)
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

    protected virtual IEnumerator CheckBetweenObstacle()
    {
        while (true)
        {
            if (CanSeePlayer())
            {
                animator.SetBool(hash_chasing, true);
                SFXPlayer.instance.Play(neckCrackingSoundClip);

                navMesh.speed = 0;

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
                if (cantSeeTime > _returnTime)
                {
                    ExitPlayer();
                    betweenCheckerEnumerator = CheckBetweenObstacle();
                    StartCoroutine(betweenCheckerEnumerator);
                }
            }

            yield return WaitTimeManager.WaitForFixedUpdate();
        }
    }

    protected void StopGlitch()
    {
        glitch.scanLineJitter = 0;
        glitch.verticalJump = 0;
        glitch.horizontalShake = 0;
        glitch.colorDrift = 0;
    }


    protected virtual IEnumerator MoveStart()
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
                        GameManager.instance.GameOver(deathImageIndex);
                        chasingSoundPlayer.Stop();
                        yield break;
                    }
                }
            }
            else
            {
                Patrol();
                yield return WaitTimeManager.WaitForSeconds(1f);
            }

            yield return applyWaitTime;
        }
    }


    protected virtual void Patrol()
    {
        if (navMesh.velocity == Vector3.zero)
        {
            if (patrolPoints.Length < 1 || patrolPoints[patrolCount] == null)
            {
                Debug.LogError("Ghost Patrol Point 오류.");
            }

            navMesh.SetDestination(patrolPoints[patrolCount++].position);
            if (patrolCount >= patrolPoints.Length) patrolCount = 0;
        }
    }


    protected void LookPlayer()
    {
        float dis = (playerTransform.position - transform.position).magnitude;

        if (dis > 2.5f)
            return;

        Vector3 targetPos = playerTransform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }

    protected bool CheckKillPlayer()
    {
        bool flag = Physics.Raycast
            (transform.position + Vector3.up, transform.forward, out hit, 1.5f, GameData.PLAYER_LAYER_MASK);

        return flag && !hit.collider.isTrigger;
    }


    protected bool CheckIsPlayer(Collider other)
    {
        if (other.CompareTag(GameData.PLAYER_TAG))
        {
            return true;
        }
        return false;
    }

    protected bool CheckIsSongPyeon(Collider other)
    {
        SongPyeon_Used temp = other.GetComponent<SongPyeon_Used>();
        if (temp != null)
        {
            return true;
        }
        return false;
    }

    protected void ExitPlayer()
    {
        if (chasingSoundPlayer != null)
            chasingSoundPlayer.Stop();

        isInsidePlayer = false;
        animator.SetBool(hash_chasing, false);
        StopGlitch();
    }

    protected bool CanSeePlayer()
    {
        Vector3 dir = playerTransform.position - (transform.position + Vector3.up * 0.1f);
        Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out hit);


        return !hit.collider.isTrigger && hit.collider.CompareTag(GameData.PLAYER_TAG);
    }

    protected void SetGlitch()
    {
        glitch.scanLineJitter = 0.1f;
        glitch.verticalJump = 0.05f;
        glitch.horizontalShake = 0.08f;
        glitch.colorDrift = 0.2f;
    }

    protected void SetAniHash()
    {
        hash_chasing = Animator.StringToHash("Chasing");
        hash_stun = Animator.StringToHash("Stun");
    }

    public void Stuned(int stunTime)
    {
        StartCoroutine(StunTimer(stunTime));
    }


    protected IEnumerator StunTimer(int stunTime)
    {
        animator.SetBool(hash_stun, true);
        navMesh.speed = 0;
        yield return WaitTimeManager.WaitForSeconds(stunTime);
        SetSpeed();
        animator.SetBool(hash_stun, false);
    }

    public void SetSlow(bool isSlow)
    {
        _isSlow = isSlow;
        SetSpeed();
    }

    protected void SetSpeed()
    {
        float slowSpeed = _isSlow ? 0.5f : 1;
        navMesh.speed = walkSpeed * slowSpeed;
    }

}
