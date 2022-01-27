using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBin : InteractiveObject
{
    public static PlayerController playerController;
    public const string hideString = "LB: 숨기";
    public const string unhideString = "LB: 나가기";

    public GameObject hideCamera;
    public Transform forwardTransform;
    public AudioClip hideSound;

    private void Reset()
    {
        objectName = "뒤주";
        explainComment = hideString;
    }

    private void Start()
    {
        
        if (playerController == null)
        {
            playerController = GameManager.instance.playerController;
        }
    }

    private IEnumerator CheckUnHide()
    {
        //좌클릭 후에 실행되어서 바로 UnHide됨. 1프레임 건너뛰기
        yield return null; 

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Unhide();
                break;
            }

            yield return null;
        }
    }

    public override void Interact()
    {
        explainComment = unhideString;
        hideCamera.SetActive(true);
        playerController.OffObjectInformation();
        GameManager.instance.hideWindow.SetActive(true);
        SFXPlayer.instance.Play(hideSound);

        //뒤주에서 나왔을때 뒤주를 보고 서있도록
        playerController.gameObject.transform.position = forwardTransform.position;
        playerController.gameObject.SetActive(false);

        StartCoroutine(CheckUnHide());
    }

    private void Unhide()
    {
        explainComment = hideString;
        hideCamera.SetActive(false);
        GameManager.instance.hideWindow.SetActive(false);

        SetPlayerLook();
        playerController.gameObject.SetActive(true);
    }

    private void SetPlayerLook()
    {
        playerController.gameObject.transform.LookAt(gameObject.transform);
        Vector3 rot = playerController.transform.eulerAngles;

        playerController.SetRotateWhenOutHideBin(rot);
    }

}
