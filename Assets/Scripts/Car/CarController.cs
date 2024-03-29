    using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
    
[Serializable]//makes the struct visiable in the inspector  
public struct WheelColliders
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;
}
    
[Serializable]  
public struct WheelMeshes
{
    public Transform frontLeft;
    public Transform frontRight;
    public Transform rearLeft;
    public Transform rearRight;
}

[Serializable]
public enum DiffType
{
    Open,
    LSD
}

[Serializable]
public enum DriveWheels
{
    FWD,
    RWD,
    AWD
}
    
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour //this class inherits the MonoBehaviour class (Start, Update, FixedUpdate)
{

    [SerializeField] 
    protected WheelColliders WheelColliders;

    [SerializeField] 
    private WheelMeshes WheelMeshes;

    [SerializeField] 
    private DiffType selectedDiffType;
    [SerializeField] 
    private DriveWheels selectedDriveWheels;

    public float strengthCoefficient = 500f;// declairs a variable called strengthCoefficient and gives it a value of 20000, f means float
    public float BrakeStrength = 50f;// stores the break strength on each wheel
    public float maxTurn = 20f; // declairs a float called maxTurn and sets it to 20 (degrees)
    public Transform CM;// center of mass
    private Rigidbody rb;// rigid body

    public Transform carPosition; // stores the cars center position
    public float[] GearRatio;
    public float FinalDriveRatio;// ratio from the end of the transmittion to the wheels
    
   public AudioSource engineSound;
   
    public float MaxRpm;
    protected float Rpm;
    
    //user controls and user controllable variables
    protected float ThrottleInput = 0;
    protected float BrakeInput = 0;
    protected float SteeringInput = 0;//negative is left, positive is right
    protected bool Handbrake = false;
    protected int CurGear = 1; // starts on the first gear (0 is reverse)

    
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        if (CM)// checks to see if the center of mass object exists
        {
            rb.centerOfMass = CM.position - carPosition.position; // sets the center of mass
        }
    }


    void FixedUpdate() // 
    {
        EngineRpm();// function that calculates the engine rpm
        //AutoTransmittion(); // function that 
        Throttle();
        Breaking();
        Steering();
        MeshPosition();
        handBrake();
        EngineAudio();
    }

    
    public void EngineRpm()
    {
        Rpm = 0;
        
        //take the average of the drive wheels
        switch (selectedDriveWheels)
        {
            case DriveWheels.AWD:
                Rpm += WheelColliders.frontRight.rpm;
                Rpm += WheelColliders.frontLeft.rpm;
                Rpm += WheelColliders.rearLeft.rpm;
                Rpm += WheelColliders.rearRight.rpm;
                Rpm /= 4;
                break;
            case DriveWheels.FWD:
                Rpm += WheelColliders.frontLeft.rpm;
                Rpm += WheelColliders.frontRight.rpm;
                Rpm /= 2;
                break;
            case DriveWheels.RWD:
                Rpm += WheelColliders.rearRight.rpm;
                Rpm += WheelColliders.rearLeft.rpm;
                Rpm /= 2;
                break;
        }
        Rpm = Rpm * FinalDriveRatio * GearRatio[CurGear];// compute the engine rpm based off of the speed of the wheel
        
    }
    


    public void Steering()
    {
        float angle = maxTurn * SteeringInput;
        WheelColliders.frontLeft.steerAngle = angle;
        WheelColliders.frontRight.steerAngle = angle;
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
    public void handBrake()
    {   
        float torque = BrakeStrength * Time.deltaTime * Convert.ToSingle(Handbrake) * 100000f;
        WheelColliders.rearLeft.brakeTorque = torque;
        WheelColliders.rearRight.brakeTorque = torque;
    }
    public void Breaking()
    {
        float torque = BrakeCurve(BrakeStrength, 100f, Rpm / (FinalDriveRatio * GearRatio[CurGear])) * Time.deltaTime * BrakeInput;
        // Debug.Log("Brake Torque: " + torque);
        WheelColliders.frontLeft.brakeTorque = torque;
        WheelColliders.frontRight.brakeTorque = torque;
        WheelColliders.rearLeft.brakeTorque = torque;
        WheelColliders.rearRight.brakeTorque = torque;

    }

    public void Throttle()
    {
        if (Rpm < MaxRpm && Rpm > -MaxRpm)
        {
            float torqueToWheels = EngineCurve(strengthCoefficient, MaxRpm, 1000f, Rpm);
            float torque = torqueToWheels * FinalDriveRatio * GearRatio[CurGear] * Time.deltaTime * ThrottleInput;

            switch (selectedDiffType)
            {
                case DiffType.Open:
                    handleDriveWheels(Open, torque);
                    break;
                case DiffType.LSD:
                    handleDriveWheels(LSD, torque);
                    break;
            }
        }
        else
        {
            WheelColliders.frontLeft.motorTorque = 0f;
            WheelColliders.frontRight.motorTorque = 0f;
            WheelColliders.rearLeft.motorTorque = 0f;
            WheelColliders.rearRight.motorTorque = 0f;
        }
        
    }

    private void handleDriveWheels(Action<float, WheelCollider, WheelCollider> f, float torque)
    {
        switch (selectedDriveWheels)
        {
            case DriveWheels.AWD:
                f(torque/2, WheelColliders.frontLeft, WheelColliders.frontRight);
                f(torque/2, WheelColliders.rearLeft, WheelColliders.rearRight);
                break;
            case DriveWheels.FWD:
                f(torque, WheelColliders.frontLeft, WheelColliders.frontRight);
                break;
            case DriveWheels.RWD:
                f(torque, WheelColliders.rearLeft, WheelColliders.rearRight);
                break;
        }
    }
    private float EngineCurve(float peakTorque, float peakRpm, float initialTorque, float rpm)
    {
        // rpm = Math.Abs(rpm);
        // float Zero = (float)Math.Sqrt((double)PeakTorque - InitialTorque);
        // // Zero = Math.Clamp(Zero, 0, float.MaxValue);
        // PeakRpm = (2 * Zero) / PeakRpm;
        return peakTorque;
    }

    private float BrakeCurve(float peakForce, float startForce, float rpm)//rpm of the wheel not the engine
    {
        //breaking works fine without any rpm beign passed in just set a constant breaking force
        // rpm = Math.Abs(rpm);
        return peakForce;
    }
    
    private void EngineAudio()
    {
        if (Math.Abs(Rpm) >= 2000f)
        {
            engineSound.pitch = Math.Abs(Rpm) / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }
        else
        {
            engineSound.pitch = 2000f / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }
        
        //this is due to the framrate of the display that the user is using so I had the issue when I switched to a 120hz monitor
        // engineSound.pitch *= 4.0f;//for whatever reason sound is quieter after a merge
    }
    
    
    //Differential types
    private void Open(float engineTorque, WheelCollider w1, WheelCollider w2)
    {//the power goes to the wheel with the lest resistance (the wheels spin at diff speeds)//what the game is doing now
        w1.motorTorque = 0.5f * engineTorque;
        w2.motorTorque = 0.5f * engineTorque;
    }
    
    private void LSD(float engineTorque, WheelCollider w1,  WheelCollider w2)
    {
        //take in the wheel colliders and use break and motor torque to adjust the wheel speeds
        //use rpm to get each of the wheel speeds.
        
        //if rpm greater on one give less torque
        //on the lesser one give more torque.
        if (w1.rpm == 0 && w2.rpm == 0)
        {
            w1.motorTorque = 0.5f * engineTorque;
            w2.motorTorque = 0.5f * engineTorque;
            return;
        }
        
        float max = Math.Max(Math.Abs(w1.rpm), Math.Abs(w2.rpm));
        float rpmDiff = w1.rpm/max - w2.rpm/max;
        
        //rpmDiff should be from -1 to 1 here
        rpmDiff++;
        rpmDiff /= 2;//rpmDiff now ranges from 0 to 1
        // Debug.Log(rpmDiff);
        w1.motorTorque =  (1.0f-rpmDiff) * engineTorque;
        w2.motorTorque =   rpmDiff * engineTorque;
    }

    //might not be possible to implement without it being janky or applying nearly infinte torque
    private void FixedDiff() //equal speed to both wheels
    {
    }
}
