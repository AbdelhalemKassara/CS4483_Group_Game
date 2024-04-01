using UnityEngine;
using System;

namespace Car
{
    public partial class CarController
    {
              
        private void MeshPosition()
        {
            Vector3 Pos;
            Quaternion quaternion;
            
            WheelColliders.frontLeft.GetWorldPose(out Pos, out quaternion);
            WheelMeshesRot.frontLeft.rotation = quaternion; //sets the rotation of the wheel to the rotation of the wheel collider
            WheelMeshesRot.frontLeft.position = Pos; //sets the position of the wheel mesh to the position of the wheel collider
            WheelMeshesStatic.frontLeft.position = Pos;
            
            WheelColliders.frontRight.GetWorldPose(out Pos, out quaternion);
            WheelMeshesRot.frontRight.rotation = quaternion;
            WheelMeshesRot.frontRight.position = Pos;
            WheelMeshesStatic.frontRight.position = Pos;

            WheelColliders.rearLeft.GetWorldPose(out Pos, out quaternion);
            WheelMeshesRot.rearLeft.rotation = quaternion;
            WheelMeshesRot.rearLeft.position = Pos;
            WheelMeshesStatic.rearLeft.position = Pos;
            
            WheelColliders.rearRight.GetWorldPose(out Pos, out quaternion);
            WheelMeshesRot.rearRight.rotation = quaternion;
            WheelMeshesRot.rearRight.position = Pos;
            WheelMeshesStatic.rearRight.position = Pos;
        }
        
        private void EngineAudio()
        {
            engineSound.pitch = Math.Abs(Rpm) / MaxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }

        private void TireSmoke()
        {
            ProcessEmission(_wheelSlip.frontLSide, _wheelSlip.frontLForward, _wheelSmoke.frontL);
            ProcessEmission(_wheelSlip.frontRSide, _wheelSlip.frontRForward, _wheelSmoke.frontR);
            ProcessEmission(_wheelSlip.rearLSide, _wheelSlip.rearLForward, _wheelSmoke.rearL);
            ProcessEmission(_wheelSlip.rearRSide, _wheelSlip.rearRForward, _wheelSmoke.rearR);
            
        }

        private void ProcessEmission(float slide, float forward, ParticleSystem particle)
        {
            float rateOverTimeSide = 0.0f;
            float rateOverTimeForward = 0.0f;
            
            ParticleSystem.EmissionModule emission = particle.emission;
            
            ParticleSystem.VelocityOverLifetimeModule emVelocity = particle.velocityOverLifetime;

            if (slide > 1.0f || slide < -1.0f)
            {
                //use this to modify based on how much the wheel is slipping
                rateOverTimeSide = Math.Abs(slide) * sideToSideEmission;
                
                //use this for switching when car spinning in reverse and wheel speed
            }
            
            if (forward > 1.0f || forward < -1.0f)
            {
                rateOverTimeForward = Math.Abs(forward) * FrontToBackEmission;
            }

            emission.rateOverTime = rateOverTimeSide + rateOverTimeForward;
            
            //this is the only reasonable way of having the smoke direction change without
            //having the particle reset all the time
            if (Speed < 0.0f && prevEmissionVelocity != -1.0f)
            {
                prevEmissionVelocity = -1.0f;
                emVelocity.y = -1.0f;
            }
            else if(prevEmissionVelocity != 1.0f)
            {
                prevEmissionVelocity = 1.0f;
                emVelocity.y = 1.0f;
            }
            
            //can't change these values as it causes the particle to reset and redoes the "previous" smoke
            // float sum = rateOverTimeSide + rateOverTimeForward;
            // emVelocity.y = forward * FrontToBackEmissionVelocity * rateOverTimeForward / sum;
            // emVelocity.x = -slide * sideToSideEmissionVelocity * rateOverTimeSide / sum;
        }

    }
}