using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JM_PointLightManager : MonoBehaviour
{
    public static JM_PointLightManager instance;

    public Transform lightContent;
    public Button lightBtn;
    [SerializeField]
    List<Button> lightBtnList = new List<Button>();

    public GameObject lightFactory;

    public GameObject light;
    [SerializeField]
    JM_PointLight lightCode;

    [SerializeField]
    public List<GameObject> lightList = new List<GameObject>();
    public List<LightInformation> lightInfoList = new List<LightInformation>();

    public Slider innerAngSlider;
    public Slider outerAngSlider;
    public FlexibleColorPicker colorPicker;
    public Slider intensitySlider;
    public Slider rangeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        innerAngSlider.maxValue = 180;
        outerAngSlider.maxValue = 180;
        intensitySlider.maxValue = 50;
        rangeSlider.maxValue = 50;

        colorPicker.onColorChange.AddListener(UpdateColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (lightInfoList.Count > 0)
            print(lightInfoList[0].intensity);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit, 1000, 6))
            {
                light = hit.transform.gameObject;
                lightCode = light.GetComponent<JM_PointLight>();
            }
        }
    }

    public void OnInnerAngValueChanged()
    {
        lightCode.innerAngle = innerAngSlider.value;
    }

    public void OnOuterAngValueChanged()
    {
        lightCode.outerAngle = outerAngSlider.value;
    }

    public void UpdateColor(Color c)
    {
        lightCode.color = c;
    }

    public void OnIntensityValueChanged()
    {
        lightCode.intensity = intensitySlider.value;
    }

    public void OnRangeValueChanged()
    {
        lightCode.range = rangeSlider.value;
    }

    // Instantiating light
    public void OnClickNewLight()
    {
        GameObject newLight = Instantiate(lightFactory);
        Button btn = Instantiate(lightBtn, lightContent);
        btn.name = lightBtnList.Count.ToString();
        lightBtnList.Add(btn);
        LightInformation lightInfo = new LightInformation();
        lightList.Add(newLight);
        newLight.transform.position = new Vector3(0, 2.5f, 0);
        lightCode = newLight.GetComponent<JM_PointLight>();

        lightCode.innerAngle = 90;
        lightCode.outerAngle = 90;
        lightCode.color = colorPicker.startingColor;
        lightCode.intensity = 5;
        lightCode.range = 25;

        /*
        innerAngSlider.value = 90;
        outerAngSlider.value = 90;
        lightCode.color = colorPicker.startingColor;
        intensitySlider.value = 5;
        rangeSlider.value = 25;
        */
    }

    public void OnClickLightBtn()
    {
        for (int i = 0; i < lightBtnList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == lightBtnList[i].name)
            {
                print(i);
            }
        }
    }
}


