using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Finishline : MonoBehaviour
{
    private DashboardManager _dashboardManager;
    private void OnTriggerEnter(Collider collision)
    {
      if (collision.tag ==  "Player")
      {
          StaticFinishData.MapName = "map_1";
          StaticFinishData.TimeScore = Mathf.FloorToInt(_dashboardManager.GetElapsedTime());
          Debug.Log("Testing");

          SceneManager.LoadScene(6);
      }
    }
}