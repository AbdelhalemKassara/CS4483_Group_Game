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
    public struct WheelSlip
    {
        public float frontLSide;
        public float frontLForward; //acceleration is negative braking is positive

        public float frontRSide;
        public float frontRForward;

        public float rearLSide;
        public float rearLForward;

        public float rearRSide;
        public float rearRForward;
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
    public struct WheelAudio
    {
        public AudioSource frontL;
        public AudioSource frontR;
        public AudioSource rearL;
        public AudioSource rearR;
    }

    [Serializable]
    public struct WheelTrail
    {
        public TrailRenderer frontL;
        public TrailRenderer frontR;
        public TrailRenderer rearL;
        public TrailRenderer rearR;
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