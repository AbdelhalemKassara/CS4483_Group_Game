using UnityEngine;
using System;

namespace Car
{
    public partial class CarForces
    {
        void Open(float engineTorque, WheelCollider w1, WheelCollider w2)
        {
            //the power goes to the wheel with the lest resistance (the wheels spin at diff speeds)//what the game is doing now
            w1.motorTorque = 0.5f * engineTorque;
            w2.motorTorque = 0.5f * engineTorque;
        }

        void LSD(float engineTorque, WheelCollider w1, WheelCollider w2)
        {
            //take in the wheel colliders and use break and motor torque to adjust the wheel speeds
            //use rpm to get each of the wheel speeds.

            //if rpm greater on one give less torque
            //on the lesser one give more torque.
            if (w1.rpm == 0 && w2.rpm == 0)
            {
                w1.motorTorque = 0.5f * engineTorque;
                w2.motorTorque = 0.5f * engineTorque;
                return;
            }

            float max = Math.Max(Math.Abs(w1.rpm), Math.Abs(w2.rpm));
            float rpmDiff = w1.rpm / max - w2.rpm / max;

            //rpmDiff should be from -1 to 1 here
            rpmDiff++;
            rpmDiff /= 2; //rpmDiff now ranges from 0 to 1

            w1.motorTorque = (1.0f - rpmDiff) * engineTorque;
            w2.motorTorque = rpmDiff * engineTorque;
        }

        //might not be possible to implement without it being janky or applying nearly infinte torque
        void FixedDiff() //equal speed to both wheels
        {
        }
    }
}