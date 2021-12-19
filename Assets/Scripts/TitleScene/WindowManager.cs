using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
   
    void Update()
    {
        CheckCloseTab();
    }

    protected void CheckCloseTab()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseThisTab();
        }
    }

    public void CloseThisTab()
    {
        gameObject.SetActive(false);
    }

}
