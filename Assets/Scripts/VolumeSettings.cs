using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    [SerializeField] private Slider volumeSlider;
    // Start is called before the first frame update
    public void SetMusicVolume()
    {
        float volume = volumeSlider.value;
        myMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }

  
}
