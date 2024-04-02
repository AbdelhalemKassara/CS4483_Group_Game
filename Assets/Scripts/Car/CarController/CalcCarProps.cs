using UnityEngine;
using System;


namespace Car
{
    public partial class CarController
    {
        public void EngineRpm()
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
            Rpm += Rpm < 0 ? -minRpm : minRpm;
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
            WheelHit wheelHit;
            WheelColliders.frontLeft.GetGroundHit(out wheelHit);
            _wheelSlip.frontLSide = wheelHit.sidewaysSlip / WheelColliders.frontLeft.sidewaysFriction.extremumSlip;
            _wheelSlip.frontLForward = wheelHit.forwardSlip / WheelColliders.frontLeft.forwardFriction.extremumSlip;
            
            WheelColliders.frontRight.GetGroundHit(out wheelHit);
            _wheelSlip.frontRSide = wheelHit.sidewaysSlip / WheelColliders.frontRight.sidewaysFriction.extremumSlip;
            _wheelSlip.frontRForward = wheelHit.forwardSlip / WheelColliders.frontRight.forwardFriction.extremumSlip;

            WheelColliders.rearLeft.GetGroundHit(out wheelHit);
            _wheelSlip.rearLSide = wheelHit.sidewaysSlip / WheelColliders.rearLeft.sidewaysFriction.extremumSlip;
            _wheelSlip.rearLForward = wheelHit.forwardSlip / WheelColliders.rearLeft.forwardFriction.extremumSlip;

            WheelColliders.rearRight.GetGroundHit(out wheelHit);
            _wheelSlip.rearRSide = wheelHit.sidewaysSlip / WheelColliders.rearRight.sidewaysFriction.extremumSlip;
            _wheelSlip.rearRForward = wheelHit.forwardSlip / WheelColliders.rearRight.forwardFriction.extremumSlip;

            
            // if (Math.Abs(_wheelSlip.frontLSide) > 1.0f || Math.Abs(_wheelSlip.frontRSide) > 1.0f ||
            //     Math.Abs(_wheelSlip.rearLSide) > 1.0f || Math.Abs(_wheelSlip.rearRSide) > 1.0f)
            // {
            //     Debug.Log("Sideways");
            //     Debug.Log(_wheelSlip.frontLSide + " | " + _wheelSlip.frontRSide);
            //     Debug.Log(_wheelSlip.rearLSide + " | " + _wheelSlip.rearRSide);
            //     Debug.Log("");
            // }
            //
            // if (Math.Abs(_wheelSlip.frontLForward) > 1.0f || Math.Abs(_wheelSlip.frontRForward) > 1.0f ||
            //     Math.Abs(_wheelSlip.rearLForward) > 1.0f || Math.Abs(_wheelSlip.rearRForward) > 1.0f)
            // {
            //     Debug.Log("Forward");
            //     Debug.Log(_wheelSlip.frontLForward + " | " + _wheelSlip.frontRForward);
            //     Debug.Log(_wheelSlip.rearLForward + " | " + _wheelSlip.rearRForward);
            //     Debug.Log("");
            // }
        }

    }
}