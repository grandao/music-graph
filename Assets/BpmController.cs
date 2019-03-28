using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpmController : MonoBehaviour
{
    int[] bpms = { 30, 60, 90, 120};
    int current = 1;

    public void Click()
    {
        current = (current + 1) % bpms.Length;
        GameController.GetInstance().SetBPM(bpms[current]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
