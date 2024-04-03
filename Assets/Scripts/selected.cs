using System.Collections;
using System.Collections.Generic;
using CameraA;
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
                CarController carController = transform.GetChild(i).gameObject.GetComponent<CarController>();
                dashboardManager.setCurCarController(carController);
                cameraManager.setCurCar(transform.GetChild(i).gameObject);
                List<CameraModeSetings> val = carController.getCameraModeSetings();
                if (val.Count > 0)
                {
                    cameraManager.setCameraModeSetings(val);
                }
            }
            transform.GetChild(i).gameObject.SetActive(i== index);
        }
    }
}
