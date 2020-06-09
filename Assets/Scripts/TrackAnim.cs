using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAnim : MonoBehaviour
{
    public GameObject trackL;
    public GameObject trackR;

    Animator AnimL;
    Animator AnimR;

    Vector2 AnimSpeed;
    public float mult;

    void Start()
    {
        AnimL = trackL.GetComponent<Animator>();
        AnimR = trackR.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 DifInput;
        DifInput = TankDiferential(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        AnimSpeed = DifInput;

        AnimL.SetFloat("Multiplier", AnimSpeed.x * mult);
        AnimR.SetFloat("Multiplier", AnimSpeed.y * mult);
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
