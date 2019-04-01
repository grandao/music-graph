using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientPallete
{
    static Color purple = new Color(0.53f, 0, 1);
    static Color dark_blue = new Color(0.07f, 0.07f, 0.24f);
    static Color cyan = new Color(0, 0.95f, 1);

    static Color red = new Color(1, 0.2f, 0);
    static Color yellow = new Color(1, 0.98f, 0);

    // default gradient to be used
    public static Gradient GetDefault()
    {
        Gradient ret = new Gradient();
        ret.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.2f), new GradientColorKey(Color.gray, 0.65f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(1, 0.20f), new GradientAlphaKey(1, 0.7f), new GradientAlphaKey(0, 1.0f) }
        );

        return ret;
    }

    // cold colors gradient
    public static Gradient GetCold()
    {
        Gradient ret = new Gradient();
        ret.SetKeys(
            new GradientColorKey[] { new GradientColorKey(purple, 0.2f), new GradientColorKey(dark_blue, 0.5f), new GradientColorKey(cyan, 0.65f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(1, 0.20f), new GradientAlphaKey(1, 0.7f), new GradientAlphaKey(0, 1.0f) }
        );

        return ret;
    }

    // hot colors gradient
    public static Gradient GetHot()
    {
        Gradient ret = new Gradient();
        ret.SetKeys(
            new GradientColorKey[] { new GradientColorKey(red, 0.2f), new GradientColorKey(yellow, 0.65f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(1, 0.20f), new GradientAlphaKey(1, 0.7f), new GradientAlphaKey(0, 1.0f) }
        );

        return ret;
    }
}
