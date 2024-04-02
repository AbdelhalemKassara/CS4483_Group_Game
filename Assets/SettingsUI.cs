using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Toggle automaticToggle;
    public Toggle manualToggle;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial toggle state based on current transmission mode
        automaticToggle.isOn = TransmissionManager.CurrentTransmissionMode == TransmissionManager.TransmissionMode.Automatic;
        manualToggle.isOn = !automaticToggle.isOn;
    }

    // Method to handle automatic transmission selection
    public void OnAutomaticSelected(bool isSelected)
    {
        if (isSelected)
        {
            
            TransmissionManager.SetTransmissionMode(TransmissionManager.TransmissionMode.Automatic);
            manualToggle.isOn = !isSelected;
            Debug.Log("Automatic Transmission Selected");
        }
    }

    // Method to handle manual transmission selection
    public void OnManualSelected(bool isSelected)
    {
        if (isSelected)
        {
            Debug.Log("Manual Transmission Selected");
            TransmissionManager.SetTransmissionMode(TransmissionManager.TransmissionMode.Manual);
            automaticToggle.isOn = !isSelected;
        }
    }
}