﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationPallete : MonoBehaviour
{
    public GameObject decoration_prefab;
    public Material[] materials;

    int[] mid = { 5, 11, 7, 10, 8, 1, 3, 4, 5, 2 };
    float[] rot = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Start is called before the first frame update
    void Create()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < 10; ++i)
        {
            GameObject d = DecorationInstancer.Create(Decoration.DecorationType.NODE_STYLE, i);
            d.transform.parent = gameObject.transform;
            d.transform.localPosition = pos;
            pos.x += 2 * 0.8f;
        }
    }

    // Create decoration pallete in hexagon form
    void CreateHexagon(float dx)
    {
        float dy = Mathf.Sqrt(3) * dx / 2;
        Vector3 pos = Vector3.zero;


        for (int i = 0; i < 10; ++i)
        {
            GameObject d = DecorationInstancer.Create(Decoration.DecorationType.NODE_STYLE, i);
            d.GetComponent<MeshRenderer>().material = materials[mid[i] - 1];
            d.transform.localRotation = d.transform.localRotation * Quaternion.Euler(new Vector3(0, 0, rot[i]));

            d.transform.parent = gameObject.transform;
            d.transform.localPosition = new Vector3(pos.x, ((i & 1) == 0) ? pos.y : (pos.y + dy), pos.z);
            d.layer = gameObject.layer;
            pos.x += dx;
        }
    }
    void Start()
    {
        //Create();
        CreateHexagon(0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
