using System.Collections.Generic;// using imports namespaces (namespaces are a collection of classes and other data types)
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.Audio;

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
        [SerializeField] private float gearShiftTimeout = 0.1f;
        [SerializeField] private float autoClutchFullEngageSpeed = 10.0f;//in km/h
        [SerializeField] protected bool enableAutoClutch = true;
        
        [SerializeField] private WheelColliders WheelColliders;
        [SerializeField] private WheelMeshes WheelMeshesRot;
        [SerializeField] private WheelMeshes WheelMeshesStatic;
        [SerializeField] private WheelSmoke _wheelSmoke;
        [SerializeField] private WheelAudio wheelAudio;
        [SerializeField] private WheelTrail wheelTrail;
        [SerializeField] private float tireSlipAudioLevel = 1.0f;
        
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
        [SerializeField] protected bool enableAutoTransmission = false;
        [SerializeField] protected float autoTransMinRpmOffsetFromMaxEngineRpm = 4500.0f;
        [SerializeField] protected float autoTransMaxRpmOffsetFromMaxEngineRpm = 100.0f;
        
        [SerializeField] private float sideToSideEmission = 10.0f;
        [SerializeField] private float FrontToBackEmission = 100.0f;
        
        [SerializeField] private HeadLightsManager headLightsManager;

        [SerializeField] private AudioSource engineSound;
        [SerializeField] private AudioMixerGroup myMixerGroup;
 
        //car properties
        private float Rpm;
        protected float Speed;
        private float timeout = 0.0f;//timeout for rev limit
        private WheelSlip _wheelSlip;
        protected float _shiftTimeout = 0.0f;
        
        private float prevEmissionVelocity = 1.0f;
        
        //user controllable variables
        protected float ThrottleInput = 0;
        protected float BrakeInput = 0;
        protected float SteeringInput = 0;//negative is left, positive is right
        protected bool Handbrake = false;
        protected int CurGear = 1; // starts on the first gear (0 is reverse)
        protected float ClutchInput = 1.0f;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            timeout = maxRpmTimeout + 1f;
            _shiftTimeout = gearShiftTimeout + 1f;
            
            if (CM)// checks to see if the center of mass object exists
            {
                rb.centerOfMass = CM.position - carPosition.position; // sets the center of mass
            }

        }


        //detect slip by getting the speed of the wheels and compare them to the speed of the car
        void FixedUpdate() // 
        {
            //CalcCarProps
            CalcSpeed();
            calcSlip();
            EngineRpm();// function that calculates the engine rpm

            //CarForces
            if (enableAutoClutch)
            {
                Debug.Log("asdfads");
                AutoClutch();
            }

            if (enableAutoTransmission)
                AutoTransmission();
            Throttle();
            Breaking();
            Steering();
            handBrake();
            
            //CarEffects
            MeshPosition();
            EngineAudio();
            TireSmoke();
        }

        public float getRpm()
        {
            return Math.Abs(Rpm);
        }

        public float getSpeed()
        {
            return Math.Abs(Speed);
        }

        public int getGear()
        {
            return CurGear;
        }

        public float getMaxRPM()
        {
            return MaxRpm;
        }

        public bool getEnableAutoTransmission()
        {
            return enableAutoTransmission;
        }

        public void setEnableAutoTransmission(bool val)
        {
            enableAutoTransmission = val;
        }

        public bool getEnableAutoClutch()
        {
            return enableAutoClutch;
        }

        public void setEnableAutoClutch(bool val)
        {
            enableAutoClutch = val;
        }

        public void setEngineAudio(float val)
        {
            engineSound.volume = Math.Clamp(val, 0.0f, 1.0f);
        }

        public void setTireSlipAudioLevel(float val)
        {
            tireSlipAudioLevel = Math.Clamp(val, 0.0f, 1.0f);
        }
        
        private void AssignMixer(AudioSource source)
        {
            source.outputAudioMixerGroup = myMixerGroup;
        }

        protected void incrementGear()
        {
            if (CurGear < GearRatio.Length - 1 && !(CurGear == 0 && Speed < -0.1f))
            {
                CurGear++;
                _shiftTimeout = 0.0f;
            }
        }

        protected void decrementGear(bool canGoRev)
        {
            int minGear = canGoRev ? 0 : 1;

            if (CurGear > minGear && !(CurGear == 1 && Speed > 0.1f))
            {
                CurGear--;
                _shiftTimeout = 0.0f;
            }
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