using UnityEngine;
using System;

namespace Car
{
    public partial class CarController
    {
        private float BrakeCurve(float peakForce, float pedalInput, float rpm)//rpm of the wheel not the engine
        {
            
            //as velocity increases brake torque should decrease
            //torque = const * pedalForce / velocity
            if(rpm < -1.0f || rpm > 1.0f)
            {
                return Math.Abs(peakForce * pedalInput / rpm);
            }
            else
            {
                return Math.Abs(peakForce * pedalInput);
            }
        }
        
        //TO-DO: give the engine a bit of inertia so there isn't a sudden cutoff of power when it hits the rev limit
        private float EngineCurve(float peakTorque, float curRpm, float maxRpm, AnimationCurve torqueCurve)
        {
            if (timeout <= maxRpmTimeout)
            {
                timeout += Time.fixedDeltaTime;
            }

            if (_shiftTimeout <= gearShiftTimeout)
            {
                _shiftTimeout += Time.fixedDeltaTime;
                return 0;
            }
            
            if (Math.Abs(curRpm) >= maxRpm)
            {
                timeout = 0f;
            }
            
            
            if (timeout >= maxRpmTimeout)
            {
                return torqueCurve.Evaluate(Math.Clamp(curRpm/maxRpm, 0, 1)) * peakTorque * ClutchInput;
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

        private void AutoClutch()
        {
            ClutchInput = Math.Clamp(Math.Abs(Speed) / autoClutchFullEngageSpeed, 0.05f, 1.0f);
        }

        private void AutoTransmission()
        {
            //if the wheels are slipping don't change gears
            
            //otherwise when rpm reaches max-100.0f upshift and when rpm reaches max - 2000.0f downshift
            //we should replace these constants with optimal rpms from the torque curve
            
            //reverse gear won't work
            switch (selectedDriveWheels)
            {
                case DriveWheels.AWD:
                    //if the wheel slips don't change gears
                    if (Math.Abs(_wheelSlip.frontRSide) > 1.0f || Math.Abs(_wheelSlip.frontRForward) > 1.0f ||
                        Math.Abs(_wheelSlip.frontLSide) > 1.0f || Math.Abs(_wheelSlip.frontLForward) > 1.0f ||
                        Math.Abs(_wheelSlip.rearRSide) > 1.0f || Math.Abs(_wheelSlip.rearRForward) > 1.0f ||
                        Math.Abs(_wheelSlip.rearLSide) > 1.0f || Math.Abs(_wheelSlip.rearLForward) > 1.0f)
                    {
                        break;
                    }

                    DecideToSwitchGears();
                    break;
                case DriveWheels.FWD:
                    //if the wheel slips don't change gears
                    if (Math.Abs(_wheelSlip.frontRSide) > 1.0f || Math.Abs(_wheelSlip.frontRForward) > 1.0f ||
                        Math.Abs(_wheelSlip.frontLSide) > 1.0f || Math.Abs(_wheelSlip.frontLForward) > 1.0f)
                    {
                        break;
                    }

                    DecideToSwitchGears();
                    break;
                case DriveWheels.RWD:
                    //if the wheel slips don't change gears
                    if (Math.Abs(_wheelSlip.rearRSide) > 1.0f || Math.Abs(_wheelSlip.rearRForward) > 1.0f ||
                        Math.Abs(_wheelSlip.rearLSide) > 1.0f || Math.Abs(_wheelSlip.rearLForward) > 1.0f)
                    {
                        break;
                    }

                    DecideToSwitchGears();
                    break;
            }
        }
        private void DecideToSwitchGears()
        {
            if (Rpm < MaxRpm - autoTransMinRpmOffsetFromMaxEngineRpm)
            {
                decrementGear(false);
            }
            else if(Rpm > MaxRpm - autoTransMaxRpmOffsetFromMaxEngineRpm)
            {
                incrementGear();
            }
            
                //if the user is pressing the brake and the car is stopped switch into reverse (we are assuming we are already at 1st gear or reverse)
                if (Speed < 0.1f && BrakeInput > 0.1f && CurGear == 1)
                {
                    decrementGear(true);
                }
                else if (Speed < 0.1f && BrakeInput > 0.1f && CurGear == 0)//if the user is pressing on the throttle and we are in reversegear and we are stopped increment gear
                {
                    incrementGear();
                }
            
            
        }
    }
}