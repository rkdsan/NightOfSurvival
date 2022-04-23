using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFox : Ghost
{
    protected override void Start()
    {
        base.Start();
        _returnTime = 2;
    }

    protected override void Patrol()
    {
        navMesh.SetDestination(patrolPoints[0].position);
    }

}
