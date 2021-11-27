using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    public Text frameText;

    private void Awake()
    {
        StartCoroutine(CountFrame());
    }

    IEnumerator CountFrame()
    {
        while (true)
        {
            frameText.text = "" + (int)(1 / Time.deltaTime);
            yield return WaitTimeManager.WaitForSeconds(1);
        }
    }

}
