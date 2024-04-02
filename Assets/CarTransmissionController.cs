using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarTransmissionController : MonoBehaviour
{
    public bool automaticTransmission = true; // Default to automatic transmission
    
    void Start()
    {
        // Load the transmission mode from PlayerPrefs if it exists
        if(PlayerPrefs.HasKey("TransmissionMode"))
        {
            automaticTransmission = Convert.ToBoolean(PlayerPrefs.GetInt("TransmissionMode"));
        }
        
        // Set the transmission mode initially
        SetTransmissionMode(automaticTransmission);
    }
    
    public void SetTransmissionMode(bool automatic)
    {
        automaticTransmission = automatic;
        
        // Update the checkbox state in the inspector
        // This assumes you have a reference to the checkbox GameObject
        // Replace "checkboxGameObject" with the actual reference
        // checkboxGameObject.GetComponent<UnityEngine.UI.Toggle>().isOn = automaticTransmission;
        
        // Save the transmission mode to PlayerPrefs
        PlayerPrefs.SetInt("TransmissionMode", Convert.ToInt32(automaticTransmission));
        PlayerPrefs.Save();
    }
}
