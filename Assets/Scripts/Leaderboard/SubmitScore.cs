using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class SubmitScore : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI mapName;
    
    public SQLiteHelper sqLiteHelper;
    public EnterUsername EnterUsername;

    private readonly string _mapName = StaticFinishData.MapName;
    private readonly int _timeScore = StaticFinishData.TimeScore;
    
    

    void Start()
    {
        score.text = "Time Completed: "+_timeScore;
        mapName.text = GetMapName.MapName(_mapName);
    }
    public void OnSubmitScore()
    {
        if (EnterUsername.SubmitUsername())
        {
            int topScore = sqLiteHelper.GetBestScore(_mapName);
            var s = sqLiteHelper.UpdateScores(_mapName, PlayerInfo.Username.ToLower(), _timeScore);
            if (topScore > _timeScore)
            {
                SaveManager.instance.money += 1000;
            }
            else
            {
                SaveManager.instance.money += 500;
            }
            SaveManager.instance.Save();
            
            Debug.Log("Test");
            if (s == "Success")
            {
                SceneManager.LoadScene(0);
            }
        }
        
    }
}
