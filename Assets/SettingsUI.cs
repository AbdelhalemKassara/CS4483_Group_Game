using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Toggle automaticToggle;
    public Toggle manualToggle;

    public delegate void TransmissionModeChangedHandler(TransmissionManager.TransmissionMode newMode);
    public event TransmissionModeChangedHandler OnTransmissionModeChanged;

    // Method to invoke the OnTransmissionModeChanged event
    public void InvokeOnTransmissionModeChanged(TransmissionManager.TransmissionMode newMode)
    {
        OnTransmissionModeChanged?.Invoke(newMode);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set initial toggle state based on current transmission mode
        UpdateToggleState();
    }

    
    // Method to update toggle state based on current transmission mode
    private void UpdateToggleState()
    {
        switch (TransmissionManager.CurrentTransmissionMode)
        {
            case TransmissionManager.TransmissionMode.Automatic:
                automaticToggle.isOn = true;
                manualToggle.isOn = false;
                break;
            case TransmissionManager.TransmissionMode.Manual:
                automaticToggle.isOn = false;
                manualToggle.isOn = true;
                break;
            default:
                Debug.LogError("Unhandled transmission mode.");
                break;
        }
    }

    // Method to handle automatic transmission selection
    public void OnAutomaticSelected(bool isSelected)
    {
        if (isSelected)
        {
            Debug.Log("Automatic");
            TransmissionManager.SetTransmissionMode(TransmissionManager.TransmissionMode.Automatic);
            manualToggle.isOn = !isSelected;
        }
    }

    // Method to handle manual transmission selection
    public void OnManualSelected(bool isSelected)
    {
        if (isSelected)
        {
            Debug.Log("Manual");
            TransmissionManager.SetTransmissionMode(TransmissionManager.TransmissionMode.Manual);
            automaticToggle.isOn = !isSelected;
        }
    }
}