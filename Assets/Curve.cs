using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Curve : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;
    Vector3 p0 = Vector3.zero;
    Vector3 p3 = Vector3.zero;

    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
    }

    void Update()
    {
        if (gameObject.GetComponent<Edge>() == null)
        {
            Debug.Log("No edge comp!");
            return;
        }

        if (gameObject.GetComponent<Edge>().origin == null)
        {
            Debug.Log("No edge.origin comp!");
            return;
        }
        p0 = gameObject.GetComponent<Edge>().origin.gameObject.transform.position;
        p3 = gameObject.GetComponent<Edge>().dest.gameObject.transform.position;

        Vector3 dx = new Vector3((p3.x - p0.x) / 2, 0, 0);
        Vector3 p1 = p0 + dx;
        Vector3 p2 = p3 - dx;

        //render lines instead of bezier
#if false
        DrawCurve(p0, p1, p2, p3);
#else
        DrawLine(p0, p3);
#endif
    }

    void DrawCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        lineRenderer.positionCount = SEGMENT_COUNT + 1;
        Vector3 u3, u2, u1, u0;
        //power basis
        u3 = -p0 + 3 * p1 - 3 * p2 + p3;
        u2 = 3 * p0 - 6 * p1 + 3 * p2;
        u1 = -3 * p0 + 3 * p1;
        u0 = p0;

        for (int i = 0; i <= SEGMENT_COUNT; i++)
        {
            float t = i / (float)SEGMENT_COUNT;
            Vector3 p = u0 + t * (u1 + t * (u2 + t * u3)); 
            lineRenderer.SetPosition(i, p);
        }
    }

    void DrawLine(Vector3 p0, Vector3 p1)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p1);
    }

    //it seems unity does not implement cross/det for Vector2
    static float cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }


    public bool Intersect(Vector2 pa0, Vector2 pa1)
    {
        Vector2 pb0 = p0;
        Vector2 pb1 = p3;

        Vector2 u, v;
        u = pa1 - pa0;
        v = pb1 - pb0;

        float c = cross(u, v);

        if (Mathf.Abs(c) < 1e-6) return false;

        float t = cross(pb1 - pa1, v) / c;

        return t >= 0.0f && t <= 1.0f;
    }
}