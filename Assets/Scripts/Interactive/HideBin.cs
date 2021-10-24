using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBin : InteractiveObject
{
    public Camera hideCamera;
    public Transform forwardTransform;
    public static PlayerController playerController;
    public string hideString;
    public string unhideString;

    private bool isHide;

    private void Start()
    {
        hideCamera.enabled = false;
        isHide = false;
        if (playerController == null)
        {
            playerController = GameManager.instance.playerController;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isHide)
            {
                Unhide();
            }
        }
    }

    public override void Interact()
    {
        isHide = true;
        explainComment = unhideString;
        hideCamera.enabled = true;
        playerController.OffObjectInformation();
        GameManager.instance.hideWindow.SetActive(true);
        playerController.gameObject.transform.position = forwardTransform.position;
        playerController.gameObject.transform.LookAt(gameObject.transform);
        playerController.gameObject.SetActive(false);
    }

    private void Unhide()
    {
        isHide = false;
        explainComment = hideString;
        hideCamera.enabled = false;
        GameManager.instance.hideWindow.SetActive(false);
        playerController.gameObject.SetActive(true);
    }

}
