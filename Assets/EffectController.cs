using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;




public class EffectController : MonoBehaviour
{
    public GameObject main_volume;
    public GameObject deco_volume;
    public GameObject menu_volume;

    static EffectController instance;

    public class AnimValue
    {
        public float value;
        float from;
        float to;

        float time;
        float total;

        public AnimValue(float from, float to, float total)
        {
            this.from = from;
            this.to = to;
            this.time = 0;
            this.total = total;
            value = from;
        }

        public void Update(float dt)
        {
            time += dt;
            time = time > total ? total : time;

            float t = time / total;
            value = from * (1 - t) + to * t;
        }

        public bool IsDone()
        {
            return time >= total;
        }
    }

    AnimValue transition;
    DepthOfField parameters;
    Bloom menu_bloom;
    DepthOfField deco_blur;

    Node active_node;
    Node next_node;

    public static EffectController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        var vol = main_volume.GetComponent<PostProcessVolume>();
        parameters = vol.profile.GetSetting<DepthOfField>();

        var mvol = menu_volume.GetComponent<PostProcessVolume>();
        menu_bloom = mvol.profile.GetSetting<Bloom>();

        var dvol = deco_volume.GetComponent<PostProcessVolume>();
        deco_blur = dvol.profile.GetSetting<DepthOfField>();
    }

    private void Update()
    {
        if (transition != null)
        {
            transition.Update(Time.deltaTime);
            parameters.focalLength.value = transition.value;
            if (transition.IsDone())
                transition = null;
            
        }

        if (next_node != null && active_node != next_node)
        {
            if (active_node != null)
                active_node.gameObject.transform.Find("bg_fx").gameObject.SetActive(false);
            active_node = next_node;

            active_node.gameObject.transform.Find("bg_fx").gameObject.SetActive(true);
            next_node = null;
        }
    }

    public void OnDecorationDrag()
    {
        transition = new AnimValue(parameters.focalLength.value, 64, 0.3f);
    }

    public void OnEnterMenu()
    {
        menu_bloom.active = true;
        deco_blur.active = true;
    }

    public void Clear()
    {
        transition = new AnimValue(parameters.focalLength.value, 1, 0.15f);
        menu_bloom.active = false;
        deco_blur.active = false;
    }

    public void OnActivateNode(Node node)
    {
        // this function is called outside main thread
        // thus the logic which handles game objects was moved to Update()
        next_node = node;
    }
}
