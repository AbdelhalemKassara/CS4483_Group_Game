using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSelection : MonoBehaviour
{
    public int currentCar;

    private void Awake(){
        SelectionCar(0);
       
    }

    void Update(){

    if (Input.GetKeyDown("a") && currentCar > 0)
        {
            currentCar -= 1;
     
        }

        else if (Input.GetKeyDown("d") && currentCar < 6 ){
            currentCar += 1;
        }

        SelectionCar(currentCar);

        CarSelected.index = currentCar;


    }

    private void SelectionCar(int index)
    {
       
        for(int i =0; i<transform.childCount; i++){

            transform.GetChild(i).gameObject.SetActive(i== index);
        }
    }

    
}
