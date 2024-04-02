using System;
using System.Collections;
using System.Collections.Generic;
using Car;
using TMPro;
using UnityEngine;

public class DashboardManager : MonoBehaviour
{
    public TextMeshProUGUI curGear;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI speed;

    public CarDashboard _carDashboard;
    private CarController _carController;
    public SQLiteHelper SqLiteHelper;
    private bool activeTimer = true;
    float elapsedTime;
    private int currentBest;
    [SerializeField] public string mapId = "map_1";
    
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

    private int getMinutes(float time)
    {
        return  Mathf.FloorToInt(time / 60);
    }
    private int getSeconds(float time)
    {
        return Mathf.FloorToInt(time % 60);
    }

    private void Start()
    {
        currentBest = SqLiteHelper.GetBestScore(mapId);
        bestTime.text = string.Format("{0:00}:{1:00}", getMinutes(currentBest), getSeconds(currentBest));
    }

    public void setActiveTimer(bool isActive)
    {
        activeTimer = isActive;
    }

    public void updateTimer()
    {
        if (activeTimer)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void Update(){
        _carDashboard.updateNeedle(_carController.getRpm());
        setSpeed(_carController.getSpeed());
        setGear(_carController.getGear());

        if (currentBest < Mathf.FloorToInt(elapsedTime))
        {
            timer.color = Color.red;
        }

        updateTimer();
        timer.text = string.Format("{0:00}:{1:00}", getMinutes(elapsedTime), getSeconds(elapsedTime));
    }
}
