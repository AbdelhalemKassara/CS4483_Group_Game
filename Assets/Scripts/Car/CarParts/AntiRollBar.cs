using UnityEngine;
using UnityEngine.Serialization; // imports the namespace called UnityEngine (namespace are collection of related classes and data)

public class AntiRollBar : MonoBehaviour
{

    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    private Rigidbody car;
    public float AntiRoll = 50000f;
    void Start()
    {
        car = GetComponent<Rigidbody>(); //gets the rigid body of this object (the car frame) 
    }
    void FixedUpdate()//used for physics calculations
    {
        WheelHit hit = new WheelHit();
        
        float travelL = 1f;
        float travelR = 1f;
        
        bool groundedL = leftWheel.GetGroundHit(out hit);
        if (groundedL)
        {
            //hit.point is the point of contact with the ground
            //gets the position of the wheel from the point it is touching the ground and subtracts the wheel radius to get the center of the wheel the divides it by the suspention distance to get the value to be inbetween 1 and zero (1 fully extended , 0 full compressed)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        }
        
        bool groundedR = rightWheel.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        }

        //calculate and apply antiroll force
        float antiRollForce = (travelL - travelR) * AntiRoll;// gets the difference in position then multiplies that by the antiroll force
        if (groundedL)
        {
            car.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);//multiplies the force with the positon of the wheel,applies the force on the center of the wheel
        }
        if (groundedR)
        {
            car.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
        }
    }
}