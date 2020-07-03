using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{

    bool active = true;

    [SerializeField]
    GameObject Parent = null;

    [SerializeField]
    GameObject Light = null;

    Rigidbody[] parts;

    void Start()
    {
        parts = Parent.GetComponentsInChildren<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Ball") && other.attachedRigidbody.velocity.magnitude > 20)
        {
            if (active)
            {
                active = false;
                Light.SetActive(false);

                foreach(Rigidbody body in parts){
                    body.isKinematic = false;
                    body.AddForce((body.transform.forward * Vector3.Dot(body.transform.forward, other.attachedRigidbody.velocity.normalized)) * -1 * 1000, ForceMode.Acceleration);
                    body.AddExplosionForce(10, other.transform.position, 20);
                }
            }
        }
    }
}
