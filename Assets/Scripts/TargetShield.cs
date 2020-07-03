using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShield : MonoBehaviour
{

    Rigidbody[] shields;

    [SerializeField]
    [Range(0.1f,10)]
    float RPM = 1;

    [SerializeField]
    [Range(0.1f, 20)]
    float extraForce = 1;

    bool active = true;

    void Start()
    {
        shields = gameObject.GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Rotate(0, (RPM * 360) * Time.deltaTime, 0, Space.Self);
        }
    }

    public void TurnOff()
    {
        active = false;
        foreach (Rigidbody body in shields)
        {
            body.isKinematic = false;
            //body.AddForce(body.transform.right * 300 * -1, ForceMode.Acceleration);
            body.AddForce(transform.up, ForceMode.VelocityChange);
            body.AddTorque(new Vector3(Random.Range(-extraForce, extraForce), Random.Range(-extraForce, extraForce), Random.Range(-extraForce, extraForce)),ForceMode.VelocityChange) ;
        }
    }    

    public void AdjustRPM(float value)
    {
        RPM = value;
    }
}
