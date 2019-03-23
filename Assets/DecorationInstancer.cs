using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// singleton for creating decorations
// used by the game and also by GameSerializer
public class DecorationInstancer : MonoBehaviour
{
    static DecorationInstancer instance;

    public GameObject decor_style_prefab;

    DecorationInstancer()
    {
        instance = this;
    }


    public static GameObject Create(Decoration.DecorationType type, int id)
    {
        GameObject g = Instantiate(instance.decor_style_prefab);
        Decoration dec = g.GetComponent<Decoration>();
        dec.id = id;
        dec.type = type;

        return g;
    }
}
