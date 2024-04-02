using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionManager : MonoBehaviour
{
    public enum TransmissionMode
    {
        Automatic,
        Manual
    }

    // Current transmission mode
    public static TransmissionMode CurrentTransmissionMode { get; private set; } = TransmissionMode.Automatic;

    // Method to set the transmission mode
    public static void SetTransmissionMode(TransmissionMode mode)
    {
        CurrentTransmissionMode = mode;
    }
}
