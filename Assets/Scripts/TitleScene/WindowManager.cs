using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
   
    void Update()
    {
        CloseTab();
    }

    private bool CheckKey_ESC()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private void CloseTab()
    {
        if (CheckKey_ESC())
        {
            gameObject.SetActive(false);
        }
    }

}
