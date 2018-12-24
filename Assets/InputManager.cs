using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject node_prefab;
    public GameObject selection;

    struct TouchStart {
        public int id;
        public Vector2 position;

        public TouchStart(int i, Vector2 p) {
            id = i;
            position = p;
        }
    };

    Dictionary<int, TouchStart> touches = new Dictionary<int, TouchStart>();

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
#if false
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Vector2 pos = Input.GetTouch(i).position;
                Debug.Log(string.Format("Touch({2}) begin at ({0}, {1})", pos.x, pos.y, i));
                //clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;

                Ray ray = Camera.main.ScreenPointToRay(pos);
                Instantiate(node_prefab, ray.origin, Quaternion.identity);

            } else if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                Vector2 dp = Input.GetTouch(i).deltaPosition;
                Debug.Log(string.Format("Touch({2}) moved by ({0}, {1})", dp.x, dp.y, i));
            }
#endif
            Touch t = Input.GetTouch(i);
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
                    if (selection != null) Debug.Log("Got Something!");
                    OnBegin(t);
                    break;
                case TouchPhase.Moved:
                    if (selection != null)
                        OnDrag(t, selection);
                    else
                        OnMove(t);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnEnd(t);
                    touches.Remove(t.fingerId);
                    break;
            }
        }
    }

    void OnBegin(Touch t) { }
    void OnMove(Touch t) { }
    void OnDrag(Touch t, GameObject obj) {
        obj.transform.position = Camera.main.ScreenPointToRay(t.position).origin;
    }
    void OnEnd(Touch t) { }
}
