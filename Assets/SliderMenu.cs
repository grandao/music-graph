using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenu : MonoBehaviour
{
    public GameObject button_prefab;
    public string[] labels = new string[] { "A", "B", "C", "D", "E", "F", "G"};
    public int display_count = 5;
    public int draw_count = 7;


    GameObject[] buttons;

    Vector2 dp = Vector2.zero;

    void SetLayer(int layer, GameObject obj)
    {
        obj.layer = layer;
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
            child.gameObject.layer = layer;
    }


    void Awake()
    {
        buttons = new GameObject[labels.Length];
        for (int i = 0; i < labels.Length; ++i)
        {
            buttons[i] = Instantiate(button_prefab, Vector3.zero, Quaternion.identity);
            buttons[i].transform.parent = gameObject.transform;
            SetLayer(gameObject.layer, buttons[i]);
            buttons[i].GetComponentInChildren<TMPro.TextMeshPro>().text = labels[i];

        }
    }

    void Update()
    {
        Rect r = Camera.main.pixelRect;

        int d = (int)r.height / display_count;


        float px = r.width / 2;
        float py = -(draw_count - display_count) / 2 * d;
        

        int size =  d * draw_count;

        int start = (int)(dp.y / d) % draw_count;
        start = start < 0 ? start + draw_count : start;

        py -= dp.y % d;

        Deactivate();

        for (int i = 0; i < draw_count; ++i)
        {
            Vector3 position = Camera.main.ScreenPointToRay(new Vector2(px, py + i * d)).origin;
            //position is to close to the camera: text is clipped
            position.z = -3;

            int idx = (start + i) % buttons.Length;
            buttons[idx].transform.position = position;
            buttons[idx].SetActive(true);
        }
    }

    void Deactivate()
    {
        foreach (GameObject o in buttons)
            o.SetActive(false);
    }


    public void Drag(Vector2 v)
    {
        dp -= v;
    }

    public int Click(Vector2 p)
    {
        Ray ray = Camera.main.ScreenPointToRay(p);
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

        ray.origin -= 100 * ray.direction;

        if (Physics.Raycast(ray, out hit, 1000, layer_mask))
            return System.Array.IndexOf(buttons, hit.collider.gameObject);

        return -1;
    }
}
