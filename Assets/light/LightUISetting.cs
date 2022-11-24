using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightUISetting : MonoBehaviour
{
    public GameObject refLight;

    
    [SerializeField]
    GameObject pointLight;
    Light light;

    public FlexibleColorPicker lightColorPicker;
    public Slider intensityLightSlider;
    public Slider rangeLightSlider;
    public Slider innerAngleLightSlider;
    public Slider outerAngleLightSlider;
    RaycastHit hit;


    //[SerializeField]
    Color colorLight = new Color();

    void Start()
    {
        rangeLightSlider.maxValue = 8;
        intensityLightSlider.maxValue = 10;
        innerAngleLightSlider.maxValue = 100;
        outerAngleLightSlider.maxValue = 100;

        rangeLightSlider.value = 8f;
        intensityLightSlider.value = 10f;
        outerAngleLightSlider.value = 50;
        innerAngleLightSlider.value = 10;
        //RTGApp.SetActive(false);

        lightColorPicker.onColorChange.AddListener(UpdatePickerColor);
        lightColorPicker.color = Color.white;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
        }

        UpdateLightColor();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        //    {
        //        if (hit.collider.gameObject.name.Contains("Light") || hit.collider.gameObject.tag.Contains("UI"))
        //        {
        //            RTGApp.SetActive(true);
        //            //LightUI.SetActive(true);
        //        }
        //        else
        //        {
        //            RTGApp.SetActive(false);
        //            //LightUI.SetActive(false);
        //        }
        //    }
        //}

    }

    public void UpdatePickerColor(Color c)
    {
        if (lightColorPicker.color == c) return;
        lightColorPicker.color = c;
        print(c);
    }

    public void UpdateLightColor()
    {
        if (!pointLight) return;
        LightSettingManager.instance.r = lightColorPicker.color.r;
        LightSettingManager.instance.g = lightColorPicker.color.g;
        LightSettingManager.instance.b = lightColorPicker.color.b;
        LightSettingManager.instance.a = lightColorPicker.color.a;
    }

    public void UpdateLightIntensity()
    {
        LightSettingManager.instance.intensityLight = intensityLightSlider.value;
    }
    public void UpdateLightRange()
    {
        LightSettingManager.instance.rangeLight = rangeLightSlider.value;
    }

    public void UpdateLightInnerAngle()
    {

        LightSettingManager.instance.innerAngleLight = innerAngleLightSlider.value;
    }

    public void UpdateLightOuterAngle()
    {

        LightSettingManager.instance.outerAngleLight = outerAngleLightSlider.value;
    }

    public void OnClickBtn()
    {
        GenerateLight();
    }
    void GenerateLight()
    {
        GameObject light = Instantiate(refLight);
        pointLight = refLight;
    }

}
