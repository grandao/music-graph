using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIState
{
    public enum State { NONE, NODE_DRAG, EDGE_DRAG };
    public State state = State.NONE;

    public void SetNodeDrag()
    {
        state = State.NODE_DRAG;
    }

    public void SetEdgeDrag()
    {
        state = State.EDGE_DRAG;
    }

    public void Clear()
    {
        state = State.NONE;
    }
}

public class InputManager : MonoBehaviour
{
    public GameObject node_prefab;
    public GameObject edge_prefab;

    public GameObject dummy_node;
    public GameObject dummy_edge;

    public GameObject selection;
    public GameObject drag;

    public UIState ui_state = new UIState();

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

    void Awake()
    {
        dummy_node = new GameObject();
        dummy_node.SetActive(false);

        dummy_edge = Instantiate(edge_prefab);
        dummy_edge.SetActive(false);
        dummy_edge.GetComponent<Bezier>().dest = dummy_node;
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

                OnBegin(t, selection);
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

    void OnBegin(Touch t, GameObject obj)
    {
        if (obj != null)
        {
            if (obj.name == "EdgeLink")
            {
                ui_state.SetEdgeDrag();

                Vector3 position = Camera.main.ScreenPointToRay(t.position).origin;
                position.z = 0;

                dummy_edge.GetComponent<Bezier>().origin = obj.transform.parent.gameObject;
                dummy_node.transform.position = position;
                dummy_node.SetActive(true);
                dummy_edge.SetActive(true);
            }
            else if (obj.name.Contains("Node")) ui_state.SetNodeDrag();
            else ui_state.Clear();
        }
        else
            ui_state.Clear();

        Debug.Log(ui_state.state);
    }

    void OnMove(Touch t)
    {
    }

    void OnDrag(Touch t, GameObject obj) {
        Vector3 position = Camera.main.ScreenPointToRay(t.position).origin;
        position.z = 0;

        switch (ui_state.state)
        {
            case UIState.State.NODE_DRAG:
                obj.transform.position = position;
                break;
            case UIState.State.EDGE_DRAG:
                dummy_node.transform.position = position;
                break;
        }
        
    }

    void OnEnd(Touch t)
    {
        switch (ui_state.state)
        {
            case UIState.State.NODE_DRAG:
                break;
            case UIState.State.EDGE_DRAG:
                dummy_node.SetActive(false);
                dummy_edge.SetActive(false);


                Ray ray = Camera.main.ScreenPointToRay(t.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject from = dummy_edge.GetComponent<Bezier>().origin;
                    GameObject to = hit.transform.gameObject;

                    if (from != to && to.name.Contains("Node"))
                    {
                        GameObject edge = Instantiate(edge_prefab, from.transform.position, Quaternion.identity);
                        edge.GetComponent<Bezier>().origin = from;
                        edge.GetComponent<Bezier>().dest = to;
                    }
                }
                else
                {
                    GameObject from = dummy_edge.GetComponent<Bezier>().origin;
                    GameObject to = Instantiate(node_prefab, dummy_node.transform.position, Quaternion.identity);
                    GameObject edge = Instantiate(edge_prefab, from.transform.position, Quaternion.identity);

                    edge.GetComponent<Bezier>().origin = from;
                    edge.GetComponent<Bezier>().dest = to;
                }

                dummy_edge.GetComponent<Bezier>().origin = null;
                break;
        }
    }
}
