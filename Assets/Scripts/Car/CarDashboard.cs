using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CarDashboard : MonoBehaviour
{
    public GameObject needle;
    private float startPos= 209f, endPos = -32f;
    private float desiredPos;

    public void updateNeedle(float rpm)
    {
        desiredPos = startPos - endPos;
        float temp = rpm / 10000;

        needle.transform.eulerAngles = new Vector3(0, 0, (startPos - temp * desiredPos));

    }
}
