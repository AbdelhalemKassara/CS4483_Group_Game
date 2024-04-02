using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputManager : CameraManager
{
    private InputMaster input = null;
    

    void Awake()
    {
        input = new InputMaster();
    }

    private void OnEnable()
    {
        input.Camera.Enable();
        input.Camera.ChangeCameraMode.performed += OnChangeViewP;
        input.Camera.ChangeCameraMode.canceled += OnChangeViewC;

        input.Camera.CameraUpDown.performed += OnCameraUpDownP;
        input.Camera.CameraUpDown.canceled += OnCameraUpDownC;

        input.Camera.CameraLeftRight.performed += OnCameraLeftRightP;
        input.Camera.CameraLeftRight.canceled += OnCameraLeftRightC;
        
    }

    private void OnDisable()
    {
        input.Camera.Disable();
        input.Camera.ChangeCameraMode.performed -= OnChangeViewP;
        input.Camera.ChangeCameraMode.canceled -= OnChangeViewC;
    }

    private void OnChangeViewP(InputAction.CallbackContext value)
    {
        curCamSetting++;
        curCamSetting %= cameraModeSetings.Count;
    }

    private void OnChangeViewC(InputAction.CallbackContext value) { }

    private void OnCameraUpDownP(InputAction.CallbackContext value)
    {
        upDownCam = value.ReadValue<float>();
    }

    private void OnCameraUpDownC(InputAction.CallbackContext value)
    {
        upDownCam = 0;
    }
    
    private void OnCameraLeftRightP(InputAction.CallbackContext value)
    {
        leftRightCam = value.ReadValue<float>();
    }

    private void OnCameraLeftRightC(InputAction.CallbackContext value)
    {
        leftRightCam = 0;
    }
}
