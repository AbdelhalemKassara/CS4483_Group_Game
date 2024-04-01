using UnityEngine;
using System;

namespace Car
{
    [Serializable]//makes the struct visiable in the inspector  
    public struct WheelColliders
    {
        public WheelCollider frontLeft;
        public WheelCollider frontRight;
        public WheelCollider rearLeft;
        public WheelCollider rearRight;
    }
        
    [Serializable]  
    public struct WheelMeshes
    {
        public Transform frontLeft;
        public Transform frontRight;
        public Transform rearLeft;
        public Transform rearRight;
    }

    [Serializable]
    public struct WheelSmoke
    {
        public ParticleSystem frontL;
        public ParticleSystem frontR;
        public ParticleSystem rearL;
        public ParticleSystem rearR;
    }

    [Serializable]
    public enum DiffType
    {
        Open,
        LSD
    }

    [Serializable]
    public enum DriveWheels
    {
        FWD,
        RWD,
        AWD
    }
    
}