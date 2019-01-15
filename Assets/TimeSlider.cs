using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//orbit slider
public class TimeSlider : MonoBehaviour
{
    Vector2 e1 = new Vector2(1, 1).normalized;
    Vector2 e2 = new Vector2(1, -1).normalized;

    public void move(Vector2 position)
    {
        Vector2 v = (position - (Vector2)Camera.main.WorldToScreenPoint(gameObject.transform.parent.position)).normalized;
        float c = Vector2.Dot(e1, v);
        float s = Vector2.Dot(e2, v);

        float angle = Mathf.Atan2(s, c);
        int i = (int)Mathf.Round(angle * (180.0f / Mathf.PI) / 45);

        i = i < 0 ? i + 8 : i;

        if (i > 5) i = 0;
        else if (i > 3) i = 3;

        c = Mathf.Cos(i * 45 * Mathf.PI / 180.0f);
        s = Mathf.Sin(i * 45 * Mathf.PI / 180.0f);

        Vector2 p = 1.2f * (e1 * c + e2 * s);
        gameObject.transform.localPosition = p;

        //change node time
        Node node = gameObject.transform.parent.GetComponent<Node>();
        node.duration = (i + 1) * 0.5f;
    }

}
