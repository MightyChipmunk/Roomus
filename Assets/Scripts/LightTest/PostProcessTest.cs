using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessTest : MonoBehaviour
{
    public GameObject go;
    Volume gameVolume;
    ShadowsMidtonesHighlights light;
    ColorAdjustments adj;
    WhiteBalance wb;
    Vector4Parameter test1;
    Vector4Parameter test2;
    Vector4Parameter test3;
    float shadowR;
    float shadowG;
    float shadowB;
    Vector4Parameter shadowColor;
    Vector4Parameter midtoneColor;
    Vector4Parameter highlightColor;

    public FlexibleColorPicker colorPicker;
    
    // Start is called before the first frame update
    void Start()
    {
        gameVolume = go.GetComponent<Volume>();
        ShadowsMidtonesHighlights demo;
        if (gameVolume.profile.TryGet<ShadowsMidtonesHighlights>(out demo))
        {
            light = demo;
        }
        ColorAdjustments demo2;
        colorPicker.onColorChange.AddListener(UpdateShadowColor);
        if (gameVolume.profile.TryGet<ColorAdjustments>(out demo2))
        {
            adj = demo2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        test1 = light.shadows;
        test2 = light.midtones;
        test3 = light.highlights;
        print(test1);
        print(test2);
        print(test3);
    }

    public void UpdateShadowColor(Color c)
    {
        shadowR = c.r;
        shadowG = c.g;
        shadowB = c.b;
        Vector4 testVal = new Vector4(shadowR, shadowG, shadowB, 0);
        shadowColor = new Vector4Parameter(testVal, true);
        light.shadows.Override(testVal);
    }
}
