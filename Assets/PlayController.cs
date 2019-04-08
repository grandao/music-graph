using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    bool toggle = false;
    GameObject bar;
    GameObject arrow;
    private void Awake()
    {
        bar = transform.Find("Bar").gameObject;
        arrow = transform.Find("Arrow").gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (toggle)
        {
            GameController.GetInstance().Play();
            bar.SetActive(true);
            arrow.SetActive(false);
        }
        else
        {
            GameController.GetInstance().Pause();
            bar.SetActive(false);
            arrow.SetActive(true);
        }

        toggle = !toggle;
    }
}
