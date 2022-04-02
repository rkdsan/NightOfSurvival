using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kino;


public class Ghost : MonoBehaviour
{
    public static List<Ghost> allGhostList = new List<Ghost>();
    public static Transform playerTransform;
    public AudioClip chasingSoundClip;
    public LayerMask playerMask;
    public Animator animator;
    public Transform[] patrolPoints;
    public float walkSpeed;

    [HideInInspector] public Transform songPyeon;

    private AudioSource chasingSoundPlayer;
    private NavMeshAgent navMesh;
    private AnalogGlitch glitch;
    private RaycastHit hit;
    private IEnumerator betweenCheckerEnumerator;

    private int patrolCount;
    private int hash_chasing;
    private int hash_stun;
    private bool isInsidePlayer;
    private bool isInsideSongPyeon;

    private float beforeSpeed;
    private bool _isSlow;


    void Awake()
    {
        allGhostList.Add(this);
        navMesh = GetComponent<NavMeshAgent>();
        patrolCount = 0;
        isInsidePlayer = false;
        SetAniHash();
        StartCoroutine(MoveStart());
    }

    private void Start()
    {
        glitch = GameManager.instance.glitch;
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


    //추적을 시작할 때 사이에 벽이 있는지 체크
    private IEnumerator CheckBetweenObstacle()
    {
        while (true)
        {
            if (CanSeePlayer())
            {
                animator.SetBool(hash_chasing, true);

                navMesh.speed = 0;

                //변경된 애니메이션으로 체크하기위해 1프레임 건너뜀
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
    }


    private bool CanSeePlayer()
    {
        Vector3 dir = playerTransform.position - (transform.position + Vector3.up * 0.1f);
        Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out hit);

        
        return !hit.collider.isTrigger && hit.collider.CompareTag("Player");
    }


    private void ExitPlayer()
    {
        //플레이어가 벽을끼고 있다면 null일수있음
        if(chasingSoundPlayer != null)
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

    private IEnumerator MoveStart()
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
                //뒤주에 숨었을때
                if (!playerTransform.gameObject.activeSelf) ExitPlayer();

                navMesh.SetDestination(playerTransform.position);

                if (CheckKillPlayer())
                {
                    GameManager.instance.GameOver(0);
                    chasingSoundPlayer.Stop();
                    yield break;
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
            if(patrolPoints.Length < 1 || patrolPoints[patrolCount] == null)
            {
                Debug.LogError("Ghost Patrol Point 설정 오류입니다.");
            }

            navMesh.SetDestination(patrolPoints[patrolCount++].position);
            if (patrolCount >= patrolPoints.Length) patrolCount = 0;
        }
    }

    public void Stuned(int stunTime)
    {
        StartCoroutine(StunTimer(stunTime));
    }

    private IEnumerator StunTimer(int stunTime)
    {
        animator.SetBool(hash_stun, true);
        navMesh.speed = 0;
        yield return WaitTimeManager.WaitForSeconds(stunTime);
        SetSpeed();
        animator.SetBool(hash_stun, false);
    }

    private bool CheckKillPlayer()
    {
        bool temp;
        temp = Physics.Raycast
            (transform.position + Vector3.up, transform.forward, out hit, 1, GameData.PLAYER_LAYER);

        return temp && !hit.collider.isTrigger;
    }

    public void SetSlow(bool isSlow)
    {
        _isSlow = isSlow;
        SetSpeed();
    }

    private void SetSpeed()
    {
        float slowSpeed = _isSlow ? 0.5f : 1;
        navMesh.speed = walkSpeed * slowSpeed;
    }
}
