using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLamp : InteractiveObject
{
    public Light lampLight;
    private bool isSave = false;
    private Material emissionMaterial;

    private void Awake()
    {
        emissionMaterial = GetComponent<MeshRenderer>().materials[3];
        emissionMaterial.SetColor("_EmissionColor", lampLight.color);
        lampLight.enabled = false;
    }

    public override void Interact()
    {
        if (!isSave)
        {
            Debug.Log("데이터를 저장합니다");
            isSave = true;
            lampLight.enabled = isSave;
            gameObject.tag = GameData.UNTAGGED_TAG;
            SaveManager.instance.Save();
            TutorialManager.instance.ShowTutoWindow("저장되었습니다.");
        }
    }

}
