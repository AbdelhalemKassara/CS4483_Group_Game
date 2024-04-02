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
            var s = sqLiteHelper.UpdateScores(_mapName, PlayerInfo.Username.ToLower(), _timeScore);
            Debug.Log("Test");
            if (s == "Success")
            {
                SceneManager.LoadScene(0);
            }
        }
        
    }
}
