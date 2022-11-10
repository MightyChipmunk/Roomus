using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

[Serializable]
public class FBXJson
{
    public int no;
    public string furnitName;
    public bool location;
    public string category;
    public string information;
    public float xsize;
    public float ysize;
    public float zsize;
    public int price;
    //public string createdDate;
    //public string lastModifiedDate;
    public string fileUrl;
    //public string screenShotUrl;
}

public class FBXUIManager : MonoBehaviour
{
    public FBXJson fbxJson = new FBXJson();
    public List<byte[]> fbxTextures = new List<byte[]>();

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

    public static FBXUIManager Instance;

    public byte[] fbxData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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
        fbxJson.xsize = float.Parse(s);
    }
    void OnYchanged(string s)
    {
        fbxJson.ysize = float.Parse(s);
    }
    void OnZchanged(string s)
    {
        fbxJson.ysize = float.Parse(s);
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
        string jsonData = JsonUtility.ToJson(fbxJson, true);
        string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".txt";

        //File.WriteAllText(path, jsonData);

        infos.gameObject.SetActive(false);
        fbx.gameObject.SetActive(true);

        screenCode.screen.SetActive(true);
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
    }

    public void OnEndClicked()
    {
        // ��ũ������ ��� ���� ����, ��ũ���� ����, ���� ���� Json �����͸� ������ ����
        StartCoroutine(capture("http://192.168.0.243:8000/v1/products"));
    }

    // ��ũ����, ���� ����, ���� �����͸� ������ �����ϴ� �Լ�
    IEnumerator capture(string uri)
    {
        yield return new WaitForEndOfFrame();

        // ������ ��ũ������ �� ���̳ʸ� �����ͷ� ����
        byte[] imgBytes;
        Texture2D texture = new Texture2D(Screen.width / 3, Screen.height / 2, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(640, 360, Screen.width / 3, Screen.height / 2), 0, 0, false);
        texture.Apply();
        imgBytes = texture.EncodeToPNG();

        // zip���Ϸ� ���� ���ϵ��� ������ ���丮 ����
        string path = Application.dataPath + "/Localserver/" + fbxJson.furnitName + "/";
        Directory.CreateDirectory(path);

        // �̸� ������ fbx�� zip������ ���̴��� �����ͷ� ����
        byte[] zipData = File.ReadAllBytes(path + fbxJson.furnitName + ".zip");

        // �������͸� �����ϰ� fbx�� zip���ϰ� ��ũ���� ������ �߰�
        WWWForm form = new WWWForm();
        form.AddBinaryData("zipFile", zipData);
        form.AddBinaryData("screenShot", imgBytes);
        // �������Ϳ� ������ ������ �߰�
        form.AddField("furnitName", fbxJson.furnitName);
        form.AddField("location", fbxJson.location.ToString());
        form.AddField("category", fbxJson.category);
        form.AddField("information", fbxJson.information);
        form.AddField("xSize", fbxJson.xsize.ToString());
        form.AddField("ySize", fbxJson.ysize.ToString());
        form.AddField("zSize", fbxJson.zsize.ToString());
        form.AddField("price", fbxJson.price.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}