using UnityEngine;
using System;
using UnityEditor;

namespace Car
{
    public class CalcCarProps
    {
        private WheelColliders WheelColliders;
        private float Speed;
        
        public CalcCarProps(WheelCollider FL, WheelCollider FR, WheelCollider RL, WheelCollider RR)
        {
            WheelColliders.frontLeft = FL;
            WheelColliders.frontRight = FR;
            WheelColliders.rearRight = RR;
            WheelColliders.rearLeft = RL;
        }

        public void FixedUpdate(out float Speed, DriveWheels selectedDriveWheels,
            out float Rpm, float FinalDriveRatio, float[] GearRatio, int CurGear, float minRpm)
        {
            CalcSpeed(out Speed, selectedDriveWheels);
            calcSlip();
            EngineRpm(out Rpm, selectedDriveWheels, FinalDriveRatio, GearRatio, CurGear, minRpm);
        }
        
        private void CalcSpeed(out float Speed, DriveWheels selectedDriveWheels)
        {
            float speed = 0.0f;
            WheelCollider cur;
            switch (selectedDriveWheels)
            {
                case DriveWheels.AWD:
                    speed += 2.0f * (float)Math.PI * WheelColliders.rearRight.radius * WheelColliders.rearRight.rpm * 3.6f / 60.0f;
                    speed += 2.0f * (float)Math.PI * WheelColliders.rearLeft.radius * WheelColliders.rearLeft.rpm * 3.6f / 60.0f;
                    speed += 2.0f * (float)Math.PI * WheelColliders.frontLeft.radius * WheelColliders.frontLeft.rpm * 3.6f / 60.0f;
                    speed += 2.0f * (float)Math.PI * WheelColliders.frontRight.radius * WheelColliders.frontRight.rpm * 3.6f / 60.0f;

                    speed /= 4.0f;
                    break;
                case DriveWheels.FWD:
                    speed += 2.0f * (float)Math.PI * WheelColliders.frontLeft.radius * WheelColliders.frontLeft.rpm * 3.6f / 60.0f;
                    speed += 2.0f * (float)Math.PI * WheelColliders.frontRight.radius * WheelColliders.frontRight.rpm * 3.6f / 60.0f;

                    speed /= 2.0f;
                    break;
                case DriveWheels.RWD:
                    speed += 2.0f * (float)Math.PI * WheelColliders.rearRight.radius * WheelColliders.rearRight.rpm * 3.6f / 60.0f;
                    speed += 2.0f * (float)Math.PI * WheelColliders.rearLeft.radius * WheelColliders.rearLeft.rpm * 3.6f / 60.0f;

                    speed /= 2.0f;
                    break;
            }
        
            Speed = Math.Abs(speed);
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
        public void EngineRpm(out float Rpm, DriveWheels selectedDriveWheels, float FinalDriveRatio, float[] GearRatio, int CurGear, float minRpm)
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
    }
}