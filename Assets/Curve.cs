using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Curve : MonoBehaviour
{
    Vector3 p0 = Vector3.zero;
    Vector3 p3 = Vector3.zero;

    ParticleSystem system;

    private void Awake()
    {
        system = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {

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
        UpdateTransform(p0, p3);
    }

    void UpdateTransform(Vector3 p0, Vector3 p1)
    {
        Vector2 a = p0;
        Vector2 b = p1;
        Vector2 d = b - a;
        float scale = d.magnitude;

        float c = d.normalized.x;
        float s = d.normalized.y;

        float theta = Mathf.Atan2(s, c) * 180.0f / Mathf.PI;

        var rot = Quaternion.Euler(0, 0, theta + 180);

        p0.z += 1;
        transform.position = p0;
        transform.rotation = rot;
        //transform.localScale = new Vector3(scale * 100, 15, 25);
        system.startLifetime = scale / system.startSpeed;

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