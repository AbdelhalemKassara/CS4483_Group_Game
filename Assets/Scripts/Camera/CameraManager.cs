using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraModes : int
{
    one= 1,
    two = 2
}
[Serializable]
public struct CameraModeSetings
{
    public CameraModes mode;
    public float x;
    public float y;
    public float z;
    public float fov;
}


public class CameraManager : MonoBehaviour
{
    [SerializeField]
    protected List<CameraModeSetings> cameraModeSetings = new List<CameraModeSetings>();

    [SerializeField] 
    protected GameObject curCar;

    protected int curCamSetting = 0;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        CameraModeSetings cameraModeSet = cameraModeSetings[curCamSetting];
        switch (cameraModeSet.mode)
        {
            case CameraModes.one:
                transform.position = Vector3.Lerp(transform.position, curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z)), Time.deltaTime);// slowely follows the car
                transform.LookAt(curCar.transform);
                Camera.main.fieldOfView = cameraModeSet.fov;
                break;
            case CameraModes.two:
                transform.position = curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z)); // stays behind the car
                transform.rotation = curCar.transform.rotation;
                Camera.main.fieldOfView = cameraModeSet.fov; 
                break;
        }
    }
    

}
