    using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
using UnityEngine.Serialization;
using Car;
    
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour //this class inherits the MonoBehaviour class (Start, Update, FixedUpdate)
{
    [SerializeField] protected AnimationCurve torqueCurve; //this should range from 0 to 1 on both axies
    [SerializeField] protected float peakTorque = 6000f;// declairs a variable called strengthCoefficient and gives it a value of 20000, f means float
    [SerializeField] protected float MaxRpm;
    [SerializeField] protected float minRpm = 1000f;
    [SerializeField] private float maxRpmTimeout = 0.1f;
    [SerializeField] protected WheelColliders WheelColliders;
    [SerializeField] private WheelMeshes WheelMeshes;

    [SerializeField] private DiffType selectedDiffType;
    [FormerlySerializedAs("selectedCarEnums")] [SerializeField] protected DriveWheels selectedDriveWheels;

    public float BrakeStrength = 50f;// stores the break strength on each wheel
    public float handbreakMult = 100.0f;
    public float maxTurn = 20f; // declairs a float called maxTurn and sets it to 20 (degrees)
    public Transform CM;// center of mass
    private Rigidbody rb;// rigid body

    public Transform carPosition; // stores the cars center position
    public float[] GearRatio;
    public float FinalDriveRatio;// ratio from the end of the transmittion to the wheels

    public AudioSource engineSound;
   
    protected float Rpm;

    protected float Speed = 0.0f;
    
    //user controls and user controllable variables
    protected float ThrottleInput = 0.0f;
    protected float BrakeInput = 0.0f;
    protected float SteeringInput = 0;//negative is left, positive is right
    protected bool Handbrake = false;
    protected int CurGear = 1; // starts on the first gear (0 is reverse)

    
    
    //components
    private CalcCarProps calcCarProps;
    private CarForces carForces;
    private CarEffects carEffects;
    
    protected void Start()
    {
        calcCarProps = new CalcCarProps(WheelColliders.frontLeft, WheelColliders.frontRight, 
                                    WheelColliders.rearLeft, WheelColliders.rearRight);
        carForces = new CarForces(WheelColliders.frontLeft, WheelColliders.frontRight, WheelColliders.rearLeft,
                                WheelColliders.rearRight, ref peakTorque, ref MaxRpm, ref torqueCurve, ref maxRpmTimeout,
                                ref BrakeStrength, ref handbreakMult, ref maxTurn, ref selectedDiffType, ref FinalDriveRatio,
                                ref GearRatio, ref selectedDriveWheels, ref Rpm);
        carEffects = new CarEffects(ref MaxRpm, ref Rpm, engineSound);
        
        rb = GetComponent<Rigidbody>();
        
        if (CM)// checks to see if the center of mass object exists
        {
            rb.centerOfMass = CM.position - carPosition.position; // sets the center of mass
        }
    }


    //detect slip by getting the speed of the wheels and compare them to the speed of the car
    protected void FixedUpdate() // 
    {
        calcCarProps.FixedUpdate(out Speed, selectedDriveWheels, 
            out Rpm, FinalDriveRatio, GearRatio, CurGear, minRpm);
        
        //CarForces
        //AutoTransmittion(); // function that 
        carForces.FixedUpdate(ThrottleInput, BrakeInput, SteeringInput, Handbrake, CurGear);

        //CarEffects
        //dashboard
        //headlights
        //tireSmoke
        MeshPosition();
        
        carEffects.FixedUpdate();
    }

    public void MeshPosition()
    {
        Vector3 Pos;
        Quaternion quaternion;
        
        WheelColliders.frontLeft.GetWorldPose(out Pos, out quaternion);
        WheelMeshes.frontLeft.rotation = quaternion; //sets the rotation of the wheel to the rotation of the wheel collider
        WheelMeshes.frontLeft.position = Pos; //sets the position of the wheel mesh to the position of the wheel collider
        
        WheelColliders.frontRight.GetWorldPose(out Pos, out quaternion);
        WheelMeshes.frontRight.rotation = quaternion;
        WheelMeshes.frontRight.position = Pos;
        
        WheelColliders.rearLeft.GetWorldPose(out Pos, out quaternion);
        WheelMeshes.rearLeft.rotation = quaternion;
        WheelMeshes.rearLeft.position = Pos;
        
        WheelColliders.rearRight.GetWorldPose(out Pos, out quaternion);
        WheelMeshes.rearRight.rotation = quaternion;
        WheelMeshes.rearRight.position = Pos;
    }
}
