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
}
