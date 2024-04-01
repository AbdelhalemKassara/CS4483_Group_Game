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

    private CarController _carController;
    float elapsedTime;
    
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
        this.speed.text = Convert.ToString(speed) + " KM/H";
    }
    public virtual void setRpm(float rpm)
    {
        curRpm.text = "MaxRPM: 90000 \nRPM: " + Convert.ToString(rpm);
    }

    void Update(){
        setRpm(_carController.getRpm());
        setSpeed(_carController.getSpeed());
        setGear(_carController.getGear());
        
        
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
