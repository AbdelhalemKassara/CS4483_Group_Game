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
                return peakForce * pedalInput / rpm;
            }
            else
            {
                return peakForce * pedalInput;
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
    }
}