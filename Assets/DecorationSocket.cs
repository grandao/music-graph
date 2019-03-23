using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSocket : MonoBehaviour
{
    public Decoration.DecorationType type = Decoration.DecorationType.NONE;
    public int id = -1;
    Decoration dec;

    public int GetId()
    {
        if (dec != null) return dec.id;
        return -1;
    }

    public bool Set(Decoration d)
    {
        if (type != d.type) return false;

        if (dec != null)
            Destroy(dec.gameObject);

        dec = d;
        dec.gameObject.transform.parent = this.gameObject.transform;
        dec.gameObject.transform.localPosition = Vector3.zero;

        return true;
    }

    public bool Set(int id)
    {
        if (id > 0)
            return Set(DecorationInstancer.Create(type, id).GetComponent<Decoration>());
        return false;
    }
}
