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

    float lastEventTime;
    bool inactivity = false;
    bool autoreset = false;

    public const float MAX_INACTIVITY = 60;

    public void SetState(State s)
    {
        state = s;
        if (state == State.MENU_INPUT)
        {
            menu.SetActive(true);
            EffectController.GetInstance().OnEnterMenu();
        }
        else
        {
            menu.SetActive(false);
            EffectController.GetInstance().Clear();
        }
    }

    void DispatchEvent(Touch t)
    {
        lastEventTime = Time.realtimeSinceStartup;
        switch (state)
        {
            case State.GAME_INPUT:
                GetComponent<GameInput>()?.DispatchEvent(t);
                break;
            case State.MENU_INPUT:
                GetComponent<MenuInput>()?.DispatchEvent(t);
                break;
        }

        // paused by inactivity & got an event
        if (inactivity && !GameController.GetInstance().IsPlaying())
        {
            GameController.GetInstance().Play();
        }
        inactivity = false;
        autoreset = false;
    }

    void Start()
    {
        SetState(State.GAME_INPUT);
        lastEventTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
            DispatchEvent(Input.GetTouch(i));

        if (!inactivity && (Time.realtimeSinceStartup - lastEventTime) > MAX_INACTIVITY)
        {
            GameController.GetInstance().Pause();
            inactivity = true;
            autoreset = true;
        }

        if (autoreset && (Time.realtimeSinceStartup - lastEventTime) > (2* MAX_INACTIVITY))
        {
            lastEventTime = Time.realtimeSinceStartup;
            GameController.GetInstance().Reload();
            GameController.GetInstance().Pause();
            autoreset = false;
        }


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
        if (Input.GetKeyDown(KeyCode.Alpha1)) GetComponent<GameController>().Save("graph.xml");
        else if (Input.GetKeyDown(KeyCode.Alpha2)) GetComponent<GameController>().Load("graph.xml");
#endif
    }

    
}
