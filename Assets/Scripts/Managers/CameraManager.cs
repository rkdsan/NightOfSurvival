using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public Camera subCamera;

    void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
    }

    public void SetCamera(Camera _subCamera)
    {
        subCamera = _subCamera;
        subCamera.enabled = true;
        mainCamera.enabled = false;
    }

    public void SetPlayerCamera()
    {
        mainCamera.enabled = true;
        subCamera.enabled = false;
    }

}
