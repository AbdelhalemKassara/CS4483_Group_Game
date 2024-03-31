using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputController : CarController
{
    private InputMaster input = null;

    public DashboardManager dashboardManager;
    public HeadLightsManager headLightsManager;
    
    void Awake()
    {
        input = new InputMaster();
    }

    void Update()
    {
        if (WheelColliders.frontLeft.brakeTorque > 0 || WheelColliders.frontRight.brakeTorque > 0 ||
            WheelColliders.rearRight.brakeTorque > 0 || WheelColliders.rearLeft.brakeTorque > 0)
        {
            headLightsManager.turnOnHeadlights();
        }
        else
        {
            headLightsManager.turnOffHeadlights();
        }
        
        dashboardManager.setRpm(Math.Abs(Rpm));   
        dashboardManager.setSpeed(Speed);
    }
    
    //called when gameobject is enabled
    private void OnEnable()
    {
        input.Car.Enable();
        
        input.Car.Throttle.performed += OnThrottleP;
        input.Car.Throttle.canceled += OnThrottleC;
        
        input.Car.DownShift.performed += OnDownShiftP;
        input.Car.DownShift.canceled += OnDownShiftC;
        
        input.Car.UpShift.performed += OnUpShiftP;
        input.Car.UpShift.canceled += OnUpShiftC;

        input.Car.Steering.performed += OnSteeringP;
        input.Car.Steering.canceled += OnSteeringC;

        input.Car.Handbrake.performed += OnHandbrakeP;
        input.Car.Handbrake.canceled += OnHandbrakeC;
        
        input.Car.Brake.performed += OnBrakeP;
        input.Car.Brake.canceled += OnBrakeC;
    }
    
    //called when gameobject is disabled
    private void OnDisable()
    {   
        input.Car.Disable();
        
        input.Car.Throttle.performed -= OnThrottleP;
        input.Car.Throttle.canceled -= OnThrottleC;
        
        input.Car.DownShift.performed -= OnDownShiftP;
        input.Car.DownShift.canceled -= OnDownShiftC;

        input.Car.UpShift.performed -= OnUpShiftP;
        input.Car.UpShift.canceled -= OnUpShiftC;
        
        input.Car.Steering.performed -= OnSteeringP;
        input.Car.Steering.canceled -= OnSteeringC;
        
        input.Car.Handbrake.performed -= OnHandbrakeP;
        input.Car.Handbrake.canceled -= OnHandbrakeC;

        input.Car.Brake.performed -= OnBrakeP;
        input.Car.Brake.canceled -= OnBrakeC;
    }

    private void OnThrottleP(InputAction.CallbackContext value)
    {
        ThrottleInput = value.ReadValue<float>();
    }
    private void OnThrottleC(InputAction.CallbackContext value)
    {
        ThrottleInput = 0;
    }

    private void OnBrakeP(InputAction.CallbackContext value)
    {
        BrakeInput = value.ReadValue<float>();
    }

    private void OnBrakeC(InputAction.CallbackContext value)
    {
        BrakeInput = 0;
    }

    private void OnDownShiftP(InputAction.CallbackContext value)
    {
        //if(value.ReadValue<bool>()) {}
        if (CurGear > 0)
        {
            CurGear--;
        }

        dashboardManager.setGear(CurGear);
    }
    private void OnDownShiftC(InputAction.CallbackContext value) { }

    private void OnUpShiftP(InputAction.CallbackContext value)
    {
        if (CurGear < GearRatio.Length - 1)
        {
            CurGear++;
        }
        
        dashboardManager.setGear(CurGear);
    }

    private void OnUpShiftC(InputAction.CallbackContext value) { }

    private void OnSteeringP(InputAction.CallbackContext value)
    {
        SteeringInput = value.ReadValue<float>();
    }

    private void OnSteeringC(InputAction.CallbackContext value)
    {
        SteeringInput = 0;
    }

    private void OnHandbrakeP(InputAction.CallbackContext value)
    {
        Handbrake = true;
    }

    private void OnHandbrakeC(InputAction.CallbackContext value)
    {
        Handbrake = false;
    }
    
}
