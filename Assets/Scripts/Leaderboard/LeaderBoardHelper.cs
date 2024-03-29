using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderBoardHelper : MonoBehaviour
{
    private TextMeshProUGUI title;
    [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;

    public SQLiteHelper SQLiteHelper;

    public void GetMapData(string currentMap)
    {
        title.text = currentMap;
        List<PlayerScore> playerScores = SQLiteHelper.GetMap(currentMap);

        for (var i = 0; i < 8; i++)
        {
            if (i < playerScores.Capacity)
            {
                names[i].text = "Player: " + playerScores[i].Username;
                scores[i].text = "Time: " + playerScores[i].TimeScore;
            }
            else
            {
                names[i].text = "Player: Unavailable";
                scores[i].text = "Time: X";
            }

        }

    }
}
