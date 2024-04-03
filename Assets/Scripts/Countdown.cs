using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private Text CountDown;
    [SerializeField] private GameObject Dashboard;
    [SerializeField] private VolumeSettings VolumeSettings;

    private int Num = 5;
    private float countdownDelay = 1f; // Delay between counts

    void Start()
    {
        StartCoroutine(StartCountdown());
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            VolumeSettings.LoadVolume();
        }
        else
        {
            VolumeSettings.SetMusicVolume();
        }
    }

    IEnumerator StartCountdown()
    {
        while (Num > 0)
        {
            CountDown.text = Num.ToString();
            yield return new WaitForSeconds(countdownDelay);
            Num--;
        }

        if(Num == 0){
            CountDown.text = "Start";
            gameObject.SetActive(false);
            CountDown.gameObject.SetActive(false);
            Dashboard.gameObject.SetActive(true);
        }
        
        // Optionally, you can add code here to handle what happens after the countdown finishes.
    }
}
