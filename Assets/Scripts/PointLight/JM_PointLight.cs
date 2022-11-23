using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PointLight : MonoBehaviour
{
    Light light;
    LightInformation lightInfo;

    public float innerAngle;
    public float outerAngle;
    public Color color;
    public float intensity;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        lightInfo = new LightInformation();
        JM_PointLightManager.instance.lightInfoList.Add(lightInfo);

        light = transform.GetComponent<Light>();
        lightInfo.innerAngle = light.innerSpotAngle;
        lightInfo.outerAngle = light.spotAngle;
        lightInfo.color = light.color;
        lightInfo.intensity = light.intensity;
        lightInfo.range = light.range;

        JM_PointLightManager.instance.innerAngSlider.value = innerAngle;
        JM_PointLightManager.instance.outerAngSlider.value = outerAngle;
        JM_PointLightManager.instance.intensitySlider.value = intensity;
        JM_PointLightManager.instance.rangeSlider.value = range;

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

        lightInfo.innerAngle = innerAngle;
        lightInfo.outerAngle = outerAngle;
        lightInfo.color = color;
        lightInfo.intensity = intensity;
        lightInfo.range = range;
    }

}

public class LightInformation
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
