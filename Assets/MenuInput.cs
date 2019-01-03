using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    bool moved = false;
    public void DispatchEvent(Touch t)
    {
        switch (t.phase)
        {
            case TouchPhase.Began:
                moved = false;
                break;
            case TouchPhase.Moved:
                moved = true;
                GetComponent<InputManager>().menu.GetComponent<SliderMenu>().Drag(t.deltaPosition);
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
                        node.note = id;
                        //Debug.Log(string.Format("----->Node set to {0}", id));
                    }
                    else
                        GetComponent<InputManager>().SetState(InputManager.State.GAME_INPUT);
                }
                
                //Debug.Log(string.Format("Got button {0}", id));
                break;
        }
        //Debug.Log(string.Format("Got button {0}", t.phase));
    }
}
