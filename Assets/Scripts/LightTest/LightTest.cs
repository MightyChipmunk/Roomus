using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightTest : MonoBehaviour
{
    [SerializeField]
    Color skyColor = new Color();
    [SerializeField]
    Color equatorColor = new Color();
    [SerializeField]
    Color groundColor = new Color();
    public float intensityMultiplier;

    public FlexibleColorPicker skyColorPicker;
    public FlexibleColorPicker equatorColorPicker;
    public FlexibleColorPicker groundColorPicker;

    public Slider intensitySlider;

    // Start is called before the first frame update
    void Start()
    {
        skyColorPicker.onColorChange.AddListener(UpdateSkyColor);
        equatorColorPicker.onColorChange.AddListener(UpdateEquatorColor);
        groundColorPicker.onColorChange.AddListener(UpdateGroundColor);
        intensitySlider.maxValue = 1;            
    }

    // Update is called once per frame
    void Update()
    {
        skyColor = RenderSettings.ambientSkyColor;
        equatorColor = RenderSettings.ambientEquatorColor;
        groundColor = RenderSettings.ambientGroundColor;
        intensityMultiplier = RenderSettings.reflectionIntensity;
    }

    public void UpdateSkyColor(Color c)
    {
        skyColor = c;
        RenderSettings.ambientSkyColor = c;
    }

    public void UpdateEquatorColor(Color c)
    {
        equatorColor = c;
        RenderSettings.ambientEquatorColor = c;
    }

    public void UpdateGroundColor(Color c)
    {
        groundColor = c;
        RenderSettings.ambientGroundColor = c;
    }

    public void UpdateIntensity()
    {
        intensitySlider.value = intensityMultiplier;
    }

}
