using UnityEngine;
using System;

namespace Car
{
    public class CarEffects
    {
        private float _maxRpm;

        private AudioSource _engineSound;
        //computed values
        private float _rpm;

        public CarEffects(ref float maxRpm, ref float rpm, AudioSource engineSound)
        {
            _maxRpm = maxRpm;
            _rpm = rpm;
            _engineSound = engineSound;
        }

        public void FixedUpdate()
        {
            EngineAudio();
        }
        
        private void EngineAudio()
        {
            _engineSound.pitch = Math.Abs(_rpm) / _maxRpm;// change the pitch to depending on the rpm (audio file needs to be at max rpm)
        }

    }
}