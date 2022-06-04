using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public List<GameObject> images;

    public bool isLast = false;
    private int imageIndex = 0;

    private void OnEnable()
    {
        imageIndex = 0;
        foreach(var img in images)
        {
            img.SetActive(false);
        }
    }

    public void TurnOnNextCut()
    {
        images[imageIndex++].SetActive(true);

        isLast = imageIndex == images.Count;
    }
}

