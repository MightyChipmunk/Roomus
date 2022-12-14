using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System;

public class PostProcessTest : MonoBehaviour
{
    public static PostProcessTest Instance;

    public GameObject go;
    Volume gameVolume;
    
    ColorAdjustments adj;
    WhiteBalance wb;
    Vector4Parameter test1;
    Vector4Parameter test2;
    Vector4Parameter test3;
    
    Vector4Parameter shadowColor;
    Vector4Parameter midtoneColor;
    Vector4Parameter highlightColor;

    Vector4 idleColor;

    ShadowsMidtonesHighlights smh;
    public GameObject shadowPicker;
    JM_ColorPicker shadowColorPickCode;
    float shadowR;
    float shadowG;
    float shadowB;
    float shadowGamma;
    public Slider shadowSlider;

    public GameObject midtonePicker;
    JM_ColorPicker midtonePickCode;
    float midtoneR;
    float midtoneG;
    float midtoneB;
    public Slider midtoneSlider;
    float midtoneGamma;

    public GameObject highlightPicker;
    JM_ColorPicker highlightPickCode;
    float highlightR;
    float highlightG;
    float highlightB;
    public Slider highlightSlider;
    float highlightGamma;

    ColorAdjustments ca;
    public Slider postExposureSlider;
    public Text peTxt;
    public Slider contrastSlider;
    public Text contTxt;
    public Slider hueShiftSlider;
    public Text hueTxt;
    public Slider saturationSlider;
    public Text satTxt;
    public GameObject colorFilterPicker;
    public Button colorFilterBtn;
    FlexibleColorPicker colorPicker;

    WhiteBalance wbe;
    public Slider tempSlider;
    public Text tempTxt;
    public Slider tintSlider;
    public Text tintTxt;

    Vector4 shadowVal;
    Vector4 midtoneVal;
    Vector4 highlightVal;
    float contrast;
    float postExposure;
    float hueShift;
    float saturation;
    float temp;
    float tint;
    Color colorFilter;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameVolume = go.GetComponent<Volume>();
        ShadowsMidtonesHighlights demo;
        if (gameVolume.profile.TryGet<ShadowsMidtonesHighlights>(out demo))
        {
            smh = demo;
        }
        ColorAdjustments demo2;
        //colorPicker.onColorChange.AddListener(UpdateShadowColor);
        if (gameVolume.profile.TryGet<ColorAdjustments>(out demo2))
        {
            ca = demo2;
        }
        WhiteBalance demo3;
        if (gameVolume.profile.TryGet<WhiteBalance>(out demo3))
        {
            wbe = demo3;
        }

        // new colorpicker test
        shadowColorPickCode = shadowPicker.GetComponent<JM_ColorPicker>();
        midtonePickCode = midtonePicker.GetComponent<JM_ColorPicker>();
        highlightPickCode = highlightPicker.GetComponent<JM_ColorPicker>();

        // color adjustment
        postExposure = (float)ca.postExposure;
        contrast = (float)ca.contrast;
        hueShift = (float)ca.hueShift;
        saturation = (float)ca.saturation;
        colorPicker = colorFilterPicker.GetComponent<FlexibleColorPicker>();
        colorFilterBtn.GetComponent<Image>().color = (Color)ca.colorFilter;

        colorPicker.onColorChange.AddListener(UpdateColor);

        // white balance
        temp = (float)wbe.temperature;
        tint = (float)wbe.tint;

        //SetRoomFilter(shadowVal, midtoneVal, highlightVal, contrast, postExposure, hueShift, saturation, colorFilter, temp, tint);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShadow();
        UpdateMidtone();
        UpdateHighlight();
        UpdatePostExposure();
        UpdateContrast();
        UpdateHue();
        UpdateSaturation();
        UpdateTemp();
        UpdateTint();

        Deco_Json.Instance.advLightInfo.shadowVal = shadowVal;
        Deco_Json.Instance.advLightInfo.midtoneVal = midtoneVal;
        Deco_Json.Instance.advLightInfo.highlightVal = highlightVal;
        Deco_Json.Instance.advLightInfo.postExposure = postExposure;
        Deco_Json.Instance.advLightInfo.contrast = contrast;
        Deco_Json.Instance.advLightInfo.hueShift = hueShift;
        Deco_Json.Instance.advLightInfo.saturation = saturation;
        Deco_Json.Instance.advLightInfo.temp = temp;
        Deco_Json.Instance.advLightInfo.tint = tint;
        Deco_Json.Instance.advLightInfo.color = colorFilter;
    }

    public void UpdateShadow()
    {
        shadowR = shadowColorPickCode.selectColor.r;
        shadowG = shadowColorPickCode.selectColor.g;
        shadowB = shadowColorPickCode.selectColor.b;
        shadowGamma = shadowSlider.value;
        shadowVal = new Vector4(shadowR, shadowG, shadowB, shadowGamma);
        smh.shadows.Override(shadowVal);
    }

    public void UpdateMidtone()
    {
        midtoneR = midtonePickCode.selectColor.r;
        midtoneG = midtonePickCode.selectColor.g;
        midtoneB = midtonePickCode.selectColor.b;
        midtoneGamma = midtoneSlider.value;
        midtoneVal = new Vector4(midtoneR, midtoneG, midtoneB, midtoneGamma);
        smh.midtones.Override(midtoneVal);
    }

    public void UpdateHighlight()
    {
        highlightR = highlightPickCode.selectColor.r;
        highlightG = highlightPickCode.selectColor.g;
        highlightB = highlightPickCode.selectColor.b;
        highlightGamma = highlightSlider.value;
        highlightVal = new Vector4(highlightR, highlightG, highlightB, highlightGamma);
        smh.highlights.Override(highlightVal);
    }

    public void UpdateShadowColor(Color c)
    {
        shadowR = c.r;
        shadowG = c.g;
        shadowB = c.b;
        
        Vector4 testVal = new Vector4(shadowR, shadowG, shadowB, 0);
        shadowColor = new Vector4Parameter(testVal, true);
        smh.shadows.Override(testVal);
    }

    public void UpdatePostExposure()
    {
        postExposure = (float)(Math.Truncate(postExposureSlider.value * 100) / 100);
        ca.postExposure.Override(postExposure);
        peTxt.GetComponent<Text>().text = postExposure.ToString();
    }

    public void UpdateContrast()
    {
        contrast = contrastSlider.value;
        ca.contrast.Override(contrast);
        contTxt.GetComponent<Text>().text = contrast.ToString();
    }

    public void UpdateHue()
    {
        hueShift = hueShiftSlider.value;
        ca.hueShift.Override(hueShift);
        hueTxt.GetComponent<Text>().text = hueShift.ToString();
    }

    public void UpdateSaturation()
    {
        saturation = saturationSlider.value;
        ca.saturation.Override(saturation);
        satTxt.GetComponent<Text>().text = saturation.ToString();
    }

    public void OnClickColor()
    {
        colorFilterPicker.SetActive(true);
    }

   public void UpdateColor(Color c)
    {
        colorFilter = c;
        colorFilterBtn.GetComponent<Image>().color = colorFilter;
        ca.colorFilter.Override(colorFilter);
        colorPicker.startingColor = colorFilter;
    }

    public void OnClickDone()
    {
        colorFilterPicker.SetActive(false);
    }
    public void OnClickReset()
    {
        ca.colorFilter.Override(Color.white);
        colorPicker.color = Color.white;
    }

    public void UpdateTemp()
    {
        temp = tempSlider.value;
        wbe.temperature.Override(temp);
        tempTxt.GetComponent<Text>().text = temp.ToString();
    }

    public void UpdateTint()
    {
        tint = tintSlider.value;
        wbe.tint.Override(tint);
        tintTxt.GetComponent<Text>().text = tint.ToString();
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
        //wb.temperature.Override(temp);
        //wb.tint.Override(tint);

        shadowColorPickCode.selectColor.r = shadow.x;
        shadowColorPickCode.selectColor.g = shadow.y;
        shadowColorPickCode.selectColor.b = shadow.z;
        shadowSlider.value = shadow.w;

        midtonePickCode.selectColor.r = midtone.x;
        midtonePickCode.selectColor.g = midtone.y;
        midtonePickCode.selectColor.b = midtone.z;
        midtoneSlider.value = midtone.w;

        highlightPickCode.selectColor.r = highlight.x;
        highlightPickCode.selectColor.g = highlight.y;
        highlightPickCode.selectColor.b = highlight.z;
        highlightSlider.value = highlight.w;

        postExposureSlider.value = postExposure;
        contrastSlider.value = contrast;
        hueShiftSlider.value = hueShift;
        saturationSlider.value = saturation;

        colorFilter = color;
        colorFilterBtn.GetComponent<Image>().color = colorFilter;
        colorPicker.startingColor = colorFilter;

        tempSlider.value = temp;
        tintSlider.value = tint;
    }
}
