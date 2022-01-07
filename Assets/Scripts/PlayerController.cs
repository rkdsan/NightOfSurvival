using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject informationTextObejct;
    public GameObject hand;
    public AudioSource walkSound;
    public Image runGaugeImage;
    public Text objectName;
    public Text explainText;
    

    public LayerMask rayLayerMask;

    [HideInInspector] public bool onTab;
    [HideInInspector] public bool canMove;
    [HideInInspector] public InventoryManager inventoryManager;

    public float walkSpeed = 1;
    public float runSpeed = 2;
    private float applySpeed;

    public float lookSensitivity;
    public float camRotateLimit = 80;

    private const float SPEED_STANDARD = 0.02f;
    private const float WALK_CAMERA_MOVE_RANGE = 0.03f;

    private CharacterController controller;
    private InteractiveObject hitInteractiveObj;
    private Camera playerCam;
    private RaycastHit hit;

    private Vector3 moveHorizontal;
    private Vector3 moveVertical;
    private Vector3 moveDir;
    private Vector3 camApplyRotate;
    private Vector3 playerRotate;
    private Vector3 cameraLocalPos;
    private Vector3 cameraLocalOriginPos;
    private Color runGaugeColor;

    private int cameraLocalPosFlag;
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
        cameraLocalPos = playerCam.transform.localPosition;
        cameraLocalOriginPos = cameraLocalPos;
        runGaugeAddValue = 0.002f;
        runGaugeUsevalue = runGaugeAddValue * 5;
        applySpeed = walkSpeed;

        runGaugeColor = Color.white;

        isInteractiveObj = false;
        isRunning = false;
        canMove = true;
        onTab = false;
        cameraLocalPosFlag = 1;

        ShadowGhost.playerController = this;
    }

    void FixedUpdate()
    {
        if(canMove) MovePos();
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

    private void MovePos()
    {
        moveHorizontal = transform.right * Input.GetAxisRaw("Horizontal");
        moveVertical = transform.forward * Input.GetAxisRaw("Vertical");

        moveDir = (moveHorizontal + moveVertical).normalized * applySpeed * SPEED_STANDARD;

        controller.Move(moveDir);

        SetWalkSound();
        //SetCameraLocalPos();
    }

    private void SetCameraLocalPos()
    {
        if (moveDir == Vector3.zero) 
        { 
            //return;
        }

        float gap = Mathf.Abs(cameraLocalPos.y - cameraLocalOriginPos.y);

        if(gap > WALK_CAMERA_MOVE_RANGE)
        {
            cameraLocalPosFlag *= -1;

        }

        float mulValue = 1 + gap * 100;
        float temp = WALK_CAMERA_MOVE_RANGE * 0.1f * cameraLocalPosFlag * mulValue;
        Debug.Log(temp);

        cameraLocalPos.y += WALK_CAMERA_MOVE_RANGE * 0.1f * cameraLocalPosFlag * mulValue;
        playerCam.transform.localPosition = cameraLocalPos;

    }

    #region 이동관련
    private void ManageMove()
    {
        CheckRun();
        if (onTab) return;
        RotateCamera();
        RotatePlayer();
    }

   
    public void CheckRun()
    {
        //W키 누른 상태에서 달릴때만, 게이지가 20% 이상 차있어야함
        if (Input.GetKeyDown(KeyCode.LeftShift)
            && Input.GetKey(KeyCode.W)
            && runGaugeImage.fillAmount > 0.2f)
        {
            StartCoroutine(StartRun());
        }
    }

    private IEnumerator StartRun()
    {
        isRunning = true;
        applySpeed = runSpeed;
        //walkSound.pitch = 1.5f;
        runGaugeColor.a = 1;
        while (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) 
            && runGaugeImage.fillAmount > runGaugeUsevalue)
        {
            runGaugeImage.fillAmount -= runGaugeUsevalue;
            UpdateRunGaugeColor();
            yield return WaitTimeManager.WaitForFixedUpdate();

        }
        isRunning = false;
        applySpeed = walkSpeed;
        //walkSound.pitch = 1.2f;
        StartCoroutine(UpRunGauge());

    }
    private IEnumerator UpRunGauge()
    {
        runGaugeColor.a = 0.3f;

        while (!isRunning && runGaugeImage.fillAmount < 1)
        {
            runGaugeImage.fillAmount += runGaugeAddValue;
            UpdateRunGaugeColor();
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        if (!isRunning)
        {
            
            UpdateRunGaugeColor();
        }
    }
    private void UpdateRunGaugeColor()
    {
        float amout = runGaugeImage.fillAmount;
        runGaugeColor.g = runGaugeColor.b = amout;

        runGaugeImage.color = runGaugeColor;
    }

    private void SetWalkSound()
    {
        if (moveDir == Vector3.zero)
        {
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
        }
        else if(!walkSound.isPlaying)
        {
            walkSound.Play();
        }
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
