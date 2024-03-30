using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class SubmitScore : MonoBehaviour
{
    [SerializeField]
    public GameObject warning;

    public TextMeshProUGUI score;
    
    [SerializeField]
    public TMP_InputField inputText;

    public SQLiteHelper sqLiteHelper;

    private readonly string _mapName = StaticFinishData.MapName;
    private readonly int _timeScore = StaticFinishData.TimeScore;

    void Start()
    {
        score.text = _timeScore.ToString();
    }
    public void OnSubmitScore()
    {
        if (inputText.text.Length < 3)
        {
            var s = sqLiteHelper.UpdateScores(_mapName, inputText.text, _timeScore);
            Debug.Log("Test");
            if (s == "Success")
            {
                SceneManager.LoadScene(0);
            }
            
        }
        else
        {
            warning.SetActive(true);
        }
        
    }
}
