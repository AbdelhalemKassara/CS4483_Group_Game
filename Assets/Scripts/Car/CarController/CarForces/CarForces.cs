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
            float torque = torqueToWheels * FinalDriveRatio * GearRatio[CurGear] * Time.fixedDeltaTime * ThrottleInput;

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
        
        private void handleDriveWheels(Action<float, WheelCollider, WheelCollider, float, float> f, float torque)
        {
            switch (selectedDriveWheels)
            {
                case DriveWheels.AWD:
                    f(torque/2, WheelColliders.frontLeft, WheelColliders.frontRight, _wheelSlip.frontLForward, _wheelSlip.frontRForward);
                    f(torque/2, WheelColliders.rearLeft, WheelColliders.rearRight, _wheelSlip.rearLForward, _wheelSlip.rearRForward);
                    break;
                case DriveWheels.FWD:
                    f(torque/2, WheelColliders.frontLeft, WheelColliders.frontRight, _wheelSlip.frontLForward, _wheelSlip.frontRForward);
                    break;
                case DriveWheels.RWD:
                    f(torque/2, WheelColliders.rearLeft, WheelColliders.rearRight, _wheelSlip.rearLForward, _wheelSlip.rearRForward);
                    break;
            }
        }
    }
}