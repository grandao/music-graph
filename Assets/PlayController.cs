using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
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
        if (GameController.GetInstance().IsPlaying())
        {
            bar.SetActive(true);
            arrow.SetActive(false);
        } else
        {
            bar.SetActive(false);
            arrow.SetActive(true);
        }
    }

    public void Click()
    {
        if (GameController.GetInstance().IsPlaying())
            GameController.GetInstance().Pause();
        else
            GameController.GetInstance().Play();
    }
}
