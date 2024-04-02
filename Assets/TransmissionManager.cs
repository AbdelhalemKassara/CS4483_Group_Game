using UnityEngine;

public class TransmissionManager : MonoBehaviour
{
    // Enum to represent transmission modes
    public enum TransmissionMode
    {
        Automatic,
        Manual
    }

    // Property to store the current transmission mode
    public static TransmissionMode CurrentTransmissionMode { get; private set; }

    // Event to notify subscribers of transmission mode changes
    public static event System.Action<TransmissionMode> OnTransmissionModeChanged;

    // Method to set the transmission mode
    public static void SetTransmissionMode(TransmissionMode mode)
    {
        CurrentTransmissionMode = mode;
        // Invoke the event to notify subscribers of the transmission mode change
        OnTransmissionModeChanged?.Invoke(mode);
    }
}