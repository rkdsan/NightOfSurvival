using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : Item
{
    public static InstallingBar installingBar;
    public GameObject installedTalisman;

    private Vector3 shootPos;
    private Vector3 shootDir;
    private Quaternion rot;
    private RaycastHit hit;

    public override bool UseItem()
    {
        StartCoroutine(StartInstall());
        return false;
    }

    IEnumerator StartInstall()
    {
        installingBar.gameObject.SetActive(true);
        while (Input.GetMouseButton(1) && ShootRaycast())
        {
            installingBar.UpTime();
            if(installingBar.fillImage.fillAmount >= 1)
            {
                Install();
                break;
            }
            yield return null;
        }
        installingBar.ResetTime();
        installingBar.gameObject.SetActive(false);
    }

    private void Install()
    {
        if (ShootRaycast())
        {
            rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Instantiate(installedTalisman, hit.point, rot);
        };
    }

    private bool ShootRaycast()
    {
        shootPos = Camera.main.transform.position;
        shootDir = Camera.main.transform.forward;

        return Physics.Raycast(shootPos, shootDir, out hit, 2.5f);
    }

    //public override bool UseItem()
    //{
    //    shootPos = Camera.main.transform.position;
    //    shootDir = Camera.main.transform.forward;
    //    if (Physics.Raycast(shootPos, shootDir, out hit, 2.5f))
    //    {

    //        rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
    //        //벽이 아니면 false 리턴 하려고
    //        Vector3 temp = rot.eulerAngles;


    //        Instantiate(installedTalisman, hit.point, rot);
    //        return true;
    //    };


    //    //레이가 안닿으면
    //    return false;
    //}

}
