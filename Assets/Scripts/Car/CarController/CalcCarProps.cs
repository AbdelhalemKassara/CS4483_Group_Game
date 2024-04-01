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
            Rpm += Rpm < 0 ? -minRpm : minRpm;//fix this garbage
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

            WheelColliders.rearLeft.GetGroundHit(out test);
            str = "";
            str += test.sidewaysSlip / WheelColliders.rearLeft.sidewaysFriction.extremumSlip;
            str += " | ";
            
            WheelColliders.rearRight.GetGroundHit(out test);
            str += test.sidewaysSlip / WheelColliders.rearRight.sidewaysFriction.extremumSlip;
            
            Debug.Log(str);
            Debug.Log("");
        }

    }
}