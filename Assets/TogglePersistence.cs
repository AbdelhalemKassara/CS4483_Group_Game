using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TogglePersistence : MonoBehaviour
{
    public Toggle toggle;

    private void Start()
    {
        // Load the toggle state from PlayerPrefs
        bool isAutomatic = PlayerPrefs.GetInt("IsAutomatic", 0) == 1;
        toggle.isOn = isAutomatic;
    }

    public void OnToggleChanged(bool isAutomatic)
    {
        // Save the toggle state to PlayerPrefs
        PlayerPrefs.SetInt("IsAutomatic", isAutomatic ? 1 : 0);
    }
}
