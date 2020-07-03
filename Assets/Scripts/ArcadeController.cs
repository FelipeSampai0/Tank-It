using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeController : MonoBehaviour
{
    public Rigidbody Rigidbody;
    SphereCollider[] wheels;

    [Header("Movement Settings")]
    public float maxSpeed = 10;
    public float rotSpeed = 30;

    float velocity;

    float groundValue;

    void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody>();
        wheels = gameObject.GetComponentsInChildren<SphereCollider>();

        Rigidbody.centerOfMass = new Vector3(0,-1,0);
    }

    void Update()
    {
        //print(groundValue);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        CalcVelocity();        
        ArcadeMove();
    }

    private void ArcadeMove()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 wantedVelocity;
        Vector3 wantedPosition;

        wantedVelocity = Rigidbody.transform.forward * velocity * maxSpeed;
        wantedPosition = Rigidbody.position + wantedVelocity * Time.deltaTime;        

        Quaternion wantedRotation = Rigidbody.transform.rotation * Quaternion.Euler(Vector3.up * ((inputX * Time.deltaTime) * rotSpeed));

        Rigidbody.MovePosition(wantedPosition);
        Rigidbody.MoveRotation(wantedRotation);
        
    }

    private void GroundCheck()
    {
        float wheelCount = 0;

        foreach (SphereCollider wheel in wheels)
        {
            if (Physics.Raycast(wheel.transform.position, Vector3.down, out RaycastHit Hit, wheel.radius + 0.5f))
            {
                wheelCount++;
            }
        }

        float groundCount = Mathf.Clamp(wheelCount / 10, 0, 1);
        groundValue = groundCount;
    }

    private void CalcVelocity()
    {
        float input = Input.GetAxisRaw("Vertical");

        switch (input)
        {
            case 1:
                {
                    if (groundValue != 0 && velocity < 1)
                    {
                        velocity += (1 * Time.deltaTime) * input;
                    }
                    break;
                }
            case 0:
                {
                    if(groundValue != 0) 
                    { 
                        if (velocity > 0) velocity -= (3 * Time.deltaTime);
                        if (velocity < 0) velocity += (3 * Time.deltaTime);                        
                        if (Mathf.Abs(velocity) < 0.1f) velocity = 0;
                    }
                    break;
                }
            case -1:
                {
                    if (groundValue != 0 && velocity > -1)
                    {
                        velocity += (1 * Time.deltaTime) * input;
                    }
                    break;
                }
        }
        velocity = Mathf.Clamp(velocity, -1, 1);
    }
}
