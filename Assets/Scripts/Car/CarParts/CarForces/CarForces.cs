using UnityEngine;
using System;
using Car;

namespace Car
{
    public partial class CarForces
    {
        //values to set in the editor
        private WheelColliders WheelColliders;
        
        private float _peakTorque;
        private float _maxRpm;
        private AnimationCurve _torqueCurve;
        private float _maxRpmTimeout;
        
        private float _brakeStrength;
        private float _handBrakeMult;

        private float _maxTurn;
        
        private DiffType _selectedDiffType;
        private float _finalDriveRatio;
        private float[] _gearRatio;
        private DriveWheels _selectedDriveWheels;
        
        //computed values
        private float _rpm;
        
        //values for this class only
        private float _timeout = 0.0f;//timeout for rev limit
        
        public CarForces(WheelCollider FL, WheelCollider FR, WheelCollider RL, WheelCollider RR,
            ref float peakTorque, ref float maxRpm, ref AnimationCurve torqueCurve, 
            ref float maxRpmTimeout, ref float brakeStrength, ref float handBrakeMult, ref float maxTurn, ref DiffType selectedDiffType,
            ref float finalDriveRatio, ref float[] gearRatio, ref DriveWheels selectedDriveWheels,
            ref float rpm)
        {
            WheelColliders.frontLeft = FL;
            WheelColliders.frontRight = FR;
            WheelColliders.rearRight = RR;
            WheelColliders.rearLeft = RL;

            _peakTorque = peakTorque;
            _maxRpm = maxRpm;
            _torqueCurve = torqueCurve;
            _maxRpmTimeout = maxRpmTimeout;
            
            _brakeStrength = brakeStrength;
            _handBrakeMult = handBrakeMult;

            _maxTurn = maxTurn;
            
            _selectedDiffType = selectedDiffType;
            _finalDriveRatio = finalDriveRatio;
            _gearRatio = gearRatio;
            _selectedDriveWheels = selectedDriveWheels;
            
            _rpm = rpm;

            
            _timeout = _maxRpmTimeout + 1f;
        }


        public void FixedUpdate(float throttleInput, float brakeInput, float steeringInput, bool Handbrake, int curGear)
        {
            Debug.Log(_rpm);
            Throttle(curGear, throttleInput);
            Braking(_brakeStrength, brakeInput);
            HandBrake(_brakeStrength, _handBrakeMult, Handbrake);
            Steering(steeringInput);
        }   
        
        private void Braking(float BrakeStrength, float BrakeInput)
        {
            WheelCollider cur = WheelColliders.frontLeft;
            cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);
        
            cur = WheelColliders.frontRight;
            cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);

            cur = WheelColliders.rearLeft;
            cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);

            cur = WheelColliders.frontRight;
            cur.brakeTorque = BrakeCurve(BrakeStrength, BrakeInput, cur.rpm);
        }
        public void HandBrake(float BrakeStrength, float handbreakMult, bool Handbrake)
        {
            WheelCollider cur = WheelColliders.rearLeft;
            cur.brakeTorque = BrakeCurve(BrakeStrength * handbreakMult, Convert.ToSingle(Handbrake), cur.rpm);

            cur = WheelColliders.rearRight;
            cur.brakeTorque = BrakeCurve(BrakeStrength * handbreakMult, Convert.ToSingle(Handbrake), cur.rpm);
        }
        
        public void Throttle(int curGear, float throttleInput)
        {
            float torqueToWheels = EngineCurve(_peakTorque, _rpm, _maxRpm, _torqueCurve, _maxRpmTimeout);
            float torque = torqueToWheels * _finalDriveRatio * _gearRatio[curGear] * Time.deltaTime * throttleInput;
            
            switch (_selectedDiffType)
            {
                case DiffType.Open:
                    handleDriveWheels(Open, torque, _selectedDriveWheels);
                    break;
                case DiffType.LSD:
                    handleDriveWheels(LSD, torque, _selectedDriveWheels);
                    break;
            }
        }
        private void handleDriveWheels(Action<float, WheelCollider, WheelCollider> f, float torque, DriveWheels selectedDriveWheels)
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
        public void Steering(float steeringInput)
        {
            float angle = _maxTurn * steeringInput;
            WheelColliders.frontLeft.steerAngle = angle;
            WheelColliders.frontRight.steerAngle = angle;
        }
        
        //TO-DO: give the engine a bit of inertia so there isn't a sudden cutoff of power when it hits the rev limit
        private float EngineCurve(float peakTorque, float curRpm, float maxRpm, AnimationCurve torqueCurve, float maxRpmTimeout)
        {
            if (_timeout <= maxRpmTimeout)
            {
                _timeout += Time.deltaTime;
            }

            if (Math.Abs(curRpm) >= maxRpm)
            {
                _timeout = 0f;
            }
        
            if (_timeout >= maxRpmTimeout)
            {
                return torqueCurve.Evaluate(Math.Clamp(curRpm/maxRpm, 0, 1)) * peakTorque;
            }
            else if(_timeout < maxRpmTimeout && Math.Abs(curRpm) >= maxRpm)
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
        
    }
}