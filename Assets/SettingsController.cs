using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle automaticToggle;
    public Toggle manualToggle;
    
    public CarTransmissionController carTransmissionController;
    
    void Start()
    {
        // Set the toggles' initial states based on the current transmission mode
        automaticToggle.isOn = carTransmissionController.automaticTransmission;
        manualToggle.isOn = !carTransmissionController.automaticTransmission;
    }
    
    public void OnAutomaticToggle()
    {
        if(automaticToggle.isOn)
        {
            carTransmissionController.SetTransmissionMode(true);
        }
    }
    
    public void OnManualToggle()
    {
        if(manualToggle.isOn)
        {
            carTransmissionController.SetTransmissionMode(false);
        }
    }
}
