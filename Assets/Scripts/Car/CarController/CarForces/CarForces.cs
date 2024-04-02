using UnityEngine;
using System;

namespace Car
{
    public partial class CarController
    {
        public void Steering()
        {
            float angle = maxTurn * SteeringInput;
            WheelColliders.frontLeft.steerAngle = angle;
            WheelColliders.frontRight.steerAngle = angle;
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
    }
}