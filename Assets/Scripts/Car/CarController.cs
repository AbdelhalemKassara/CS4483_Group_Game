    using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Serialization; // for convert to single


    
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


[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour //this class inherits the MonoBehaviour class (Start, Update, FixedUpdate)
{

    [SerializeField] 
    protected WheelColliders WheelColliders;

    [SerializeField] 
    private WheelMeshes WheelMeshes;

    public bool FWD;
    public bool RWD;
    public float strengthCoefficient = 500f;// declairs a variable called strengthCoefficient and gives it a value of 20000, f means float
    public float BrakeStrength = 50f;// stores the break strength on each wheel
    public float maxTurn = 20f; // declairs a float called maxTurn and sets it to 20 (degrees)
    public Transform CM;// center of mass
    private Rigidbody rb;// rigid body

    public Transform carPosition; // stores the cars center position
    public List<GameObject> TailLights; // stores the tail lights (for lighting up the tail lights)
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


    //detect slip by getting the speed of the wheels and compare them to the speed of the car
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
        // Debug.Log("Motor Torque");
        // Debug.Log(WheelColliders.rearRight.motorTorque);
        // Debug.Log(WheelColliders.rearLeft.motorTorque);
        // Debug.Log(WheelColliders.frontRight.motorTorque);
        // Debug.Log(WheelColliders.frontLeft.motorTorque);
        // Debug.Log("BreakTorque");
        // Debug.Log(WheelColliders.rearRight.brakeTorque);
        // Debug.Log(WheelColliders.rearLeft.brakeTorque);
        // Debug.Log(WheelColliders.frontRight.brakeTorque);
        // Debug.Log(WheelColliders.frontLeft.brakeTorque);

    }

    
    //kinda terrible way of doing this but it works
    public void EngineRpm()
    {
        // if (RWD)
        // {
        //     
        // }
        //
        // if (FWD)
        // {
        //     
        // }
        WheelColliders.rearLeft.GetGroundHit(out WheelHit wheelData);
        // Debug.Log(wheelData.sidewaysSlip);
        // Debug.Log(wheelData.forwardSlip);
        // Debug.Log("");
        Rpm = WheelColliders.frontLeft.rpm;
        Rpm = Math.Min(Rpm, WheelColliders.frontRight.rpm);
        Rpm = Math.Min(Rpm, WheelColliders.rearLeft.rpm);
        Rpm = Math.Min(Rpm, WheelColliders.rearRight.rpm);
        Rpm = Rpm * FinalDriveRatio * GearRatio[CurGear];// compute the engine rpm based off of the speed of the wheel
        Rpm = 7000 -1;

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
        // Debug.Log(torque);
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
            float TorqueToWheels;
            // if (CurGear == 0 || Rpm < 0f)
            // {
            //     TorqueToWheels = 0;
            // }
            // else
            // {
            //     TorqueToWheels = EngineCurve(strengthCoefficient, MaxRpm, 1000f, Rpm);
            // }
            TorqueToWheels = EngineCurve(strengthCoefficient, MaxRpm, 1000f, Rpm);

            // Debug.Log(TorqueToWheels);
            float torque = TorqueToWheels * FinalDriveRatio * GearRatio[CurGear] * Time.deltaTime * ThrottleInput;
            // Debug.Log("Torque passed " + torque);
            if (FWD)
            {
                WheelColliders.frontLeft.motorTorque = torque;
                WheelColliders.frontRight.motorTorque = torque;
            }
            
            if (RWD)
            {
                // LSD(torque, WheelColliders.rearLeft, WheelColliders.rearRight);
                WheelColliders.rearLeft.motorTorque = 0.5f * torque;
                WheelColliders.rearRight.motorTorque = 0.5f * torque;
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
    public float EngineCurve(float PeakTorque, float PeakRpm, float InitialTorque, float rpm)
    {
        rpm = Math.Abs(rpm);
        float Zero = (float)Math.Sqrt((double)PeakTorque - InitialTorque);
        // Zero = Math.Clamp(Zero, 0, float.MaxValue);
        PeakRpm = (2 * Zero) / PeakRpm;
        return -1f * (float)Math.Pow((double)PeakRpm * rpm - Zero, 2) + PeakTorque;
    }

    public float BrakeCurve(float PeakForce, float startForce, float rpm)//rpm of the wheel not the engine
    {
        //breaking works fine without any rpm beign passed in just set a constant breaking force
        rpm = Math.Abs(rpm);
        return PeakForce * rpm;
    }
    
    public void EngineAudio()
    {
        if (Math.Abs(Rpm) >= 2000f)
        {
            engineSound.pitch = Math.Abs(Rpm) / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }
        else
        {
            engineSound.pitch = 2000f / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }
        // Debug.Log(Math.Abs(Rpm));
        // Debug.Log(engineSound.pitch);
    }
    
    //can't drift because of the open diff.
    
    //Differential types
    public void Spool()
    {// solid connection between the wheels(the wheels spin at the same speed)

    }
    
    public void Open()
    {//the power goes to the wheel with the lest resistance (the wheels spin at diff speeds)//what the game is doing now

    }
    
    public void LSD(float engineTorque, WheelCollider w1,  WheelCollider w2)
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
        
        w1.motorTorque =  (1.0f-rpmDiff)* engineTorque;
        w2.motorTorque =   rpmDiff * engineTorque;
    }

    //might not be possible to implement without it being janky or applying nearly infinte torque
    public void FixedDiff() //equal speed to both wheels
    {
    }
}
