using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public Image backgroundImage;
    public Text guideText;

    private Color translucentColor;
    private Coroutine showCor;

    private float waitTime;

    private void Awake()
    {
        TutoCommentSender.tutoManager = this;
        translucentColor = new Color(0, 0, 0, 0.2f);
        instance = this;   
    }

    public void ShowTutoWindow(string comment)
    {
        ShowTutoWindow(comment, 2f);
    }

    public void ShowTutoWindow(string comment, float time)
    {
        waitTime = time;
        guideText.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);

        guideText.text = comment.Replace("\\n", "\n");

        if (showCor != null) StopCoroutine(showCor);
        showCor = StartCoroutine(Show());
    }
    public void StopShow()
    {
        if (showCor != null) 
            StopCoroutine(showCor);

        backgroundImage.color = Color.clear;
        guideText.color = Color.clear;
        backgroundImage.gameObject.SetActive(false);
        guideText.gameObject.SetActive(false);
    }

    private IEnumerator Show()
    {
        backgroundImage.DOColor(translucentColor, 1);
        guideText.DOColor(Color.white, 1);

        yield return WaitTimeManager.WaitForSeconds(waitTime);
        backgroundImage.DOColor(Color.clear, 1);
        guideText.DOColor(Color.clear, 1);

        yield return WaitTimeManager.WaitForSeconds(1);
        backgroundImage.gameObject.SetActive(false);
        guideText.gameObject.SetActive(false);
    }


}
