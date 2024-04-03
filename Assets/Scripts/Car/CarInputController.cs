using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Car;

public class  CarInputController : CarController
{
    private InputMaster input = null;
    private int curGearToggle = -1;
    private float timeout = 0.0f;
    [SerializeField] private float AutoTransTimeoutDuration = 0.5f;
    private float SteeringJoystickVal = 0.0f;
    

    private bool AutoClutchToggle = false;
    void Awake()
    {
        input = new InputMaster();
        
    }

    void Update()
    {
        float returnAmount = 1f;

        if (SteeringInput < -0.01f)
        {
            SteeringInput = Math.Clamp(SteeringInput + returnAmount * Time.deltaTime, -1f, 0f);
        }
        else if (SteeringInput > 0.01f)
        {
            SteeringInput = Math.Clamp(SteeringInput - returnAmount * Time.deltaTime, 0f, 1f);
        }

        SteeringInput = Math.Clamp(SteeringInput + 2.5f * SteeringJoystickVal * Time.deltaTime,
            -Math.Abs(SteeringJoystickVal), Math.Abs(SteeringJoystickVal));



        if (timeout < AutoTransTimeoutDuration)
        {
            timeout += Time.deltaTime;
        }

        if (!enableAutoTransmission)
        {
            if (curGearToggle == -1)
            {
                //this probably is redundant
                input.Car.Brake.performed -= OnThrottleP;
                input.Car.Throttle.performed -= OnBrakeP;

                input.Car.Throttle.performed += OnThrottleP;
                input.Car.Brake.performed += OnBrakeP;
            }
            else if (curGearToggle == 0)
            {
                input.Car.Throttle.performed -= OnBrakeP;
                input.Car.Brake.performed -= OnThrottleP;

                input.Car.Brake.performed += OnBrakeP;
                input.Car.Throttle.performed += OnThrottleP;

            }
            else if (curGearToggle > 0)
            {
                input.Car.Brake.performed -= OnThrottleP;
                input.Car.Throttle.performed -= OnBrakeP;

                input.Car.Throttle.performed += OnThrottleP;
                input.Car.Brake.performed += OnBrakeP;
            }

            return;
        }

        if (curGearToggle != CurGear && CurGear <= 1 && timeout > AutoTransTimeoutDuration)
        {
            curGearToggle = CurGear;
            timeout = 0.0f;
            ThrottleInput = 0f;
            BrakeInput = 0f;

            if (CurGear == 0)
            {
                input.Car.Brake.performed -= OnBrakeP;
                input.Car.Throttle.performed -= OnThrottleP;

                input.Car.Throttle.performed += OnBrakeP;
                input.Car.Brake.performed += OnThrottleP;
            }
            else
            {
                input.Car.Brake.performed -= OnThrottleP;
                input.Car.Throttle.performed -= OnBrakeP;

                input.Car.Throttle.performed += OnThrottleP;
                input.Car.Brake.performed += OnBrakeP;
            }
        }
        
        if (!CarSelected.enableAutoTransmission)
        {
            setEnableAutoTransmission(false);
            
        }
        else
        {
            setEnableAutoTransmission(true);
        }
        

        if (AutoClutchToggle != enableAutoClutch)
        {
            AutoClutchToggle = enableAutoClutch;
            if (AutoClutchToggle)
            {
                input.Car.Clutch.performed -= OnClutchP;
                input.Car.Clutch.canceled -= OnClutchC;
            }
            else
            {
                input.Car.Clutch.performed += OnClutchP;
                input.Car.Clutch.canceled += OnClutchC;
            }
        }
    }


    //called when gameobject is enabled
    private void OnEnable()
    {
        input.Car.Enable();
        
        // input.Car.Throttle.performed += OnThrottleP;
        input.Car.Throttle.canceled += OnThrottleC;
        
        input.Car.DownShift.performed += OnDownShiftP;
        input.Car.DownShift.canceled += OnDownShiftC;
        
        input.Car.UpShift.performed += OnUpShiftP;
        input.Car.UpShift.canceled += OnUpShiftC;

        input.Car.Steering.performed += OnSteeringP;
        input.Car.Steering.canceled += OnSteeringC;

        input.Car.Handbrake.performed += OnHandbrakeP;
        input.Car.Handbrake.canceled += OnHandbrakeC;
        
        // input.Car.Brake.performed += OnBrakeP;
        input.Car.Brake.canceled += OnBrakeC;
        
        input.Car.Clutch.performed += OnClutchP;
        input.Car.Clutch.canceled += OnClutchC;

    }
    
    

    
    //called when gameobject is disabled
    private void OnDisable()
    {   
        input.Car.Disable();

        if (curGearToggle == 0)
        {
            input.Car.Brake.performed -= OnThrottleP;
            input.Car.Throttle.performed -= OnBrakeP;
        }
        else
        {
            input.Car.Throttle.performed -= OnThrottleP;
            input.Car.Brake.performed -= OnBrakeP;
        }

        
        input.Car.Throttle.canceled -= OnThrottleC;
        
        input.Car.DownShift.performed -= OnDownShiftP;
        input.Car.DownShift.canceled -= OnDownShiftC;

        input.Car.UpShift.performed -= OnUpShiftP;
        input.Car.UpShift.canceled -= OnUpShiftC;
        
        input.Car.Steering.performed -= OnSteeringP;
        input.Car.Steering.canceled -= OnSteeringC;
        
        input.Car.Handbrake.performed -= OnHandbrakeP;
        input.Car.Handbrake.canceled -= OnHandbrakeC;

        input.Car.Brake.canceled -= OnBrakeC;

        if (!enableAutoClutch)
        {
            input.Car.Clutch.performed -= OnClutchP;
            input.Car.Clutch.canceled -= OnClutchC;

        }
    }

    private void OnThrottleP(InputAction.CallbackContext value)
    {
        ThrottleInput = value.ReadValue<float>();
    }
    private void OnThrottleC(InputAction.CallbackContext value)
    {
        ThrottleInput = 0;
    }

    private void OnClutchP(InputAction.CallbackContext value)
    {
        ClutchInput = 1.0f - value.ReadValue<float>();
    }

    private void OnClutchC(InputAction.CallbackContext value)
    {
        ClutchInput = 1.0f;
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
        if (enableAutoTransmission)
        {
            return;
        }

        decrementGear(true);
    }
    private void OnDownShiftC(InputAction.CallbackContext value) { }

    private void OnUpShiftP(InputAction.CallbackContext value)
    {
        if (enableAutoTransmission)
        {
            return;
        }

        incrementGear();   
    }

    private void OnUpShiftC(InputAction.CallbackContext value) { }

    private void OnSteeringP(InputAction.CallbackContext value)
    {
        SteeringJoystickVal = value.ReadValue<float>();
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
