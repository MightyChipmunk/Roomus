using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class FBXJson
{
    public int id;
    public string furnitName;
    public bool location;
    public string category;
    public string information;
    public float xSize;
    public float ySize;
    public float zSize;
    public int price;
}

public class FBXUIManager : MonoBehaviour
{
    FBXJson fbxJson = new FBXJson();

    public JM_ScreenManager screenCode;

    public GameObject infos;
    public GameObject fbx;

    public InputField nameInput;
    public InputField priceInput;
    public Toggle floorToggle;
    public Toggle wallToggle;
    public InputField xInput;
    public InputField yInput;
    public InputField zInput;
    public Dropdown categoryUI;
    public InputField infoInput;
    // Start is called before the first frame update
    void Start()
    {
        nameInput.onEndEdit.AddListener(OnNameChanged);
        priceInput.onEndEdit.AddListener(OnPriceChanged);

        floorToggle.onValueChanged.AddListener(OnFloorChanged);
        wallToggle.onValueChanged.AddListener(OnWallChanged);

        xInput.onEndEdit.AddListener(OnXchanged);
        yInput.onEndEdit.AddListener(OnYchanged);
        zInput.onEndEdit.AddListener(OnZchanged);

        infoInput.onEndEdit.AddListener(OnInfoChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnNameChanged(string s)
    {
        fbxJson.furnitName = s;
        FBXUpLoad.Instance.FileName = s;
    }

    void OnPriceChanged(string s)
    {
        fbxJson.price = Int32.Parse(s);
    }

    void OnFloorChanged(bool value)
    {
        if (value)
            wallToggle.isOn = false;
        else
            wallToggle.isOn = true;
    }

    void OnWallChanged(bool value)
    {
        if (value)
            floorToggle.isOn = false;
        else
            floorToggle.isOn = true;
    }

    void OnXchanged(string s)
    {
        fbxJson.xSize = float.Parse(s);
    }
    void OnYchanged(string s)
    {
        fbxJson.ySize = float.Parse(s);
    }
    void OnZchanged(string s)
    {
        fbxJson.zSize = float.Parse(s);
    }

    void OnInfoChanged(string s)
    {
        fbxJson.information = s;
    }

    public void OnNextButtonClicked()
    {
        fbxJson.location = floorToggle.isOn;
        switch (categoryUI.value)
        {
            case 0:
                fbxJson.category = "Bed";
                break;
            case 1:
                fbxJson.category = "Chair";
                break;
            case 2:
                fbxJson.category = "Dest";
                break;
            case 3:
                fbxJson.category = "Table";
                break;
            case 4:
                fbxJson.category = "Lighting";
                break;
            case 5:
                fbxJson.category = "Sofa";
                break;
            case 6:
                fbxJson.category = "Shelf";
                break;
            case 7:
                fbxJson.category = "Drawer";
                break;
            case 8:
                fbxJson.category = "Kitchen";
                break;
        }
        fbxJson.id = UnityEngine.Random.Range(0, 10000);
        string jsonData = JsonUtility.ToJson(fbxJson, true);
        string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".txt";

        UnityWebRequest www = UnityWebRequest.Post("192.168.0.243", jsonData);
        //www.Post("192.168.0.243", jsonData);

        File.WriteAllText(path, jsonData);

        infos.gameObject.SetActive(false);
        fbx.gameObject.SetActive(true);

        screenCode.screen.SetActive(true);
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
    }

    public void OnEndClicked()
    {
        StartCoroutine("capture");
    }

    IEnumerator capture()
    {
        yield return new WaitForEndOfFrame();

        byte[] imgBytes;
        string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "ScreenShot" + ".png";

        Texture2D texture = new Texture2D(Screen.width / 3, Screen.height / 2, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(960, 360, Screen.width / 3, Screen.height / 2), 0, 0, false);
        texture.Apply();

        imgBytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, imgBytes);
    }
}