using UnityEngine;
using System;

namespace Car
{
    public partial class CarController
    {
        //Differential types
        private void Open(float engineTorque, WheelCollider w1, WheelCollider w2, float w1ForwardSlip, float w2ForwardSlip)
        {//the power goes to the wheel with the lest resistance (the wheels spin at diff speeds)//what the game is doing now
            w1.motorTorque = 0.5f * engineTorque;
            w2.motorTorque = 0.5f * engineTorque;
        }
        
        private void LSD(float engineTorque, WheelCollider w1,  WheelCollider w2, float w1ForwardSlip, float w2ForwardSlip)
        {
            //take in the wheel colliders and use break and motor torque to adjust the wheel speeds
            //use rpm to get each of the wheel speeds.
            
            //if rpm greater on one give less torque
            //on the lesser one give more torque.
            
            // _wheelSlip.
            if (w1ForwardSlip <= 1.0f && w2ForwardSlip <=  1.0f)
            {
                w1.motorTorque = 0.5f * engineTorque;
                w2.motorTorque = 0.5f * engineTorque;
                return;
            } else if (w2ForwardSlip == 0.0f)//prevent infinity errors
            {
                w2.motorTorque = 1.0f * engineTorque;
                return;
            }

            float max = w1ForwardSlip > w2ForwardSlip ? w1ForwardSlip : w2ForwardSlip;
            float slipDiv = Math.Abs(w1ForwardSlip / w2ForwardSlip);
            slipDiv /= max;
            
            //rpmDiff should be from 0 to 1 here
            Debug.Log(w1ForwardSlip);
            Debug.Log(w2ForwardSlip);
            Debug.Log(w1ForwardSlip / w2ForwardSlip);
            Debug.Log(slipDiv);
            Debug.Log("");
            
            w1.motorTorque =  (1.0f-slipDiv) * engineTorque;
            w2.motorTorque = slipDiv * engineTorque;
        }

        //might not be possible to implement without it being janky or applying nearly infinte torque
        private void FixedDiff() //equal speed to both wheels
        {
        }
    }
}