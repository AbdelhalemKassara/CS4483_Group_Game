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
            
        }

    }
}