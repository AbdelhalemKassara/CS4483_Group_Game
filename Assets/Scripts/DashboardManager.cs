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
    float elapsedTime;
    private int currentBest;
    
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
        currentBest = SqLiteHelper.GetBestScore();
        bestTime.text = string.Format("{0:00}:{1:00}", getMinutes(currentBest), getSeconds(currentBest));
    }

    void Update(){
        _carDashboard.updateNeedle(_carController.getRpm());
        setSpeed(_carController.getSpeed());
        setGear(_carController.getGear());

        if (currentBest < Mathf.FloorToInt(elapsedTime))
        {
            timer.color = Color.red;
        }
        elapsedTime += Time.deltaTime;
        timer.text = string.Format("{0:00}:{1:00}", getMinutes(elapsedTime), getSeconds(elapsedTime));
    }
}
