using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CarTestLogic : MonoBehaviour
{
    public Rigidbody myRigidBody; //to intialize this variable you do this in the editor by dragging in the rigidBody2D
    public float force = 1;

    private InputMaster input = null;

    private float throttle = 0;
    private float brake = 0;
        
    //difference between "Send Messages" and "Broadcast Messages" is that Broadcast Onfunction can be put in child gameobject
    void Awake()
    {
        input = new InputMaster();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.AddForce(Vector3.forward * (throttle * force));
        myRigidBody.AddForce(Vector3.back * (brake * force));
    }
    
    //called when gameobject is enabled
    private void OnEnable()
    {
        input.Enable();
        input.Car.Throttle.performed += OnThrottleP;
        input.Car.Throttle.canceled += OnThrottleC;
        input.Car.Brake.performed += OnBrakeP;
        input.Car.Brake.canceled += OnBrakeC;

    }
    
    //called when gameobject is disabled
    private void OnDisable()
    {   
        input.Disable();
        input.Car.Throttle.performed -= OnThrottleP;
        input.Car.Throttle.canceled -= OnThrottleC;
        input.Car.Brake.performed -= OnBrakeP;
        input.Car.Brake.canceled -= OnBrakeC;

    }

    public void OnThrottleP(InputAction.CallbackContext value)
    {
        throttle = value.ReadValue<float>();
    }
    public void OnThrottleC(InputAction.CallbackContext value)
    {
        throttle = 0;
    }

    public void OnBrakeP(InputAction.CallbackContext value)
    {
        brake = value.ReadValue<float>();
    }

    public void OnBrakeC(InputAction.CallbackContext value)
    {
        brake = 0;
    }
    
    // public void OnBrake()
    // {
    //     myRigidBody.AddForce(Vector3.back * force);
    // }
}
