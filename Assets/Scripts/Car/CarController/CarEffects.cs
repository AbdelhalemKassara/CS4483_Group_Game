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
            float emissionDir;
            if (Rpm < 0.0f)
            {
                emissionDir = 1.0f;
            }
            else
            {
                emissionDir = -1.0f;
            }

            ProcessEmission(_wheelSlip.frontLSide, _wheelSlip.frontLForward, _wheelSmoke.frontL, wheelAudio.frontL, wheelTrail.frontL, emissionDir);
            ProcessEmission(_wheelSlip.frontRSide, _wheelSlip.frontRForward, _wheelSmoke.frontR, wheelAudio.frontR, wheelTrail.frontR, emissionDir);
            ProcessEmission(_wheelSlip.rearLSide, _wheelSlip.rearLForward, _wheelSmoke.rearL, wheelAudio.rearL, wheelTrail.rearL, emissionDir);
            ProcessEmission(_wheelSlip.rearRSide, _wheelSlip.rearRForward, _wheelSmoke.rearR, wheelAudio.rearR, wheelTrail.rearR, emissionDir);
            
        }

        private void ProcessEmission(float slide, float forward, ParticleSystem particle, AudioSource audio, TrailRenderer trail, float emissionDir)
        {
            float rateOverTimeSide = 0.0f;
            float rateOverTimeForward = 0.0f;
            float audioLevel = 0.0f;
            trail.emitting = false;
            
            ParticleSystem.EmissionModule emission = particle.emission;
            
            ParticleSystem.VelocityOverLifetimeModule emVelocity = particle.velocityOverLifetime;

            if (slide > 1.0f || slide < -1.0f)
            {
                //use this to modify based on how much the wheel is slipping
                rateOverTimeSide = Math.Abs(slide) * sideToSideEmission;
                audioLevel += Math.Clamp(Math.Abs(slide) - 1.0f, 0.0f, tireSlipAudioLevel);
                //use this for switching when car spinning in reverse and wheel speed
                trail.emitting = true;
            }
            
            if (forward > 1.0f || forward < -1.0f)
            {
                rateOverTimeForward = Math.Abs(forward) * FrontToBackEmission;
                audioLevel += Math.Clamp(Math.Abs(forward) - 1.0f, 0.0f, tireSlipAudioLevel);
                trail.emitting = true;
            }

            
            
            emission.rateOverTime = rateOverTimeSide + rateOverTimeForward;
            
            audioLevel = Math.Clamp(audioLevel, 0.0f, tireSlipAudioLevel);
            audio.volume = audioLevel;
            AssignMixer(audio);

            
            //this is the only reasonable way of having the smoke direction change without
            //having the particle reset all the time
            if (prevEmissionVelocity != emissionDir)
            {
                prevEmissionVelocity = emissionDir;
                emVelocity.y = emissionDir;
            }
            

     
            
            //can't change these values as it causes the particle to reset and redoes the "previous" smoke
            // float sum = rateOverTimeSide + rateOverTimeForward;
            // emVelocity.y = forward * FrontToBackEmissionVelocity * rateOverTimeForward / sum;
            // emVelocity.x = -slide * sideToSideEmissionVelocity * rateOverTimeSide / sum;
        }

    }
}