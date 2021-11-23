using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : InteractiveObject
{
    public Light lampLight;

    void Awake()
    {
        explainComment = "LB: ²ô±â";
    }

    public override void Interact()
    {
        if (lampLight.enabled)
        {
            lampLight.enabled = false;
            explainComment = "LB: ÄÑ±â";
        }
        else
        {
            lampLight.enabled = true;
            explainComment = "LB: ²ô±â";
        }
            
    }


}
