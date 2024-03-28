using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class carSelection : MonoBehaviour
{
    public int currentCar;

    [Header("Play/Buy Buttons")]
    [SerializeField] private Button play;
    [SerializeField] private Button buy;
    [SerializeField] private Text priceText;

    [Header("Car Attributes")]
    [SerializeField] private int[] carPrices;
    

    private void Start(){
        SelectionCar(0);
       
    }

    void Update(){

    if (Input.GetKeyDown("a") && currentCar > 0)
        {
            currentCar -= 1;
     
        }

        else if (Input.GetKeyDown("d") && currentCar < 5 ){
            currentCar += 1;
        }

        SelectionCar(currentCar);

        CarSelected.index = currentCar;


    }

    private void SelectionCar(int index)
    {
       
        for(int i =0; i<transform.childCount; i++){

            transform.GetChild(i).gameObject.SetActive(i == index);
        }

        if(SaveManager.instance.carsUnlocked[index]){

            play.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
        }
        else{
             play.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text = carPrices[index ]+ "$";
        }
    }

    
}
