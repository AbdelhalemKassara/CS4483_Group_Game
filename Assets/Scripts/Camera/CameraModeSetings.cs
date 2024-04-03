using System;

namespace CameraA
{
    [Serializable]
    public struct CameraModeSetings
    {
        public CameraModes mode;
        public float x;
        public float y;
        public float z;
        public float fov;
    }
    
    public enum CameraModes : int
    {
        one= 1,
        two = 2,
        three = 3
    }
}