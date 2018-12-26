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
    public enum State { GAME_INPUT, MENU_INPUT }
    State state = State.GAME_INPUT;

    Vector3 mouse_position = new Vector3();
    public GameObject menu;

    public void SetState(State s)
    {
        state = s;
        if (state == State.MENU_INPUT) menu.SetActive(true);
        else menu.SetActive(false);
    }

    void DispatchEvent(Touch t)
    {
        switch (state)
        {
            case State.GAME_INPUT:
                GetComponent<GameInput>()?.DispatchEvent(t);
                break;
            case State.MENU_INPUT:
                GetComponent<MenuInput>()?.DispatchEvent(t);
                break;
        }
    }

    void Start()
    {
        SetState(State.GAME_INPUT);
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
            DispatchEvent(Input.GetTouch(i));


#if true
        //Mouse touch simulation for debug
        //touch also alters mouse state
        if (Input.touchCount == 0)
        {
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
                mouse_position = Input.mousePosition;
            }
        }
#endif
    }

    
}
