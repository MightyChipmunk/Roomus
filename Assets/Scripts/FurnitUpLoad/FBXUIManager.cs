using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using Ookii.Dialogs;
using System.Windows.Forms;
using Screen = UnityEngine.Screen;

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
    public int countLikes = 0;
    public bool isDelete = false;
    //public string screenShotUrl;
    public string sellUrl = "";
}

public class FBXUIManager : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog = new VistaOpenFileDialog();

    private string[] m_FilePaths; // 파일 패스

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
    public InputField sellUrl;

    public static FBXUIManager Instance;

    public byte[] fbxData;
    byte[] imgBytes;

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
        sellUrl.onEndEdit.AddListener(OnSellChanged);
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
        fbxJson.zsize = float.Parse(s);
    }

    void OnInfoChanged(string s)
    {
        fbxJson.information = s;
    }

    void OnSellChanged(string s)
    {
        fbxJson.sellUrl = s;
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
        string path = UnityEngine.Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".txt";

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
        // 스크린샷을 찍고 가구 파일, 스크린샷 파일, 가구 정보 Json 데이터를 서버에 전달
        StartCoroutine(capture("http://54.180.108.64:80/v1/products"));
    }

    public void OnImageOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFBXFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);

        if (m_FilePaths.Length > 0)
            OpenImageFile();
    }

    private void OpenImageFile()
    {
        imgBytes = File.ReadAllBytes(m_FilePaths[0]);
    }

    string[] FileOpen(VistaOpenFileDialog openFileDialog)
    {
        var result = openFileDialog.ShowDialog();
        var filenames = result == DialogResult.OK ?
            openFileDialog.FileNames :
            new string[0];
        openFileDialog.Dispose();
        return filenames;
    }

    void SetOpenFBXFileDialog()
    {
        m_OpenFileDialog.Title = "파일 열기";
        m_OpenFileDialog.Filter = "이미지 파일| *.png";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = false;
    }

    // 스크린샷, 가구 파일, 정보 데이터를 서버로 전달하는 함수
    IEnumerator capture(string uri)
    {
        yield return new WaitForEndOfFrame();

        // zip파일로 묶을 파일들을 저장할 디렉토리 생성
        string path = UnityEngine.Application.dataPath + "/Localserver/" + fbxJson.furnitName + "/";
        Directory.CreateDirectory(path);

        // 미리 생성된 fbx의 zip파일을 파이더리 데이터로 읽음
        byte[] zipData = File.ReadAllBytes(path + fbxJson.furnitName + ".zip");

        // 폼데이터를 생성하고 fbx의 zip파일과 스크린샷 파일을 추가
        WWWForm form = new WWWForm();
        form.AddBinaryData("zipFile", zipData, "ZipFile", "application/zip");
        form.AddBinaryData("screenShot", imgBytes);
        // 폼데이터에 가구의 정보를 추가
        form.AddField("furnitName", fbxJson.furnitName);
        form.AddField("location", fbxJson.location.ToString());
        form.AddField("category", fbxJson.category);
        form.AddField("information", fbxJson.information);
        form.AddField("xsize", fbxJson.xsize.ToString());
        form.AddField("ysize", fbxJson.ysize.ToString());
        form.AddField("zsize", fbxJson.zsize.ToString());
        form.AddField("price", fbxJson.price.ToString());
        form.AddField("sell", fbxJson.sellUrl);

        Debug.Log(fbxJson.xsize.ToString());

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