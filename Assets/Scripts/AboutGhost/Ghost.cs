using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kino;


public class Ghost : MonoBehaviour
{
    public static List<Ghost> allGhostList;

    public static Transform playerTransform;


    public Animator animator;
    public NavMeshAgent navMesh;

    public float walkSpeed;


    protected int hash_chasing;
    protected int hash_stun;
    protected bool _isSlow;

    void Awake()
    {
        if (allGhostList == null) 
            allGhostList = new List<Ghost>();

        allGhostList.Add(this);
        SetAniHash();
        StartCoroutine(MoveStart());
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

    protected virtual IEnumerator MoveStart() 
    {
        //정의해서 사용 할 것
        yield return null;
    }

    private IEnumerator StunTimer(int stunTime)
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
