using System.Collections;
using System.Collections.Generic;
using Car;
using UnityEngine;

public class selected : MonoBehaviour
{
    public CameraManager cameraManager;
    public DashboardManager dashboardManager;
    private void Awake(){
        SelectionCar(CarSelected.index);
       
    }



    private void SelectionCar(int index)
    {
       
        for(int i =0; i<transform.childCount; i++)
        {
            if (i == index)
            {
                cameraManager.setCurCar(transform.GetChild(i).gameObject);
                dashboardManager.setCurCarController(transform.GetChild(i).gameObject.GetComponent<CarController>());
            }
            transform.GetChild(i).gameObject.SetActive(i== index);
        }
    }
}
