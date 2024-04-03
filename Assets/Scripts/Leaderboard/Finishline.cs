using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Finishline : MonoBehaviour
{
    public DashboardManager _dashboardManager;
    public GameObject submitScreen;
    public GameObject dashboard;
    [SerializeField]
    public string mapName = "map_1";
    private void OnTriggerEnter(Collider collision)
    {
      if (collision.tag ==  "Player")
      {
          _dashboardManager.setActiveTimer(false);
          StaticFinishData.MapName = mapName;
          StaticFinishData.TimeScore = Mathf.FloorToInt(_dashboardManager.GetElapsedTime());
          submitScreen.SetActive(true);
          dashboard.SetActive(false);
          Debug.Log("Hello");
      }
    }
}