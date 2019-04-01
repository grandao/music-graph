using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    Vector3 oposition;
    Quaternion orot;

    private void Awake()
    {
        oposition = gameObject.transform.localPosition;
        orot = gameObject.transform.localRotation;
    }

    public void MoveToOrigin()
    {
        gameObject.transform.localPosition = oposition;
        gameObject.transform.localRotation = orot;
    }

    public void MoveTo(Vector3 p)
    {
        Vector3 dif = p - gameObject.transform.parent.position;
        float c = dif.normalized.x;
        float s = dif.normalized.y;

        float theta = Mathf.Atan2(s, c) * 180.0f / Mathf.PI;
        var rot = Quaternion.Euler(0, 0, theta);


        gameObject.transform.position = p;
        gameObject.transform.localRotation = orot * rot ;
    }
}
