using System;
using UnityEngine;

namespace Car
{
    [Serializable]
    public enum DriveWheels
    {
        FWD,
        RWD,
        AWD
    }
    
    [Serializable]
    public enum DiffType
    {
        Open,
        LSD
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
    public struct WheelColliders
    {
        public WheelCollider frontLeft;
        public WheelCollider frontRight;
        public WheelCollider rearLeft;
        public WheelCollider rearRight;
    }
}