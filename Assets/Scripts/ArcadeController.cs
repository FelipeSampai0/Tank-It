using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeController : MonoBehaviour
{
    Rigidbody Rigidbody;
    SphereCollider[] wheels;

    public float maxSpeed = 10;
    public float rotSpeed = 30;
    float acceleration = 1;

    void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody>();
        wheels = gameObject.GetComponentsInChildren<SphereCollider>();

        Rigidbody.centerOfMass = Rigidbody.transform.position;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ArcadeMove();
    }

    private void ArcadeMove()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 wantedVelocity = Rigidbody.transform.forward * inputY * maxSpeed;
        Vector3 wantedPosition = Rigidbody.position + wantedVelocity * Time.deltaTime;
        Quaternion wantedRotation = Rigidbody.transform.rotation * Quaternion.Euler(Vector3.up * ((inputX * Time.deltaTime) * rotSpeed));

        if (GroundCheck() > 0)
        {
            Rigidbody.MovePosition(wantedPosition);
            Rigidbody.MoveRotation(wantedRotation);
        }
    }
        

        

    private float GroundCheck()
    {
        float wheelCount = 0;

        foreach (SphereCollider wheel in wheels)
        {
            if (Physics.Raycast(wheel.transform.position, Vector3.down, out RaycastHit Hit, wheel.radius + 0.1f))
            {
                wheelCount++;
            }
        }

        float groundCount = Mathf.Clamp(wheelCount / 10, 0, 1);

        return (groundCount);
    }
}
