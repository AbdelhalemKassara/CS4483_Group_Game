using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GetSubmitScene : MonoBehaviour
{
   public TextMeshProUGUI mapName;
   public TextMeshProUGUI time;

   void Update()
   {
      int curtime = StaticFinishData.TimeScore;
      mapName.text = GetMapName.MapName(StaticFinishData.MapName);
      time.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(curtime / 60), Mathf.FloorToInt(curtime % 60));
   }

   public void onSubmitClick()
   {
      SceneManager.LoadScene(6);
   }
}
