using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selected : MonoBehaviour
{

    private void Awake(){
        SelectionCar(CarSelected.index);
       
    }



    private void SelectionCar(int index)
    {
       
<<<<<<< Updated upstream
        for(int i =0; i<transform.childCount; i++){

=======
        for(int i =0; i<transform.childCount; i++)
        {

            if(i==index){
            cameraManager.setCurCar(transform.GetChild(i).gameObject);
            }
>>>>>>> Stashed changes
            transform.GetChild(i).gameObject.SetActive(i== index);
        }
    }
}
