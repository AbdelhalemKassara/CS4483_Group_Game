using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class SubmitScore : MonoBehaviour
{
    public TextMeshProUGUI score;
    
    public SQLiteHelper sqLiteHelper;

    // private readonly string _mapName = StaticFinishData.MapName;
    // private readonly int _timeScore = StaticFinishData.TimeScore;
    
    

    void Start()
    {
        // score.text = _timeScore.ToString();
        score.text = "Time Completed: "+200;
    }
    public void OnSubmitScore()
    {
        // var s = sqLiteHelper.UpdateScores(_mapName, inputText.text, _timeScore);
        var s = sqLiteHelper.UpdateScores("map_1", "test1".ToLower(), 177);

        Debug.Log("Test");
        if (s == "Success")
        {
            SceneManager.LoadScene(0);
        }
        
    }
}
