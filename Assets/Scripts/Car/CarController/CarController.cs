using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace Car 
{
    [RequireComponent(typeof(Rigidbody))]
    public partial class CarController : MonoBehaviour //this class inherits the MonoBehaviour class (Start, Update, FixedUpdate)
    {
        [SerializeField] private AnimationCurve torqueCurve; //this should range from 0 to 1 on both axies
        [SerializeField] private float peakTorque = 6000f;// declairs a variable called strengthCoefficient and gives it a value of 20000, f means float
        [SerializeField] private float MaxRpm;
        [SerializeField] private float minRpm = 1000f;
        [SerializeField] private float maxRpmTimeout = 0.1f;
        [SerializeField] private WheelColliders WheelColliders;
        [SerializeField] private WheelMeshes WheelMeshesRot;
        [SerializeField] private WheelMeshes WheelMeshesStatic;

        [SerializeField] private DiffType selectedDiffType;
        [SerializeField] private DriveWheels selectedDriveWheels;

        [SerializeField] private float BrakeStrength = 50f;// stores the break strength on each wheel
        [SerializeField] private float handbreakMult = 100.0f;
        [SerializeField] private float maxTurn = 20f; // declairs a float called maxTurn and sets it to 20 (degrees)
        [SerializeField] private Transform CM;// center of mass
        [SerializeField] private Rigidbody rb;// rigid body

        [SerializeField] private Transform carPosition; // stores the cars center position
        [SerializeField] protected float[] GearRatio;
        [SerializeField] private float FinalDriveRatio;// ratio from the end of the transmittion to the wheels
        
        [SerializeField] private DashboardManager dashboardManager;
        [SerializeField] private HeadLightsManager headLightsManager;

        [SerializeField] private AudioSource engineSound;
       
        //rpm and speed of the car
        private float Rpm;
        private float Speed;
        private float timeout = 0.0f;//timeout for rev limit

        //user controllable variables
        protected float ThrottleInput = 0;
        protected float BrakeInput = 0;
        protected float SteeringInput = 0;//negative is left, positive is right
        protected bool Handbrake = false;
        protected int CurGear = 1; // starts on the first gear (0 is reverse)

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
            //calcCarProps
            CalcSpeed();
            calcSlip();
            EngineRpm();// function that calculates the engine rpm
            
            //AutoTransmittion(); // function that 
            Throttle();
            Breaking();
            Steering();
            handBrake();
            
            //update stuff
            //tireSmoke
            MeshPosition();
            EngineAudio();

            dashboardManager.setGear(CurGear);
            dashboardManager.setRpm(Math.Abs(Rpm));   
            dashboardManager.setSpeed(Speed);
        }

        public float getRpm()
        {
            return Math.Abs(Rpm);
        }

        public float getSpeed()
        {
            return Math.Abs(Speed);
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
        }
    }
}