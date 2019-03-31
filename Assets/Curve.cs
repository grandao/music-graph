using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Curve : MonoBehaviour
{
    Vector3 p0 = Vector3.zero;
    Vector3 p1 = Vector3.zero;

    ParticleSystem system;
    ParticleSystem.Particle[] particles;

    private void Awake()
    {
        system = gameObject.GetComponentInChildren<ParticleSystem>();
        particles = new ParticleSystem.Particle[system.maxParticles];
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
        var p0 = gameObject.GetComponent<Edge>().origin.gameObject.transform.position;
        var p1 = gameObject.GetComponent<Edge>().dest.gameObject.transform.position;

        UpdateTransform(p0, p1);
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

        Vector2 d0 = this.p0 - p0;
        Vector2 d1 = this.p1 - p1;

        //position changed, kill particles outside range
        if (d0.sqrMagnitude > 1e-5 || d1.sqrMagnitude > 1e-5)
        {
            this.p0 = p0;
            this.p1 = p1;

            float u = (p0 - p1).x;
            float v = (p0 - p1).y;
            float w = -u * p1.x - v * p1.y;
            int count = system.GetParticles(particles);

            for (int i = 0; i < count; ++i)
            {
                //particle is in local space which points towards +z
                if (particles[i].position.z > scale)
                    particles[i].lifetime = 0;
            }

            system.SetParticles(particles, count);
        }

    }


    //it seems unity does not implement cross/det for Vector2
    static float cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }


    public bool Intersect(Vector2 pa0, Vector2 pa1)
    {
        Vector2 pb0 = p0;
        Vector2 pb1 = p1;

        Vector2 u, v;
        u = pa1 - pa0;
        v = pb1 - pb0;

        float c = cross(u, v);

        if (Mathf.Abs(c) < 1e-6) return false;

        float t = cross(pb1 - pa1, v) / c;

        return t >= 0.0f && t <= 1.0f;
    }
}