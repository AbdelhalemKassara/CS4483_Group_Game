using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carshow : MonoBehaviour
{
    [SerializeField]private Vector3 finalPosition;
    private Vector3 initialPostion;

    void Awake(){

        initialPostion = transform.position;
    }

    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
    }


    void OnDisable()
    {
        
        transform.position = initialPostion;
    }
}
