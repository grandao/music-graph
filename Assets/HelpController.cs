using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    public GameObject helpPlane;
    public GameObject helpText;
    public Material[] materials;
    public string[] texts;
    bool active = false;
    int current = 0;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Enable()
    {
        current = 0;
        Set();

        active = true;
        helpPlane.SetActive(true);
        helpText.SetActive(true);  
    }

    public void Disable()
    {
        active = false;
        helpPlane.SetActive(false);
        helpText.SetActive(false);
    }

    public void Next()
    {
        ++current;
        if (current >= materials.Length)
        {
            current = 0;
            Disable();
        }
        Set();
    }

    void Set()
    {
        helpPlane.GetComponent<MeshRenderer>().material = materials[current];
        helpText.GetComponent<TMPro.TextMeshPro>().text = texts[current];
    }

}
