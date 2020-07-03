using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour
{
    float xValue;
    float yValue;

    [SerializeField]
    float maxValue = 0;

    void Update()
    {        
        xValue += Input.GetAxis("Mouse X")/maxValue;
        xValue = Mathf.Clamp(xValue, -maxValue, maxValue);

        yValue += Input.GetAxis("Mouse Y")/maxValue;
        yValue = Mathf.Clamp(yValue, -maxValue, maxValue);

        gameObject.transform.localPosition = new Vector3(xValue,0,40);
    }
}
