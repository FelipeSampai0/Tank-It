using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRenderer2 : MonoBehaviour
{
    public LineRenderer line;
    public LineRenderer target;

    [Range(2, 100)]
    public int resolution;
    public int maxSteps;

    private Vector3 velocity;
    private Vector3 position;

    public float yLimit;
    private float g;

    [Range(2, 30)]
    public int linecastResolution;
    public LayerMask canHit;

    Vector3[] arcPoints;
    RaycastHit impact;

    private void Start()
    {
        g = Mathf.Abs(Physics.gravity.y);
    }

    private void Update()
    {
        position = gameObject.GetComponent<TurretController>().muzzlePoint;
        velocity = gameObject.GetComponent<TurretController>().muzzleVector * gameObject.GetComponent<TurretController>().muzzleVel;
        StartCoroutine(RenderArc());
    }

    private IEnumerator RenderArc()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());

        arcPoints = CalculateLineArray();
        yield return null;                
    }

    private IEnumerator RenderTarget()
    {
        target.positionCount = resolution;
        target.SetPositions(CalculateTargetCircle(1, 32, impact.point, impact.normal));
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray;
        var lowestTimeValue = MaxTimeX() / resolution;

        lineArray = new Vector3[resolution + 1];

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }
        return lineArray;
    }

    private Vector3[] CalculateConstantLineArray()
    {
        Vector3[] lineArray;

        float step = Time.fixedDeltaTime * resolution;
        float newSize = MaxTimeX() / step;

        if (newSize > maxSteps)
        {
            newSize = maxSteps;
        }

        lineArray = new Vector3[(int)newSize];

        for (int i = 0; i < lineArray.Length; i++)
        {
            lineArray[i] = CalculateLinePoint(step * i);
        }
        return lineArray;
    }

    private Vector3[] CalculateTargetCircle(float radius, int circleResolution, Vector3 point, Vector3 normal)
    {
        Vector3[] circleArray = new Vector3[circleResolution];



        return circleArray;
    }

    private Vector3 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            var hit = Physics.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), out RaycastHit hitInfo, canHit);
            impact = hitInfo;
            
            if (hit)                
                return hitInfo.point;
        }
        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = velocity.x * t;
        float z = velocity.z * t;
        float y = (velocity.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + position.x, y + position.y, z + position.z);
    }

    private float MaxTimeY()
    {
        var v = velocity.y;
        var vv = v * v;

        var t = (v + Mathf.Sqrt(vv + 2 * g * (position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        var x = velocity.x;
        if (x == 0)
        {
            velocity.x = 000.1f;
            x = velocity.x;
        }

        var t = (HitPosition().x - position.x) / x;
        return t;
    }

    private float MaxTimeZ()
    {
        var z = velocity.z;
        if (z == 0)
        {
            velocity.x = 000.1f;
            z = velocity.x;
        }

        var t = (HitPosition().z - position.z) / z;
        return t;
    }
}