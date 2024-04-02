using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraModes : int
{
    one= 1,
    two = 2,
    three = 3
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
    protected float upDownCam = 0.0f;
    protected float leftRightCam = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        float angle;
        if (upDownCam > 0)
        {
            angle = 90.0f + 90.0f * upDownCam;
            angle *= leftRightCam > 0 ? 1 : -1;
        }
        else
        {
            angle = 90.0f * leftRightCam;
        }
        
        CameraModeSetings cameraModeSet = cameraModeSetings[curCamSetting];
        switch (cameraModeSet.mode)
        {
            case CameraModes.one:
                //right now it is learping from the old camera positon to the new position.
                transform.position = Vector3.Lerp(transform.position, RotatePointAroundPivot(curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z)), curCar.transform.position, new Vector3(0.0f, angle, 0.0f)), Time.deltaTime * 3.0f);// slowely follows the car
                transform.LookAt(curCar.transform);

                Camera.main.fieldOfView = cameraModeSet.fov;
                break;
            case CameraModes.two:
                transform.position = RotatePointAroundPivot(curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z)), curCar.transform.position, new Vector3(0.0f, angle, 0.0f)); // stays behind the car
                transform.rotation = curCar.transform.rotation;
                transform.Rotate(new Vector3(0.0f, angle, 0.0f));

                Camera.main.fieldOfView = cameraModeSet.fov; 
                break;
            case CameraModes.three:
                transform.position = Vector3.ClampMagnitude(Vector3.Lerp(transform.position, curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z)), Time.deltaTime), (curCar.transform.position + curCar.transform.TransformDirection(new Vector3(cameraModeSet.x, cameraModeSet.y, cameraModeSet.z))).magnitude);// slowely follows the car
                transform.LookAt(curCar.transform);
                
                Camera.main.fieldOfView = cameraModeSet.fov;
                
                break;
        }
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot,Vector3 angles) {
        Vector3 dir = point - pivot; 
        dir = Quaternion.Euler(angles) * dir; 
        point = dir + pivot; 
        return point; 
    }

    public void setCurCar(GameObject obj)
    {
        curCar = obj;
    }

}
