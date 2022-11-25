using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_SpotLight : MonoBehaviour
{
    Light light;
    SpotLightInformation lightInfo;

    public float innerAngle;
    public float outerAngle;
    public Color color;
    public float intensity;
    public float range;
    public Vector3 lightPos;
    public Vector3 lightRot;
    public Vector3 lightScale;

    // Start is called before the first frame update
    void Start()
    {
        lightInfo = new SpotLightInformation();
        JM_LightManager.instance.spotLightInfoList.Add(lightInfo);

        light = transform.GetComponent<Light>();
        lightInfo.innerAngle = light.innerSpotAngle;
        lightInfo.outerAngle = light.spotAngle;
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
        light.innerSpotAngle = innerAngle;
        light.spotAngle = outerAngle;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        lightPos = light.transform.position;
        lightRot = light.transform.eulerAngles;
        lightScale = light.transform.localScale;

        lightInfo.innerAngle = innerAngle;
        lightInfo.outerAngle = outerAngle;
        lightInfo.color = color;
        lightInfo.intensity = intensity;
        lightInfo.range = range;
        lightInfo.lightPos = lightPos;
        lightInfo.lightRot = lightRot;
        lightInfo.lightScale = lightScale;
    }

    public void Sync()
    {
        JM_LightManager.instance.innerAngSlider.value = innerAngle;
        JM_LightManager.instance.outerAngSlider.value = outerAngle;
        JM_LightManager.instance.intensitySlider.value = intensity;
        JM_LightManager.instance.rangeSlider.value = range;
        JM_LightManager.instance.sptColorPicker.color = color;
    }

    public void UpdateSptLightInfo(int i)
    {
        //print(JsonUtility.ToJson(lightInfo));
        JM_LightManager.instance.spotLightInfoList[i] = lightInfo;
    }

}

[System.Serializable]
public class SpotLightInformation
{
    public float innerAngle;
    public float outerAngle;
    public Color color;
    public float intensity;
    public float range;
    public Vector3 lightPos;
    public Vector3 lightRot;
    public Vector3 lightScale;
}
