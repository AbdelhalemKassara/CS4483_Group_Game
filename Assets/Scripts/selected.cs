using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selected : MonoBehaviour
{
    public CameraManager cameraManager;
    
    private void Awake(){
        SelectionCar(CarSelected.index);
       
    }



    private void SelectionCar(int index)
    {
       
        for(int i =0; i<transform.childCount; i++)
        {
            cameraManager.setCurCar(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.SetActive(i== index);
        }
    }
}
