using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class JM_LightManager : MonoBehaviour
{
    public static JM_LightManager instance;

    public GameObject activeLight;

    public Transform sptLightContent;
    public Button sptLightBtn;
    [SerializeField]
    List<Button> sptLightBtnList = new List<Button>();

    public Transform ptLightContent;
    public Button ptLightBtn;
    List<Button> ptLightBtnList = new List<Button>();

    public GameObject spotLightFactory;
    public GameObject pointLightFactory;

    public GameObject spotLight;

    public JM_PointLight pointLightCode;
    public JM_SpotLight spotLightCode;

    [SerializeField]
    public List<GameObject> spotLightList = new List<GameObject>();
    public List<SpotLightInformation> spotLightInfoList = new List<SpotLightInformation>();

    public Slider innerAngSlider;
    public Slider outerAngSlider;
    public FlexibleColorPicker sptColorPicker;
    public Slider intensitySlider;
    public Slider rangeSlider;

    public Text innerAngContent;
    public Text outerAngContent;
    public Text intensityContent;
    public Text rangeContent;

    public List<GameObject> pointLightList = new List<GameObject>();
    public List<PointLightInformation> pointLightInfoList = new List<PointLightInformation>();

    public FlexibleColorPicker ptColorPicker;
    public Slider ptIntensitySlider;
    public Slider ptRangeSlider;

    public Text ptIntensityContent;
    public Text ptRangeContent;

    public RTG.RuntimeGizmo gizmoManager;
    bool isRotate;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        innerAngSlider.maxValue = 180;
        outerAngSlider.maxValue = 180;
        intensitySlider.maxValue = 50;
        rangeSlider.maxValue = 50;
        ptIntensitySlider.maxValue = 50;
        ptRangeSlider.maxValue = 50;

        sptColorPicker.onColorChange.AddListener(UpdateSptColor);
        ptColorPicker.onColorChange.AddListener(UpdatePtColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isRotate)
            {
                gizmoManager.GizmoRotateON();
                isRotate = true;
            }
            else
            {
                gizmoManager.GizmoMoveON();
                isRotate = false;
            }

        }
    }

    public void OnInnerAngValueChanged()
    {
        spotLightCode.innerAngle = innerAngSlider.value;
        innerAngContent.GetComponent<Text>().text = innerAngSlider.value.ToString();
    }

    public void OnOuterAngValueChanged()
    {
        spotLightCode.outerAngle = outerAngSlider.value;
        outerAngContent.GetComponent<Text>().text = outerAngSlider.value.ToString();
    }

    public void UpdateSptColor(Color c)
    {
        spotLightCode.color = c;
    }

    public void OnIntensityValueChanged()
    {
        spotLightCode.intensity = intensitySlider.value;
        intensityContent.GetComponent<Text>().text = intensitySlider.value.ToString();
    }

    public void OnRangeValueChanged()
    {
        spotLightCode.range = rangeSlider.value;
        rangeContent.GetComponent<Text>().text = rangeSlider.value.ToString();
    }

    public void OnPtIntensityValueChanged()
    {
        pointLightCode.intensity = ptIntensitySlider.value;
        ptIntensityContent.GetComponent<Text>().text = ptIntensitySlider.value.ToString();
    }

    public void OnPtRangeValueChanged()
    {
        pointLightCode.range = ptRangeSlider.value;
        ptRangeContent.GetComponent<Text>().text = ptRangeSlider.value.ToString();
    }

    public void UpdatePtColor(Color c)
    {
        pointLightCode.color = c;
    }

    // new spot light
    public void OnClickNewSptLightBtn()
    {
        GameObject newLight = Instantiate(spotLightFactory);
        activeLight = newLight;

        gizmoManager.targetObject = newLight;
        gizmoManager.isMove = true;

        Button btn = Instantiate(sptLightBtn, sptLightContent);
        sptLightBtnList.Add(btn);

        spotLightList.Add(newLight);
        newLight.transform.position = new Vector3(0, 2.5f, 0);
        spotLightCode = newLight.GetComponent<JM_SpotLight>();

        spotLightCode.innerAngle = 90;
        spotLightCode.outerAngle = 90;
        spotLightCode.color = Color.white;
        spotLightCode.intensity = 5;
        spotLightCode.range = 25;
    }

    public void LoadSptLight(LightInfo info)
    {
        GameObject newLight = Instantiate(spotLightFactory);
        activeLight = newLight;

        gizmoManager.targetObject = newLight;
        gizmoManager.isMove = true;

        Button btn = Instantiate(sptLightBtn, sptLightContent);
        sptLightBtnList.Add(btn);

        spotLightList.Add(newLight);
        spotLightCode = newLight.GetComponent<JM_SpotLight>();

        newLight.transform.position = info.position;
        newLight.transform.eulerAngles = info.eulerAngle;
        newLight.transform.localScale = info.localScale;
        spotLightCode.innerAngle = info.innerAngle;
        spotLightCode.outerAngle = info.outerAngle;
        spotLightCode.color = info.color;
        spotLightCode.intensity = info.intensity;
        spotLightCode.range = info.range;
    }

    // new point light
    public void OnClickNewPtLightBtn()
    {
        GameObject newLight = Instantiate(pointLightFactory);
        activeLight = newLight;

        gizmoManager.targetObject = newLight;
        gizmoManager.isMove = true;

        Button btn = Instantiate(ptLightBtn, ptLightContent);
        ptLightBtnList.Add(btn);

        pointLightList.Add(newLight);
        newLight.transform.position = new Vector3(0, 2.5f, 0);
        pointLightCode = newLight.GetComponent<JM_PointLight>();

        pointLightCode.color = Color.white;
        pointLightCode.intensity = 1;
        pointLightCode.range = 10;
    }

    public void LoadPtLight(LightInfo info)
    {
        GameObject newLight = Instantiate(pointLightFactory);
        activeLight = newLight;

        gizmoManager.targetObject = newLight;
        gizmoManager.isMove = true;

        Button btn = Instantiate(ptLightBtn, ptLightContent);
        ptLightBtnList.Add(btn);

        pointLightList.Add(newLight);
        pointLightCode = newLight.GetComponent<JM_PointLight>();

        newLight.transform.position = info.position;
        newLight.transform.eulerAngles = info.eulerAngle;
        newLight.transform.localScale = info.localScale;
        //spotLightCode.innerAngle = info.innerAngle;
        //spotLightCode.outerAngle = info.outerAngle;
        pointLightCode.color = info.color;
        pointLightCode.intensity = info.intensity;
        pointLightCode.range = info.range;
    }

    public void OnClickDupSptLightBtn()
    {
        float dupInner = activeLight.GetComponent<JM_SpotLight>().innerAngle;
        float dupOuter = activeLight.GetComponent<JM_SpotLight>().outerAngle;
        Color dupColor = activeLight.GetComponent<JM_SpotLight>().color;
        float dupIntensity = activeLight.GetComponent<JM_SpotLight>().intensity;
        float dupRange = activeLight.GetComponent<JM_SpotLight>().range;

        GameObject newLight = Instantiate(spotLightFactory);
        activeLight = newLight;
        Button btn = Instantiate(sptLightBtn, sptLightContent);
        sptLightBtnList.Add(btn);

        spotLightList.Add(newLight);
        newLight.transform.position = new Vector3(0, 2.5f, 0);
        spotLightCode = newLight.GetComponent<JM_SpotLight>();

        spotLightCode.innerAngle = dupInner;
        spotLightCode.outerAngle = dupOuter;
        spotLightCode.color = dupColor;
        spotLightCode.intensity = dupIntensity;
        spotLightCode.range = dupRange;
    }

    public void OnClickDupPtLightBtn()
    {
        Color dupColor = activeLight.GetComponent<JM_PointLight>().color;
        float dupIntensity = activeLight.GetComponent<JM_PointLight>().intensity;
        float dupRange = activeLight.GetComponent<JM_PointLight>().range;

        GameObject newLight = Instantiate(pointLightFactory);
        activeLight = newLight;
        Button btn = Instantiate(ptLightBtn, ptLightContent);
        ptLightBtnList.Add(btn);

        pointLightList.Add(newLight);
        newLight.transform.position = new Vector3(0, 2.5f, 0);
        pointLightCode = newLight.GetComponent<JM_PointLight>();

        pointLightCode.color = dupColor;
        pointLightCode.intensity = dupIntensity;
        pointLightCode.range = dupRange;
    }

    // delete spot light
    public void OnClickDeleteSptLightBtn()
    {
        for (int i = 0; i < spotLightList.Count; i++)
        {
            if (activeLight == spotLightList[i])
            {
                Destroy(activeLight);
                spotLightList.RemoveAt(i);
                spotLightInfoList.RemoveAt(i);
                sptLightBtnList.RemoveAt(i);
                // need to delete button in context as well
                Destroy(sptLightContent.GetChild(i).gameObject);
                gizmoManager.GizmoOff();
                Deco_Json.Instance.DeleteLightJson(activeLight);
            }
        }
    }

    // delete point light
    public void OnClickDeletePtLightBtn()
    {
        for (int i = 0; i < pointLightList.Count; i++)
        {
            if (activeLight == pointLightList[i])
            {
                print(i);
                Destroy(activeLight);
                pointLightList.RemoveAt(i);
                pointLightInfoList.RemoveAt(i);
                ptLightBtnList.RemoveAt(i);
                Destroy(ptLightContent.GetChild(i).gameObject);
                gizmoManager.GizmoOff();
                Deco_Json.Instance.DeleteLightJson(activeLight);
            }
        }
    }   

    // spot light library
    public void OnClickSptLightBtn()
    {
        for (int i = 0; i < sptLightBtnList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == sptLightBtnList[i].gameObject)
            {
                gizmoManager.targetObject = spotLightList[i];
                gizmoManager.isMove = true;
                activeLight = spotLightList[i];
                spotLightCode = spotLightList[i].GetComponent<JM_SpotLight>();
                spotLightCode.Sync();
            }
        }
    }

    // point light library
    public void OnClickPtLightBtn()
    {
        for (int i = 0; i < ptLightBtnList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == ptLightBtnList[i].gameObject)
            {
                gizmoManager.targetObject = pointLightList[i];
                gizmoManager.isMove = true;
                activeLight = pointLightList[i];
                pointLightCode = pointLightList[i].GetComponent<JM_PointLight>();
                pointLightCode.Sync();
            }
        }
    }

    public void UpdateLightInfo()
    {
        for (int i = 0; i < spotLightList.Count; i++)
        {
            spotLightList[i].GetComponent<JM_SpotLight>().UpdateSptLightInfo(i);
        }

        for (int i = 0; i < spotLightInfoList.Count; i++)
        {
            LightInfo info = new LightInfo();
            info.spot = true;
            info.position = spotLightInfoList[i].lightPos;
            info.eulerAngle = spotLightInfoList[i].lightRot;
            info.localScale = spotLightInfoList[i].lightScale;
            info.innerAngle = spotLightInfoList[i].innerAngle;
            info.outerAngle = spotLightInfoList[i].outerAngle; 
            info.color = spotLightInfoList[i].color;
            info.range = spotLightInfoList[i].range;
            info.intensity = spotLightInfoList[i].intensity;
            info.range = spotLightInfoList[i].range;

            Deco_Json.Instance.SaveLightJson(info);
        }

        for (int i = 0; i < pointLightList.Count; i++)
        {
            pointLightList[i].GetComponent<JM_PointLight>().UpdatePtLightInfo(i);
        }

        for (int i = 0; i < pointLightInfoList.Count; i++)
        {
            LightInfo info = new LightInfo();
            info.spot = false;
            info.position = pointLightInfoList[i].lightPos;
            info.eulerAngle = pointLightInfoList[i].lightRot;
            info.localScale = pointLightInfoList[i].lightScale;
            //info.innerAngle = pointLightInfoList[i].innerAngle;
            //info.outerAngle = pointLightInfoList[i].outerAngle;
            info.color = pointLightInfoList[i].color;
            info.range = pointLightInfoList[i].range;
            info.intensity = pointLightInfoList[i].intensity;
            info.range = pointLightInfoList[i].range;

            Deco_Json.Instance.SaveLightJson(info);
        }

        // test
        //AllLightInfo test = new AllLightInfo();
        //test.spotLightInfo = spotLightInfoList;
        //test.pointLightInfo = pointLightInfoList;
        //string jsonInfo = JsonUtility.ToJson(test);
        //print(jsonInfo);
        //string fileName = "test";
        //string path = Application.dataPath + "/" + fileName + ".Json";
        //File.WriteAllText(path, jsonInfo);


    }
}

[System.Serializable]
public class AllLightInfo
{
    public List<SpotLightInformation> spotLightInfo = new List<SpotLightInformation>();
    public List<PointLightInformation> pointLightInfo = new List<PointLightInformation>();
}
