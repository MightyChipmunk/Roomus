using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class JM_FilterManager : MonoBehaviour
{
    public static JM_FilterManager instance;

    public GameObject go;
    Volume volume;
    ShadowsMidtonesHighlights smh;
    ColorAdjustments ca;
    WhiteBalance wb;

    /*
    public Vector4 shadowVal;
    public Vector4 midtoneVal;
    public Vector4 highlightVal;
    public float contrast;
    public float postExposure;
    public float hueShift;
    public float saturation;
    public float temp;
    public float tint;
    public Color colorFilter;
    */

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        volume = go.GetComponent<Volume>();
        ShadowsMidtonesHighlights demo;
        if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out demo))
        {
            smh = demo;
        }
        ColorAdjustments demo2;
        //colorPicker.onColorChange.AddListener(UpdateShadowColor);
        if (volume.profile.TryGet<ColorAdjustments>(out demo2))
        {
            ca = demo2;
        }
        WhiteBalance demo3;
        if (volume.profile.TryGet<WhiteBalance>(out demo3))
        {
            wb = demo3;
        }

        SetRoomFilter(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1), 0, 0, 0, 0, Color.green, 0, 0);


    }

    float currentTime;

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Click()
    {

    }

    public void SetRoomFilter(Vector4 shadow, Vector4 midtone, Vector4 highlight, float contrast,
        float postExposure, float hue, float saturation, Color color, float temp, float tint)
    {
        // set shadows midtones highlights value
        smh.shadows.Override(shadow);
        smh.midtones.Override(midtone);
        smh.highlights.Override(highlight);

        // set color adjustment
        ca.contrast.Override(contrast);
        ca.postExposure.Override(postExposure);
        ca.hueShift.Override(hue);
        ca.saturation.Override(saturation);
        ca.colorFilter.Override(color);

        // set white balance
        wb.temperature.Override(temp);
        wb.tint.Override(tint);
    }
}
