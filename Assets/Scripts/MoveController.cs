using System;
using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    Rigidbody Rigidbody;

    public float maxSpeed;

    public GameObject wheelMotherL;
    SphereCollider[] wheelsL = new SphereCollider[0];

    public GameObject wheelMotherR;
    SphereCollider[] wheelsR = new SphereCollider[0];

    public GameObject trackL;
    public GameObject trackR;
    Animator AnimL;
    Animator AnimR;
    Vector2 AnimSpeed;
    public float mult;

    private void Start()
    {
        wheelsL = wheelMotherL.GetComponentsInChildren<SphereCollider>();
        wheelsR = wheelMotherR.GetComponentsInChildren<SphereCollider>();
       
        Rigidbody = GetComponent<Rigidbody>();
        //Rigidbody.centerOfMass = Rigidbody.transform.position;
        // Rigidbody.maxAngularVelocity = 7;

        AnimL = trackL.GetComponent<Animator>();
        AnimR = trackR.GetComponent<Animator>();
    }

    private void Update()
    {
        AnimL.SetFloat("Multiplier", AnimSpeed.x * mult);
        AnimR.SetFloat("Multiplier", AnimSpeed.y * mult);
    }

    private void FixedUpdate()
    { 
        Move();
    }

    private void Move()
    {
        float motor = 40;
        float groundCount = 0;

        Vector2 DifInput; 
        DifInput = TankDiferential(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        foreach (SphereCollider wheel in wheelsL)
        {
            bool Contact = Physics.Raycast(wheel.transform.position, Vector3.down, out RaycastHit Hit, wheel.radius + 0.1f);

            if (Contact)
            {
                groundCount++;
                if (Rigidbody.velocity.magnitude < maxSpeed)
                {                    
                    Rigidbody.AddForceAtPosition(transform.forward * ((DifInput.x) * motor), Hit.point, ForceMode.Acceleration);
                } 
            }

        }

        foreach (SphereCollider wheel in wheelsR)
        {
            bool Contact = Physics.Raycast(wheel.transform.position, Vector3.down, out RaycastHit Hit, wheel.radius + 0.1f);

            if (Contact)
            {
                groundCount++;
                if (Rigidbody.velocity.magnitude < maxSpeed)
                {
                    Rigidbody.AddForceAtPosition(transform.forward * ((DifInput.y) * motor), Hit.point, ForceMode.Acceleration);
                } 
               
            }

        }

        if ((groundCount != 0) && (!Input.GetKeyDown(KeyCode.W) || !Input.GetKeyDown(KeyCode.S)))
        {
           Rigidbody.velocity *= 0.88f;
        }

        AnimSpeed = DifInput;
    }

    public Vector2 TankDiferential(float x, float y)
    {
        // Converts a single dual-axis joystick into a differential
        // drive motor control, with support for both drive, turn
        // and pivot operations.

        // INPUTS
        float nJoyX = x * 127;              // Joystick X input                     (-128..+127)
        float nJoyY = y * 127;              // Joystick Y input                     (-128..+127)

        // OUTPUTS
        float nMotMixL;               // Motor (left)  mixed output           (-128..+127)
        float nMotMixR;               // Motor (right) mixed output           (-128..+127)

        // CONFIG
        // - fPivYLimt  : The threshold at which the pivot action starts
        //                This threshold is measured in units on the Y-axis
        //                away from the X-axis (Y=0). A greater value will assign
        //                more of the joystick's range to pivot actions.
        //                Allowable range: (0..+127)
        float fPivYLimit = 127/4;

        // TEMP VARIABLES
        float nMotPremixL;    // Motor (left)  premixed output        (-128..+127)
        float nMotPremixR;    // Motor (right) premixed output        (-128..+127)
        float nPivSpeed;      // Pivot Speed                          (-128..+127)
        float fPivScale;      // Balance scale b/w drive and pivot    (   0..1   )


        // Calculate Drive Turn output due to Joystick X input
        if (nJoyY >= 0)
        {
            // Forward
            nMotPremixL = (nJoyX >= 0) ? 127.0f : (127.0f + nJoyX);
            nMotPremixR = (nJoyX >= 0) ? (127.0f - nJoyX) : 127.0f;
        }
        else
        {
            // Reverse
            nMotPremixL = (nJoyX >= 0) ? (127.0f - nJoyX) : 127.0f;
            nMotPremixR = (nJoyX >= 0) ? 127.0f : (127.0f + nJoyX);
        }

        // Scale Drive output due to Joystick Y input (throttle)
        nMotPremixL = nMotPremixL * nJoyY / 127.0f;
        nMotPremixR = nMotPremixR * nJoyY / 127.0f;

        // Now calculate pivot amount
        // - Strength of pivot (nPivSpeed) based on Joystick X input
        // - Blending of pivot vs drive (fPivScale) based on Joystick Y input
        nPivSpeed = nJoyX;
        fPivScale = (Mathf.Abs(nJoyY) > fPivYLimit) ? 0.0f : (1.0f - Mathf.Abs(nJoyY) / fPivYLimit);

        // Calculate final mix of Drive and Pivot
        nMotMixL = (1.0f - fPivScale) * nMotPremixL + fPivScale * (nPivSpeed);
        nMotMixR = (1.0f - fPivScale) * nMotPremixR + fPivScale * (-nPivSpeed);

        // Convert to Motor PWM range
        // ...
        return new Vector2(nMotMixL/127, nMotMixR/127); ;
    }   
}