using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingClock : MonoBehaviour
{
    public Transform sandglass;

    float speed = 2f;
    private void Start()
    {
        
    }
    void Update()
    {
        sandglass.transform.Rotate(Vector3.forward * speed);
    }
}
