using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    bool moved = false;
    //track move to discard unintentional small moves.
    Vector2 opos;
    float dist = 0;

    public void DispatchEvent(Touch t)
    {
        switch (t.phase)
        {
            case TouchPhase.Began:
                moved = false;
                opos = t.position;
                dist = 0;
                break;
            case TouchPhase.Moved:
                dist += (t.position - opos).magnitude;
                opos = t.position;
                if (dist > 5)
                {
                    moved = true;
                    GetComponent<InputManager>().menu.GetComponent<SliderMenu>().Drag(t.deltaPosition);
                }
                
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                int id = -1;
                if (!moved)
                {
                    id = GetComponent<InputManager>().menu.GetComponent<SliderMenu>().Click(t.position);
                    if (id >= 0)
                    {
                        Node node = GetComponent<GameInput>().GetSelection().GetComponent<Node>();
                        node.SetNote(id);
                        GetComponent<InputManager>().menu.GetComponent<SliderMenu>().SetSelected(id);
                    }
                    else
                        GetComponent<InputManager>().SetState(InputManager.State.GAME_INPUT);
                }
                break;
        }
    }
}
