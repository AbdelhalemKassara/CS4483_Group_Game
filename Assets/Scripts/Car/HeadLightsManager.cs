using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLightsManager : MonoBehaviour
{
    public List<Light> lights;// list that stores the Headlights

    public virtual void turnOnHeadlights() 
    {
        foreach (Light light in lights) // for each of the light objects 
        {
            light.intensity = 2; // change the light intensity
        }
    }
    
    public virtual void turnOffHeadlights() 
    {
        foreach (Light light in lights) // for each of the light objects 
        {
            light.intensity = 0; // change the light intensity
        }
    }
   
}
