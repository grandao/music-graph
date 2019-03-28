using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public enum State { GAME_INPUT, MENU_INPUT }
    State state = State.GAME_INPUT;

    Vector3 mouse_position = new Vector3();
    bool drag = false;
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

            if (Input.GetMouseButtonUp(0)) { phase = TouchPhase.Ended; drag = false; }
            else if (!drag && Input.GetMouseButtonDown(0)) { phase = TouchPhase.Began; drag = true; }
            else if (drag && mouse_position != Input.mousePosition) phase = TouchPhase.Moved;

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

        // Test save / load state 
#if true
        /*
        bool save = Input.GetKeyDown(KeyCode.Space);
        string name = null;
        if (Input.GetKeyDown(KeyCode.Alpha1)) name = "graph1.xml";
        else if (Input.GetKeyDown(KeyCode.Alpha2)) name = "graph2.xml";
        else if (Input.GetKeyDown(KeyCode.Alpha3)) name = "graph3.xml";
        else if (Input.GetKeyDown(KeyCode.Alpha4)) name = "graph4.xml";

        if (name != null)
        {
            if (save) GetComponent<GameController>().Save(name);
            else GetComponent<GameController>().Load(name);
        }
        */

        if (Input.GetKeyDown(KeyCode.Alpha1)) GetComponent<GameController>().Save("graph.xml");
        else if (Input.GetKeyDown(KeyCode.Alpha2)) GetComponent<GameController>().Load("graph.xml");
#endif
    }

    
}
