using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public const float CAM_ROTATE_LIMIT = 80;

    public CharacterController controller;
    public GameObject informationTextObejct;
    public GameObject hand;
    public AudioClip walkSound;
    public AudioClip runSound;
    public Image runGaugeImage;
    public Text objectName;
    public Text explainText;
    

    public LayerMask rayLayerMask;

    [HideInInspector] public bool onTab;
    [HideInInspector] public bool canMove;
    [HideInInspector] public InventoryManager inventoryManager;

    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float lookSensitivity;

    private float applySpeed;

    private const float SPEED_STANDARD = 0.02f;

    private AudioSource moveSoundPlayer;
    private InteractiveObject hitInteractiveObj;
    private Camera playerCam;
    private RaycastHit hit;

    private Vector3 moveHorizontal;
    private Vector3 moveVertical;
    private Vector3 moveDir;
    private Vector3 camApplyRotate;
    private Vector3 playerRotate;
    private Color runGaugeColor;

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
        runGaugeAddValue = 0.002f;
        runGaugeUsevalue = runGaugeAddValue * 5;
        applySpeed = walkSpeed;

        runGaugeColor = Color.white;

        isInteractiveObj = false;
        isRunning = false;
        canMove = true;
        onTab = false;

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
        moveSoundPlayer.clip = runSound;
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
        moveSoundPlayer.clip = walkSound;
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
            if (moveSoundPlayer != null && moveSoundPlayer.isPlaying)
            {
                moveSoundPlayer.Stop();
                moveSoundPlayer = null;
            }
        }
        else if(moveSoundPlayer == null || 
            (moveSoundPlayer != null && !moveSoundPlayer.isPlaying))
        {
            moveSoundPlayer = SFXPlayer.instance.Play(walkSound);
            
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
        camApplyRotate.x = Mathf.Clamp(camApplyRotate.x, -CAM_ROTATE_LIMIT, CAM_ROTATE_LIMIT);

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
