using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    Rigidbody Body;

    WheelCollider[] wheelsL = new WheelCollider[7];
    WheelCollider[] wheelsR = new WheelCollider[7];

    public WheelCollider L1;
    public WheelCollider L2;
    public WheelCollider L3;
    public WheelCollider L4;
    public WheelCollider L5;
    public WheelCollider L6;
    public WheelCollider L7;

    public WheelCollider R1;
    public WheelCollider R2;
    public WheelCollider R3;
    public WheelCollider R4;
    public WheelCollider R5;
    public WheelCollider R6;
    public WheelCollider R7;


    void Start()
    {
        Body = gameObject.GetComponent<Rigidbody>();
        Body.centerOfMass = gameObject.GetComponent<Rigidbody>().position;

        wheelsL[0] = L1;
        wheelsL[1] = L2;
        wheelsL[2] = L3;
        wheelsL[3] = L4;
        wheelsL[4] = L5;
        wheelsL[5] = L6;
        wheelsL[6] = L7;

        wheelsR[0] = R1;
        wheelsR[1] = R2;
        wheelsR[2] = R3;
        wheelsR[3] = R4;
        wheelsR[4] = R5;
        wheelsR[5] = R6;
        wheelsR[6] = R7;
    }

    
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 DifInput;
        Vector2 DifInputRaw;

        float motor = 1000;

        DifInput = TankDiferential(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        DifInputRaw = TankDiferential(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        foreach (WheelCollider wheel in wheelsL)
        {
            wheel.motorTorque = DifInput.x * motor;
        }

        foreach (WheelCollider wheel in wheelsR)
        {
            wheel.motorTorque = DifInput.y * motor;
        }

    }

    private Vector2 TankDiferential(float x, float y)
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
        float fPivYLimit = 127 / 4;

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
        return new Vector2(nMotMixL / 127, nMotMixR / 127); ;
    }
}
