using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : Item
{
    public GameObject installedTalisman;

    Vector3 shootPos;
    Vector3 shootDir;
    Quaternion rot;
    RaycastHit hit;
    public override bool UseItem()
    {
        shootPos = Camera.main.transform.position;
        shootDir = Camera.main.transform.forward;
        if (Physics.Raycast(shootPos, shootDir, out hit, 2.5f))
        {
            
            rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            //벽이 아니면 false 리턴 하려고
            Vector3 temp = rot.eulerAngles;
            

            Instantiate(installedTalisman, hit.point, rot);
            return true;
        };


        return false;
    }
}
