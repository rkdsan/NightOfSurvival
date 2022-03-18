using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLamp : InteractiveObject
{
    private bool isSave = false;


    public override void Interact()
    {
        if (!isSave)
        {
            Debug.Log("데이터를 저장합니다");
            isSave = true;
            SaveManager.instance.Save();
            TutorialManager.instance.ShowTutoWindow("저장되었습니다.");
        }
    }

}
