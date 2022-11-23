using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettingManager : MonoBehaviour
{
    public static LightSettingManager instance;
    private void Awake()
    {
        instance = this;
    }

    public float intensityLight;
    public float rangeLight;
    public float innerAngleLight;
    public float outerAngleLight;

    public float r;
    public float g;
    public float b;
    public float a;

    Light light;
    GameObject Lights;
    GameObject pointLight;

    [SerializeField]
    Color colorLight = new Color();

    void Start()
    {
        light = gameObject.GetComponent<Light>();
        colorLight = Color.red;
        intensityLight = 10;
        rangeLight = 10;
        innerAngleLight = 10;
        outerAngleLight = 50;
    }

    void Update()
    {
        //r = colorLight.r;
        //g = colorLight.g;
        //b = colorLight.b;
        //a = colorLight.a;
        //print(r + "," + g + "," + b + "," + a);
        //colorLight = light.color;

        colorLight.r = r;
        colorLight.g = g;
        colorLight.b = b;
        colorLight.a = a;

        light.intensity = intensityLight;
        light.range = rangeLight;
        light.innerSpotAngle = innerAngleLight;
        light.spotAngle = outerAngleLight;

        light.color = colorLight;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeColor();
        }
    }

    void OnGUI()
    {
        // 라이트 키기 끄기
        if (GUI.Button(new Rect(10, 10, 100, 30), "Light On/Off"))
        {

            bool light_chk = !light.enabled;

            light.enabled = light_chk;

        }

        // 라이트 종류 바꾸기
        if (GUI.Button(new Rect(10, 50, 100, 30), "Change"))
        {

            if (light.type == LightType.Spot)
            {
                light.type = LightType.Point;
            }
            else light.type = LightType.Spot;

        }
    }

    public void ChangeColor(/*float r, float g, float b, float a*/)
    {
        Color newColor = Color.yellow;
        colorLight = newColor;
    }
}
