using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstallingBar : MonoBehaviour
{
    public const int MAX_TIME = 2;
    public Image fillImage;

    private float timeSum;
    private float fillAmout;

    public InstallingBar() 
    { 
        timeSum = 0; 
    }


    public float UpTime()
    {
        timeSum += Time.deltaTime;
        SetImageFill();
        return fillAmout;
    }

    public void ResetTime()
    {
        timeSum = 0;
        SetImageFill();
    }

    private void SetImageFill()
    {
        fillAmout = timeSum / MAX_TIME;
        fillImage.fillAmount = fillAmout;
        
    }
}
