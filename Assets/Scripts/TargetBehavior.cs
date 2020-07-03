using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetBehavior : MonoBehaviour
{
    public Transform master;
    public Transform shield;

    public Animator anim;

    Transform[] positions;
    Vector3[] normals;

    [Range(0.69f, 5)]
    public float size;

    [Range(1,10)]
    public float mult;

    bool active = true;

    [SerializeField]
    TargetShield Shield = null;

    [SerializeField]
    [Range(0.1f, 10)]
    float shieldRPM = 1;

    void Start()
    {
        positions = master.GetComponentsInChildren<Transform>();
        normals = new Vector3[positions.Length];

        for (int i = 1; i < positions.Length; i++)
        {
            normals[i] = Vector3.Normalize(positions[i].localPosition);
        }        
    }

    void Update()
    {

        for (int i = 1; i < positions.Length; i++)
        {
            positions[i].localPosition = normals[i] * size;
        }

        float diameter = size * mult;
        shield.localScale = new Vector3(diameter, diameter, diameter);

        Shield.AdjustRPM(shieldRPM);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            OnHit();
        }         
    }

    public void OnHit()
    {
        if (active) {
            active = false;
            anim.SetTrigger("Deactivate");
            Shield.TurnOff();
        } else
        {
            active = !active;
            anim.SetTrigger("Activate");
        }
        
    }    
}
