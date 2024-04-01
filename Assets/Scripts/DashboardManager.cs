using System;
using System.Collections;
using System.Collections.Generic;
using Car;
using TMPro;
using UnityEngine;

public class DashboardManager : MonoBehaviour
{
    public TextMeshProUGUI curGear;
    public TextMeshProUGUI curRpm;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI speed;

    public CarDashboard _carDashboard;
    private CarController _carController;
    float elapsedTime;
    
    public virtual void setGear(int gear)
    {
        if (gear == 0)
        {
            curGear.text = "R";

        }
        else
        {
            curGear.text = "M: " + Convert.ToString(gear);
        }
    }


    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void setCurCarController(CarController carController)
    {
        _carController = carController;
    }

    public virtual void setSpeed(float speed)
    {
        this.speed.text = Convert.ToString((int)speed);
    }

    void Update(){
        _carDashboard.updateNeedle(_carController.getRpm());
        setSpeed(_carController.getSpeed());
        setGear(_carController.getGear());
        
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
