using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject hand;

    [Header("Sound Clip")]
    public AudioClip walkSound;
    public AudioClip runSound;

    [Header("UI")]
    public GameObject informationTextObejct;
    public Image runGaugeImage;
    public Text objectName;
    public Text explainText;
    
    [Header("etc")]
    public LayerMask rayLayerMask;
    public float lookSensitivity;

    [HideInInspector] public bool onTab = false;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public InventoryManager inventoryManager;


    private const float WALK_SPEED = 4;
    private const float RUN_SPEED = 8;
    private const float SPEED_STANDARD = 0.02f;
    private const float CAM_ROTATE_LIMIT = 80;
    private const float RUN_GAUGE_ADD_VALUE = 0.002f;
    private const float RUN_GAUGE_USE_VALUE = 0.004f;

    private AudioSource moveSoundPlayer;
    private InteractiveObject hitInteractiveObj;
    private Camera playerCam;
    private RaycastHit hit;

    private Vector3 moveHorizontal;
    private Vector3 moveVertical;
    private Vector3 moveDir;
    private Vector3 camRotate;
    private Vector3 playerRotate;
    private Color runGaugeColor;

    
    private float applySpeed = WALK_SPEED;
    private bool isInteractiveObj = false;
    private bool isRunning = true;

    private Vector3 temp;

    void Awake()
    {
        playerCam = Camera.main;
        controller = GetComponent<CharacterController>();

        camRotate = playerCam.transform.localEulerAngles;
        playerRotate = transform.localEulerAngles;
        runGaugeColor = Color.white;

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
        applySpeed = RUN_SPEED;
        moveSoundPlayer.clip = runSound;
        runGaugeColor.a = 1;

        while (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) 
            && runGaugeImage.fillAmount > RUN_GAUGE_USE_VALUE)
        {
            runGaugeImage.fillAmount -= RUN_GAUGE_USE_VALUE;
            UpdateRunGaugeColor();
            yield return WaitTimeManager.WaitForFixedUpdate();

        }
        
        StartCoroutine(UpRunGauge());

    }
    public IEnumerator UpRunGauge()
    {
        isRunning = false;
        applySpeed = WALK_SPEED;
        if (moveSoundPlayer != null) moveSoundPlayer.clip = walkSound;
        runGaugeColor.a = 0.3f;

        while (!isRunning && runGaugeImage.fillAmount < 1)
        {
            runGaugeImage.fillAmount += RUN_GAUGE_ADD_VALUE;
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
            ReturnMoveSoundPlayer();
        }
        else if(moveSoundPlayer == null)
        {
            moveSoundPlayer = SFXPlayer.instance.Play(walkSound);
            moveSoundPlayer.loop = true;
        }
        else if(!moveSoundPlayer.isPlaying)
        {
            moveSoundPlayer.Play();
        }
    }

    public void ReturnMoveSoundPlayer()
    {
        if (moveSoundPlayer != null && moveSoundPlayer.isPlaying)
        {
            moveSoundPlayer.Stop();
            moveSoundPlayer.loop = false;
            moveSoundPlayer = null;
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

        camRotate.x -= addRotX;
        camRotate.x = Mathf.Clamp(camRotate.x, -CAM_ROTATE_LIMIT, CAM_ROTATE_LIMIT);

        playerCam.transform.localEulerAngles = camRotate;

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


    public void SetRotateWhenOutHideBin(Vector3 rotate)
    {
        rotate.x = 0;
        rotate.z = 0;
        rotate.y += 180;

        playerRotate = rotate;
    }

}
