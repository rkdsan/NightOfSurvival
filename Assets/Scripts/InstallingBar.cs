using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstallingBar : MonoBehaviour
{
    public const int MAX_TIME = 2;
    public Image fillImage;

    private float timeSum;

    public InstallingBar() { timeSum = 0; }


    public void UpTime()
    {
        timeSum += Time.deltaTime;
        SetImageFill();
    }

    public void ResetTime()
    {
        timeSum = 0;
        SetImageFill();
    }

    private void SetImageFill()
    {
        fillImage.fillAmount = timeSum / MAX_TIME;
    }
}
