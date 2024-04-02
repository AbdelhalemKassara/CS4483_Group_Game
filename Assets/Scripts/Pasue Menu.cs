using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PasueMenu : MonoBehaviour
{
    [SerializeField] private GameObject Dashboard;
    [SerializeField] private GameObject CarHolder;
    [SerializeField] private GameObject PasueMenus;
    [SerializeField] private GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("p") && Cube.gameObject.activeSelf==false)
        {
            PasueMenus.gameObject.SetActive(true);
            CarHolder.gameObject.SetActive(false);
            Dashboard.gameObject.SetActive(false);
        }

    }

    public void back(){

            PasueMenus.gameObject.SetActive(false);
            CarHolder.gameObject.SetActive(true);
            Dashboard.gameObject.SetActive(true);
    }

    public void restart(){

            Application.LoadLevel(Application.loadedLevel);
        
    }

    public void Menu(){

        SceneManager.LoadScene(0);
    }
}
