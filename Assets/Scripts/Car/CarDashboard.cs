using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CarDashboard : MonoBehaviour
{
    public GameObject needle;
    private float startPos= 210f, endPos = -32f;
    private float desiredPos;
    public 

    public float rpm;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateNeedle();
    }

    public void updateNeedle()
    {
        desiredPos = startPos - endPos;
        float temp = rpm / 10000;

        needle.transform.eulerAngles = new Vector3(0, 0,(startPos - temp * desiredPos));

    }
}
