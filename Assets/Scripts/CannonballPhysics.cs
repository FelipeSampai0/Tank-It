using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CannonballPhysics : MonoBehaviour
{       
    Rigidbody Rigidbody;
    VisualEffect vfx;

    float radius;

    [Header("Physics Settings")]
    public float rollingCoefficient;

    [System.Serializable]
    public struct Stats
    {
        [Header("Projectile Settings")]

        [Tooltip("Modifier for muzzle velocity")]
        public float velocityMod;

        [Tooltip("Bullet mass")]
        public float mass;

        [Tooltip("Damage")]
        public float damage;
    }

    public Stats baseStats = new Stats
    {
        velocityMod = 1,
        mass = 10,
        damage = 1
    };

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        radius = GetComponent<SphereCollider>().radius;
        Rigidbody.mass = baseStats.mass;
        vfx = GetComponent<VisualEffect>();
    }       

    void FixedUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Linecast(transform.position, transform.position - new Vector3(0, radius * 2, 0), out hitInfo))
        {
            float rollingFriction = (rollingCoefficient * baseStats.mass * (Physics.gravity.y * -1) * Mathf.Cos(Vector3.Angle(hitInfo.normal, Vector3.up)));

            if (rollingFriction < 0)
            { 
                rollingFriction *= -1;           
            }

            if (Rigidbody.velocity.magnitude > rollingFriction * Time.fixedDeltaTime)
            {
                var vel = new Vector3(Mathf.Abs(Rigidbody.velocity.x), Mathf.Abs(Rigidbody.velocity.y), Mathf.Abs(Rigidbody.velocity.z));
                var toOne = 1 / Mathf.Max(Mathf.Max(vel.x, vel.y), vel.z);
                Rigidbody.AddForce((Rigidbody.velocity * toOne) * -rollingFriction);
            }
            else
            {
                Rigidbody.Sleep();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Rigidbody.velocity.magnitude > 20 && collision.collider.gameObject.tag != "Player")
        {
            vfx.SendEvent("Trigger");
            vfx.SetVector3("posit",collision.GetContact(0).point);
            vfx.SetVector3("normal", collision.GetContact(0).normal);
            vfx.SetVector3("veloce", Rigidbody.velocity);
        } 
            
    }
}
