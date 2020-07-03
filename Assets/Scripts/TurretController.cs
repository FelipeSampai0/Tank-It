using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TurretController : MonoBehaviour
{
    [Header("Bodies")]

    public Transform Barrel;
    public Transform Muzzle;
    public CannonballPhysics projectile;
    public Animator Recoil;

    [Header("Barrel Settings")]

    [Tooltip("Highest barrel inclination")]
    public float maxAngle = 60;

    [Tooltip("Start bullet velocity")]
    public float muzzleVel = 50;

    [Tooltip("Max bullet velocity")]
    public float maxVel = 100;

    [Tooltip("Min bullet velocity")]
    public float minVel = 1;

    [Tooltip("Time in seconds to turn barrel")]
    public float turnSpeed = 1;

    [Tooltip("Bullets fired per second")]
    public float fireRate = 1;

    private float turnInput = 0;

    public Vector3 muzzlePoint;
    public Vector3 muzzleVector;

    [SerializeField]
    VisualEffect vfx = null;

    void Start()
    {

    }

    void Update()
    {
        
        muzzlePoint = Muzzle.position;
        muzzleVector = Barrel.forward;

        AdjustAim();
        AdjustForce();

        Recoil.ResetTrigger("boom");

        if (Input.GetButtonDown("Fire1"))
        {
            vfx.SendEvent("Trigger");
            vfx.SetVector3("Direction", muzzleVector);

            Recoil.SetTrigger("boom");
            Boom(projectile, muzzlePoint, Barrel.rotation);
        }
    }

    private void Boom(CannonballPhysics bulletType, Vector3 position, Quaternion direction)
    {
        CannonballPhysics ball = Instantiate(bulletType, position, direction);
        ball.GetComponent<Rigidbody>().angularVelocity = (direction * Vector3.right) * 720 * Mathf.Deg2Rad;
        ball.GetComponent<Rigidbody>().velocity = (direction * Vector3.forward) * muzzleVel * ball.baseStats.velocityMod;
    }

    private void AdjustAim()
    {
        if (Input.GetKey("q") && turnInput < 1)
        {
            turnInput += turnSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("e") && turnInput > 0)
        {
            turnInput -= turnSpeed * Time.deltaTime;
        }

        turnInput = Mathf.Clamp(turnInput, 0, 1);

        Quaternion maxPitch = Quaternion.Euler(-maxAngle, Barrel.localRotation.y, Barrel.localRotation.z);
        Barrel.localRotation = Quaternion.Lerp(Quaternion.identity, maxPitch, turnInput);
    }

    private void AdjustForce()
    {
        if (Input.GetKey("1") && muzzleVel < maxVel)
        {
            muzzleVel += 10 * Time.deltaTime;
        }
        else if(Input.GetKey("3") && muzzleVel > minVel)
        {
            muzzleVel -= 10 * Time.deltaTime;
        }

        muzzleVel = Mathf.Clamp(muzzleVel, minVel, maxVel);
    }
}


