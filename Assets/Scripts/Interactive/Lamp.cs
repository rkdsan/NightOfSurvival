using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : InteractiveObject
{
    public Light lampLight;
    public AudioClip lampSound;

    private Color emissionColor;
    private Material emissionMaterial;

    void Awake()
    {
        emissionMaterial = GetComponent<MeshRenderer>().materials[3];
        emissionColor = lampLight.color;
        emissionColor.r = 0.8f;

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
            //Å²»óÅÂ -> ²ö»óÅÂ
            lampLight.enabled = false;
            explainComment = "LB: ÄÑ±â";
            emissionMaterial.SetColor("_EmissionColor", Color.black);
            SFXPlayer.instance.Play(lampSound);
        }
        else
        {   //²ö»óÅÂ -> Å²»óÅÂ
            lampLight.enabled = true;
            explainComment = "LB: ²ô±â";
            emissionMaterial.SetColor("_EmissionColor", emissionColor);
            SFXPlayer.instance.Play(lampSound);
        }
            
    }


}
