using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Node : MonoBehaviour
{
    int note;
    float duration;



    // Start is called before the first frame update
    void Start()
    {
        /*
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-1, -1);
        vertices[1] = new Vector3(-1, 1);
        vertices[2] = new Vector3(1, 1);
        vertices[3] = new Vector3(1, -1);

        int[] triangles = new int[6] {0, 2, 1, 0, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
