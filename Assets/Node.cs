using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Node : MonoBehaviour
{
    static int next_id = 0;
    public int id;
    public int note = 0;
    public float duration = 1;

    public GameObject[] note_labels;
    GameObject label;

    //DecorationSocket reference for access outside the main thread
    public DecorationSocket socket;

    void Awake()
    {
        id = next_id++;
        socket = gameObject.GetComponentInChildren<DecorationSocket>(); 
    }


    // Start is called before the first frame update
    void Start()
    {
        SetNote(note);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNote(int i)
    {
        note = i;
        if (note_labels != null)
        {
            if (label != null) Destroy(label);
            if (note_labels[i])
                label = Instantiate(note_labels[i], transform);
        }
    }
}
