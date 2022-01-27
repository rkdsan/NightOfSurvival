using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : InteractiveObject
{
    public Light lampLight;
    public AudioClip lampSound;

    void Awake()
    {
        if (lampLight.enabled)
        {
            explainComment = "LB: ²ô±â";
        }
        else
        {
            explainComment = "LB: ÄÑ±â";
        }
    }

    public override void Interact()
    {
        if (lampLight.enabled)
        {
            lampLight.enabled = false;
            explainComment = "LB: ÄÑ±â";
            SFXPlayer.instance.Play(lampSound);
        }
        else
        {
            lampLight.enabled = true;
            explainComment = "LB: ²ô±â";
            SFXPlayer.instance.Play(lampSound);
        }
            
    }


}
