using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleController : MonoBehaviour
{
    //public Toggle autoClutchToggle;
    public Toggle autoTransmissionToggle;

    public void ToggleAutoTransmission()
    {
        
        bool isOn = autoTransmissionToggle.isOn;
        CarSelected.enableAutoTransmission = isOn;
        Debug.Log(CarSelected.enableAutoTransmission);
    }
}