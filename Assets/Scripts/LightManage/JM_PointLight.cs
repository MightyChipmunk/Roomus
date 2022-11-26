using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PointLight : MonoBehaviour
{
    Light light;
    PointLightInformation lightInfo;

    public Color color;
    public float intensity;
    public float range;
    public Vector3 lightPos;
    public Vector3 lightRot;
    public Vector3 lightScale;

    // Start is called before the first frame update
    void Start()
    {
        lightInfo = new PointLightInformation();
        JM_LightManager.instance.pointLightInfoList.Add(lightInfo);

        light = transform.GetComponent<Light>();
        lightInfo.color = light.color;
        lightInfo.intensity = light.intensity;
        lightInfo.range = light.range;

        Sync();

        lightInfo.lightPos = transform.position;
        lightInfo.lightRot = transform.eulerAngles;
        lightInfo.lightScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        light.color = color;
        light.intensity = intensity;
        light.range = range;
        lightPos = light.transform.position;
        lightRot = light.transform.eulerAngles;
        lightScale = light.transform.localScale;

        lightInfo.color = color;
        lightInfo.intensity = intensity;
        lightInfo.range = range;
        lightInfo.lightPos = lightPos;
        lightInfo.lightRot = lightRot;
        lightInfo.lightScale = lightScale;
    }

    public void Sync()
    {
        JM_LightManager.instance.ptIntensitySlider.value = intensity;
        JM_LightManager.instance.ptRangeSlider.value = range;
        JM_LightManager.instance.ptColorPicker.color = color;
    }

    public void UpdatePtLightInfo(int i)
    {
        JM_LightManager.instance.pointLightInfoList[i] = lightInfo;
    }
}

[System.Serializable]
public class PointLightInformation
{  
    public Color color;
    public float intensity;
    public float range;
    public Vector3 lightPos;
    public Vector3 lightRot;
    public Vector3 lightScale;
}
