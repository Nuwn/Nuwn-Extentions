using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer2D : MonoBehaviour
{
    LineRenderer lineRenderer;

    public float velocity { get; set; }
    public float angle { get; set; }
    public int resolution = 20;

    float gravity;
    float radianAngle;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
    }
#if (UNITY_EDITOR)
    private void OnValidate()
    {
        if(lineRenderer != null && Application.isPlaying)
        {
            RenderArc();
        }
    }
#endif
    private void RenderArc()
    {
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(CalcArcArray());
    }

    private Vector3[] CalcArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalcArcPoint(t, maxDistance);
        }
        return arcArray;
    }

    private Vector3 CalcArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
