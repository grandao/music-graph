using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class EffectController : MonoBehaviour
{
    public GameObject main_volume;
    static EffectController instance;

    public static EffectController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
    }

    public void OnDecorationDrag()
    {
        var vol = main_volume.GetComponent<PostProcessVolume>();
        var conf = vol.profile.GetSetting<DepthOfField>();
        conf.focalLength.value = 64;
    }

    public void Clear()
    {
        var vol = main_volume.GetComponent<PostProcessVolume>();
        var conf = vol.profile.GetSetting<DepthOfField>();
        conf.focalLength.value = 1;
    }

}
