using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass_InHand : InHandItem
{
    public static Transform target;
    public static Transform playerTransform;

    public GameObject needlePivot;

    public override bool UseItem()
    {
        return false;
    }

    private void FixedUpdate()
    {
        PointGhost();
    }

    private void PointTarget()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        needlePivot.transform.localRotation = Quaternion.FromToRotation(-playerTransform.forward, dir);
    }

    private void PointGhost()
    {
        

    }

}
