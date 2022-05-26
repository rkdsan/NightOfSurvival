using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : InHandItem
{
    private MeshRenderer protectiveCube;
    private Slot nowSlot;

    private bool isUsed = false;

    private void Start()
    {

        protectiveCube = GameManager.instance.protectiveCube;
    }

    public override bool UseItem()
    {
        if (CanUseDistance() && !isUsed)
        {
            isUsed = true;
            nowSlot = GameManager.instance.inventoryManager.nowSlot;
            StartCoroutine(FadeEffect());
        }
        return false;
    }

    private IEnumerator FadeEffect()
    {
        float max = 40;
        Color startCol = protectiveCube.material.color;
        Color endCol = startCol;
        endCol.a = 0;

        for (int i = 0; i < max; i++) 
        {
            protectiveCube.material.color = Color.Lerp(startCol, endCol, i/max);
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        nowSlot.ConsumeItem();
        Destroy(protectiveCube.gameObject);
        Destroy(gameObject);
    }

    private bool CanUseDistance()
    {
        Vector3 dis = protectiveCube.transform.position - transform.position;
        if(dis.magnitude <= 4)
        {
            return true;
        }

        TutorialManager.instance.ShowTutoWindow("장막 앞에서 사용해주세요.");
        return false;
    }

}
