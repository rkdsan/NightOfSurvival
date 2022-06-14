using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CutScene : MonoBehaviour
{
    public List<Image> images;

    [HideInInspector] public bool isLast = false;
    private int imageIndex = 0;
    private bool isChanging = false;

    private void OnEnable()
    {
        imageIndex = 0;
        isLast = false;
        foreach(var img in images)
        {
            img.color = Color.clear;
        }
    }

    public void TurnOnNextCut()
    {
        if (!isChanging)
        {
            isChanging = true;
            images[imageIndex++].DOColor(Color.white, 0.5f).OnComplete(() => isChanging = false);
        }


        isLast = imageIndex == images.Count;
    }

}

