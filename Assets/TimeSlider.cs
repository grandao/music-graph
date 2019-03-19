using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Orbit slider

    Allowed to move ccw between {45, 360, 295, 270} degrees
*/
public class TimeSlider : MonoBehaviour
{
    Vector2 e1 = new Vector2(1, 1).normalized;
    Vector2 e2 = new Vector2(1, -1).normalized;

    public void Move(Vector2 position)
    {
        Vector2 v = (position - (Vector2)Camera.main.WorldToScreenPoint(gameObject.transform.parent.position)).normalized;
        float c = Vector2.Dot(e1, v);
        float s = Vector2.Dot(e2, v);

        float angle = Mathf.Atan2(s, c);
        int i = (int)Mathf.Round(angle * (180.0f / Mathf.PI) / 45);

        i = i < 0 ? i + 8 : i;

        if (i > 5) i = 0;
        else if (i > 3) i = 3;


        //Set(i);
        //change node time
        Node node = gameObject.transform.parent.GetComponent<Node>();
        //node.duration = (i + 1) * 0.5f;
        // set not.duration should auto update this position
        switch (i)
        {
            case 0: node.duration = 0.50f; break;
            case 1: node.duration = 1.00f; break;
            case 2: node.duration = 2.00f; break;
            case 3: node.duration = 4.00f; break;
        }
    }

    //move handler
    public void Set(int i)
    {
        if (i < 0) i = 0;
        if (i > 3) i = 3;

        float c = Mathf.Cos(i * 45 * Mathf.PI / 180.0f);
        float s = Mathf.Sin(i * 45 * Mathf.PI / 180.0f);

        Vector2 p = 1.2f * (e1 * c + e2 * s);
        gameObject.transform.localPosition = p;
    }

    public void UpdatePosition()
    {
        Node node = gameObject.transform.parent.GetComponent<Node>();
        switch (node.duration)
        {
            case 0.5f: Set(0); break;
            case 1.0f: Set(1); break;
            case 2.0f: Set(2); break;
            case 4.0f: Set(3); break;
            default: Set(1); break;
        }
    }

    private void Update()
    {
        UpdatePosition();
    }

}
