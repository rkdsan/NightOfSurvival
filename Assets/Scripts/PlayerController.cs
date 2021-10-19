using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject informationTextObejct;
    public GameObject hand;
    public Image runGaugeImage;
    public Text objectName;
    public Text explainText;

    public LayerMask rayLayerMask;

    [HideInInspector] public bool onTab;
    [HideInInspector] public InventoryManager inventoryManager;

    public float walkSpeed = 1;
    public float runSpeed = 2;
    private float applySpeed;

    public float lookSensitivity = 1;
    public float camRotateLimit = 80;

    private CharacterController controller;
    private InteractiveObject hitInteractiveObj;
    private Camera playerCam;
    private RaycastHit hit;

    private Vector3 moveHorizontal;
    private Vector3 moveVertical;
    private Vector3 moveDir;
    private Vector3 camApplyRotate;
    private Vector3 playerRotate;

    private float runGaugeAddValue;
    private float runGaugeUsevalue;
    private bool isInteractiveObj;
    private bool isRunning;

    void Awake()
    {
        playerCam = Camera.main;
        controller = GetComponent<CharacterController>();

        camApplyRotate = playerCam.transform.localEulerAngles;
        playerRotate = transform.localEulerAngles;
        runGaugeAddValue = 0.005f;
        runGaugeUsevalue = runGaugeAddValue * 5;
        applySpeed = walkSpeed;

        isInteractiveObj = false;
        isRunning = false;
        onTab = false;

        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        ManageMove();
        if (!onTab)
        {
            ShootRaycast();
            CheckInteracteObject();
            CheckUseItem();
        }
        controller.SimpleMove(Vector3.down * Time.deltaTime);
    }


    #region 이동관련
    private void ManageMove()
    {
        CheckRun();
        MovePos();
        if (onTab) return;
        RotateCamera();
        RotatePlayer();
    }

    public void CheckRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && runGaugeImage.fillAmount > 0.2f)
        {
            StartCoroutine(StartRun());
        }
    }

    IEnumerator StartRun()
    {
        isRunning = true;
        applySpeed = runSpeed;
        while (Input.GetKey(KeyCode.LeftShift) && runGaugeImage.fillAmount > runGaugeUsevalue)
        {
            runGaugeImage.fillAmount -= runGaugeUsevalue;
            yield return new WaitForFixedUpdate();

        } 

        isRunning = false;
        applySpeed = walkSpeed;
        StartCoroutine(UpRunGauge());
        
    }

    IEnumerator UpRunGauge()
    {
        while (!isRunning && runGaugeImage.fillAmount < 1)
        {
            runGaugeImage.fillAmount += runGaugeAddValue;
            yield return new WaitForFixedUpdate();
        }
    }

    private void MovePos()
    {
        moveHorizontal = transform.right * Input.GetAxisRaw("Horizontal");
        moveVertical = transform.forward * Input.GetAxisRaw("Vertical");

        moveDir = (moveHorizontal + moveVertical).normalized * applySpeed;

        controller.Move(moveDir * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        float addRotY = Input.GetAxisRaw("Mouse X") * lookSensitivity;

        playerRotate.y += addRotY;

        transform.localEulerAngles = playerRotate;
    }

    private void RotateCamera()
    {
        float addRotX = Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        camApplyRotate.x -= addRotX;
        camApplyRotate.x = Mathf.Clamp(camApplyRotate.x, -camRotateLimit, camRotateLimit);

        playerCam.transform.localEulerAngles = camApplyRotate;
    }
    #endregion

    #region 아이템상호작용
    public void ShootRaycast()
    {
        isInteractiveObj = false;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 2.5f, rayLayerMask))
        {
            if(hit.collider.CompareTag("InteractiveObject"))
            {
                OnObjectInformation();
            }
        }

        if (!isInteractiveObj)
        {
            OffObjectInformation();
        }
    }

    public void OnObjectInformation()
    {
        isInteractiveObj = true;
        hitInteractiveObj = hit.transform.GetComponent<InteractiveObject>();
        objectName.text = hitInteractiveObj.objectName;
        explainText.text = hitInteractiveObj.explainComment;
        informationTextObejct.SetActive(true);
    }

    public void OffObjectInformation()
    {
        informationTextObejct.SetActive(false);
    }

    public void CheckInteracteObject()
    {
        //오브젝트 상호작용
        if (Input.GetMouseButtonDown(0))
        {
            if (isInteractiveObj)
            {
                hitInteractiveObj.Interact();
            }
        }
    }

    public void CheckUseItem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            inventoryManager.UseNowItem();
        }
    }

    #endregion

}
