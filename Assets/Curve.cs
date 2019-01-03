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
        Vector3 p0 = gameObject.GetComponent<Edge>().origin.gameObject.transform.position;
        Vector3 p3 = gameObject.GetComponent<Edge>().dest.gameObject.transform.position;

        Vector3 dx = new Vector3((p3.x - p0.x) / 2, 0, 0);
        Vector3 p1 = p0 + dx;
        Vector3 p2 = p3 - dx;

        DrawCurve(p0, p1, p2, p3);

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
}