using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman_InHand : InHandItem
{
    public static InstallingBar installingBar;
    public GameObject talisman_Used;
    public LayerMask layermask;

    private Vector3 shootPos;
    private Vector3 shootDir;
    private Quaternion rot;
    private RaycastHit hit;

    public override bool UseItem()
    {
        StartCoroutine(StartInstall());
        return false;
    }


    #region 설치
    private IEnumerator StartInstall()
    {
        installingBar.gameObject.SetActive(true);
        while (Input.GetMouseButton(1) && ShootRaycast())
        {
            //fillAmout가 반환됨
            if (installingBar.UpTime() >= 1)
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
            Instantiate(talisman_Used, hit.point, rot);

            GameManager.instance.inventoryManager.nowSlot.ConsumeItem();

            EffectSoundManager.instance.effectSound_pickUp.Play();
        };
    }

    private bool ShootRaycast()
    {
        shootPos = Camera.main.transform.position;
        shootDir = Camera.main.transform.forward;

        return Physics.Raycast(shootPos, shootDir, out hit, 2.5f, layermask);
    }

    #endregion
}
