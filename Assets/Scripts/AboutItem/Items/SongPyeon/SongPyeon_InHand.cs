using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPyeon_InHand : InHandItem
{
    public GameObject songPyeon_Used;

    public override bool UseItem()
    {
        Vector3 forward = transform.parent.forward;
        Instantiate(songPyeon_Used, transform.position + forward, transform.rotation
            , transform.parent);

        EffectSoundManager.instance.effectSound_pickUp.Play();
        return true;
    }
}
