    using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;

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
    [SerializeField] protected AnimationCurve torqueCurve; //this should range from 0 to 1 on both axies
    [SerializeField] protected float peakTorque = 6000f;// declairs a variable called strengthCoefficient and gives it a value of 20000, f means float
    [SerializeField] protected float MaxRpm;
    [SerializeField] protected float minRpm = 1000f;
    [SerializeField] private float maxRpmTimeout = 0.1f;
    [SerializeField] protected WheelColliders WheelColliders;
    [SerializeField] private WheelMeshes WheelMeshes;

    [SerializeField] private DiffType selectedDiffType;
    [SerializeField] protected DriveWheels selectedDriveWheels;
    public CarDashboard carDashboard;

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

    protected float Speed;
    
    //user controls and user controllable variables
    protected float ThrottleInput = 0;
    protected float BrakeInput = 0;
    protected float SteeringInput = 0;//negative is left, positive is right
    protected bool Handbrake = false;
    protected int CurGear = 1; // starts on the first gear (0 is reverse)

    //timeout for rev limit
    private float timeout = 0.0f;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        timeout = maxRpmTimeout + 1f;
        
        if (CM)// checks to see if the center of mass object exists
        {
            rb.centerOfMass = CM.position - carPosition.position; // sets the center of mass
        }
    }


    //detect slip by getting the speed of the wheels and compare them to the speed of the car
    void FixedUpdate() // 
    {
        CalcSpeed();
        calcSlip();
        EngineRpm();// function that calculates the engine rpm
        
        //AutoTransmittion(); // function that 
        Throttle();
        Breaking();
        Steering();
        handBrake();
        
        //update stuff
        //dashboard
        //headlights
        //tireSmoke
        MeshPosition();
        EngineAudio();
    }

    
    private void EngineRpm()
    {
        Rpm = 0;
        
        //take the average of the drive wheels
        switch (selectedDriveWheels)
        {
            case DriveWheels.AWD:
                Rpm += Math.Abs(WheelColliders.frontRight.rpm);
                Rpm += Math.Abs(WheelColliders.frontLeft.rpm);
                Rpm += Math.Abs(WheelColliders.rearLeft.rpm);
                Rpm += Math.Abs(WheelColliders.rearRight.rpm);
                Rpm /= 4;
                break;
            case DriveWheels.FWD:
                Rpm += Math.Abs(WheelColliders.frontLeft.rpm);
                Rpm += Math.Abs(WheelColliders.frontRight.rpm);
                Rpm /= 2;
                break;
            case DriveWheels.RWD:
                Rpm += Math.Abs(WheelColliders.rearRight.rpm);
                Rpm += Math.Abs(WheelColliders.rearLeft.rpm);
                Rpm /= 2;
                break;
        }

        Rpm = Rpm * FinalDriveRatio * GearRatio[CurGear];// compute the engine rpm based off of the speed of the wheel
        Rpm += Rpm < 0 ? -minRpm : minRpm;//fix this garbage
        carDashboard.updateNeedle(Rpm);
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
        WheelCollider cur = WheelColliders.rearLeft;
        cur.brakeTorque = BrakeCurve(BrakeStrength * handbreakMult, Convert.ToSingle(Handbrake), cur.rpm);

        cur = WheelColliders.rearRight;
        cur.brakeTorque = BrakeCurve(BrakeStrength * handbreakMult, Convert.ToSingle(Handbrake), cur.rpm);
    }
    public void Breaking()
    {
        // float torque = BrakeCurve(BrakeStrength, 100f, Rpm / (FinalDriveRatio * GearRatio[CurGear])) * Time.deltaTime * BrakeInput;
        // Debug.Log("Brake Torque: " + torque);
        WheelCollider cur = WheelColliders.frontLeft;
        cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);
        
        cur = WheelColliders.frontRight;
        cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);

        cur = WheelColliders.rearLeft;
        cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);

        cur = WheelColliders.frontRight;
        cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);
    }

    public void Throttle()
    {
        float torqueToWheels = EngineCurve(peakTorque, Rpm, MaxRpm, torqueCurve);
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
    
    //TO-DO: give the engine a bit of inertia so there isn't a sudden cutoff of power when it hits the rev limit
    private float EngineCurve(float peakTorque, float curRpm, float maxRpm, AnimationCurve torqueCurve)
    {
        if (timeout <= maxRpmTimeout)
        {
            timeout += Time.deltaTime;
        }

        if (Math.Abs(curRpm) >= maxRpm)
        {
            timeout = 0f;
        }
        
        if (timeout >= maxRpmTimeout)
        {
            return torqueCurve.Evaluate(Math.Clamp(curRpm/maxRpm, 0, 1)) * peakTorque;
        }
        else if(timeout < maxRpmTimeout && Math.Abs(curRpm) >= maxRpm)
        {
            return -peakTorque * Math.Clamp(Math.Abs(curRpm - maxRpm)/maxRpm, 0, 1); 
        }
        else
        {
            return 0;
        }
    }

    private float BrakeCurve(float peakForce, float pedalInput, float rpm)//rpm of the wheel not the engine
    {
        
        //as velocity increases brake torque should decrease
        //torque = const * pedalForce / velocity
        if(rpm < -1.0f || rpm > 1.0f)
        {
            return peakForce * pedalInput / rpm;
        }
        else
        {
            return peakForce * pedalInput;
        }
    }
    
    private void EngineAudio()
    {
        engineSound.pitch = Math.Abs(Rpm) / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
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

        w1.motorTorque =  (1.0f-rpmDiff) * engineTorque;
        w2.motorTorque =   rpmDiff * engineTorque;
    }

    //might not be possible to implement without it being janky or applying nearly infinte torque
    private void FixedDiff() //equal speed to both wheels
    {
    }

    private void CalcSpeed()
    {
        float speed = 0.0f;
        WheelCollider cur;
        switch (selectedDriveWheels)
        {
            case DriveWheels.AWD:
                cur = WheelColliders.rearRight;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;
                cur = WheelColliders.rearLeft;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;
                cur = WheelColliders.frontLeft;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;
                cur = WheelColliders.frontRight;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;

                speed /= 4.0f;
                break;
            case DriveWheels.FWD:
                cur = WheelColliders.frontLeft;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;
                cur = WheelColliders.frontRight;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;

                speed /= 2.0f;
                break;
            case DriveWheels.RWD:
                cur = WheelColliders.rearRight;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;
                cur = WheelColliders.rearLeft;
                speed += 2.0f * (float)Math.PI * cur.radius * cur.rpm * 3.6f / 60.0f;

                speed /= 2.0f;
                break;
        }

        Speed = speed;
    }
    private void calcSlip()
    {
        WheelHit test;
        WheelColliders.frontLeft.GetGroundHit(out test);
        String str = "";
        str += test.sidewaysSlip / WheelColliders.frontLeft.sidewaysFriction.extremumSlip;
        str += " | ";
        
        WheelColliders.frontRight.GetGroundHit(out test);
        str += test.sidewaysSlip / WheelColliders.frontRight.sidewaysFriction.extremumSlip;
        
        Debug.Log(str);

        WheelColliders.frontLeft.GetGroundHit(out test);
        str = "";
        str += test.sidewaysSlip / WheelColliders.frontLeft.sidewaysFriction.extremumSlip;
        str += " | ";
        
        WheelColliders.frontRight.GetGroundHit(out test);
        str += test.sidewaysSlip / WheelColliders.frontRight.sidewaysFriction.extremumSlip;
        
        Debug.Log(str);
        Debug.Log("");
    }
}
