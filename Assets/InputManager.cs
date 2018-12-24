using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject node_prefab;
    public GameObject selection;
    public GameObject drag;

    struct TouchStart {
        public int id;
        public Vector2 position;

        public TouchStart(int i, Vector2 p) {
            id = i;
            position = p;
        }
    };

    Dictionary<int, TouchStart> touches = new Dictionary<int, TouchStart>();
    Vector3 mouse_position = new Vector3();

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
            DispatchEvent(Input.GetTouch(i));

//Mouse touch simulation for debug
#if true
        TouchPhase phase = TouchPhase.Canceled;

        if (Input.GetMouseButtonUp(0)) phase = TouchPhase.Ended;
        else if (Input.GetMouseButtonDown(0)) phase = TouchPhase.Began;
        else if (mouse_position != Input.mousePosition) phase = TouchPhase.Moved;

        if (phase != TouchPhase.Canceled)
        {
            Touch t = new Touch();
            t.position = Input.mousePosition;
            t.phase = phase;
            t.deltaPosition = Input.mousePosition - mouse_position;
            DispatchEvent(t);
        }
#endif
    }

    void DispatchEvent(Touch t)
    {
        switch (t.phase)
        {
            case TouchPhase.Began:
                touches[t.fingerId] = new TouchStart(t.fingerId, t.position);
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                    selection = hit.transform.gameObject;
                else
                    selection = null;
                if (selection != null) Debug.Log(string.Format("Got {0}", selection.name));

                drag = selection;

                OnBegin(t);
                break;
            case TouchPhase.Moved:
                if (drag != null)
                    OnDrag(t, drag);
                else
                    OnMove(t);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                drag = null;
                OnEnd(t);
                touches.Remove(t.fingerId);
                break;
        }
    }

    void OnBegin(Touch t) { }
    void OnMove(Touch t) { }
    void OnDrag(Touch t, GameObject obj) {
        Vector3 p = Camera.main.ScreenPointToRay(t.position).origin;
        p.z = 0;
        obj.transform.position = p;
    }
    void OnEnd(Touch t) { }
}
