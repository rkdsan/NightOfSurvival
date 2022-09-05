using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Byway : InteractiveObject
{
    private const string FIND_STRING = "샛길을 발견하였습니다. \\n입장하시겠습니까?? \\n( Y / N )";
    public GameObject targetObj;
    public AudioClip openSound;

    //private float checkTime = 0;
    private Coroutine answerCor;

    public override void Interact()
    {
        StartCoroutine(MovePlayer());
        //TutorialManager.instance.ShowTutoWindow(FIND_STRING, 999f);

        //if (answerCor != null) 
        //    StopCoroutine(answerCor);
        //answerCor = StartCoroutine(WaitAnswer());
    }

    private IEnumerator MovePlayer()
    {
        SFXPlayer.instance.Play(openSound);
        GameManager.instance.fadeImage.DOColor(Color.black, 0.5f).SetEase(Ease.InQuart);

        yield return WaitTimeManager.WaitForSeconds(0.5f);

        GameManager.instance.fadeImage.DOColor(Color.clear, 0.5f).SetEase(Ease.InQuart);
        GameManager.instance.player.transform.position = targetObj.transform.position;
        Physics.SyncTransforms();

    }
    //private IEnumerator WaitAnswer()
    //{
    //    checkTime = 0;
    //    while (true)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Y))
    //        {
    //            MovePlayer();
    //            break;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.N))
    //            break;


    //        //0.5초마다 플레이어 범위 체크
    //        checkTime += Time.deltaTime;
    //        if (checkTime >= 0.5f && !IsInPlayer())
    //            break;

    //        yield return null;
    //    }

    //    TutorialManager.instance.StopShow();
    //}

    //private bool IsInPlayer()
    //{
    //    checkTime = 0;
    //    Collider[] colliders
    //            = Physics.OverlapSphere(transform.position, 3, GameData.PLAYER_LAYER_MASK);


    //    return colliders.Length > 0;
    //}
}
