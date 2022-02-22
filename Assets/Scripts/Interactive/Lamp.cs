using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : InteractiveObject
{
    public Light lampLight;
    public AudioClip lampSound;

    private Color emissionColor;

    void Awake()
    {
        emissionColor = lampLight.color;

        if (lampLight.enabled)
        {
            explainComment = "LB: 끄기";
        }
        else
        {
            explainComment = "LB: 켜기";
        }
    }

    public override void Interact()
    {
        if (lampLight.enabled)
        {
            //킨상태 -> 끈상태
            lampLight.enabled = false;
            explainComment = "LB: 켜기";
            GetComponent<MeshRenderer>().materials[3].SetColor("_EmissionColor", Color.black);
            SFXPlayer.instance.Play(lampSound);
        }
        else
        {   //끈상태 -> 킨상태
            lampLight.enabled = true;
            explainComment = "LB: 끄기";
            GetComponent<MeshRenderer>().materials[3].SetColor("_EmissionColor", emissionColor);
            SFXPlayer.instance.Play(lampSound);
        }
            
    }


}
