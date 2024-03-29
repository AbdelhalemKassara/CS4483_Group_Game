using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashboardManager : MonoBehaviour
{
    public TextMeshProUGUI curGear;
    public TextMeshProUGUI curRpm;
    
    public virtual void setGear(int gear)
    {
        if (gear == 0)
        {
            curGear.text = "Gear: R";

        }
        else
        {
            curGear.text = "Gear: " + Convert.ToString(gear);
        }
    }

    public virtual void setRpm(float rpm)
    {
        curRpm.text = "MaxRPM: 90000 \nRPM: " + Convert.ToString(rpm);
    }
}
