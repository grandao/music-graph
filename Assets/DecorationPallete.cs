using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationPallete : MonoBehaviour
{
    public GameObject decoration_prefab;

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
    void Start()
    {
        Create();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
