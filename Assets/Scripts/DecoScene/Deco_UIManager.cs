using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class dataInfo
{
    public int no;
    public string screenShotUrl;
}

public class Deco_UIManager : MonoBehaviour
{
    public static Deco_UIManager Instance;

    public GameObject screenManager;
    JM_ScreenManager screenCode;

    public InputField nameField;
    public Toggle publicToggle;
    public Toggle privateToggle;
    public Dropdown category;
    public InputField descriptField;

    string roomName;
    string description;

    GameObject library;
    GameObject posting;
    public GameObject furnitItem;
    RectTransform trContent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        library = GameObject.Find("Library");
        posting = GameObject.Find("Canvas").transform.Find("Posting").gameObject;
        posting.SetActive(false);
        trContent = (RectTransform)library.transform.Find("Viewport").transform.Find("Content");

        // 추후에 서버에 있는 모든 Json파일을 요청해서 받는 식으로 전환
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        //foreach (FileInfo file in di.GetFiles())
        //{
        //    string type = file.Name.Substring(file.Name.Length - 3, 3);
        //    if (type == "txt")
        //    {
        //        FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
        //        AddContent(fbxJson);
        //    }
        //}

        StartCoroutine(OnGetJson("http://192.168.0.243:8000/v1/products"));

        library.SetActive(false);

        nameField.onEndEdit.AddListener(OnNameSubmit);
        descriptField.onEndEdit.AddListener(OnDescSubmit);

        publicToggle.onValueChanged.AddListener(OnPublicChanged);
        privateToggle.onValueChanged.AddListener(OnPrivateChanged);

        // Screen (Jaemin)
        screenCode = screenManager.GetComponent<JM_ScreenManager>();
    }

    void OnPublicChanged(bool value)
    {
        if (value)
            privateToggle.isOn = false;
        else
            privateToggle.isOn = true;
    }

    void OnPrivateChanged(bool value)
    {
        if (value)
            publicToggle.isOn = false;
        else
            publicToggle.isOn = true;
    }

    public void OnLoadLibrary()
    {
        if (library.activeSelf)
            library.SetActive(false);
        else
            library.SetActive(true);
    }

    void AddContent(int id = 0, byte[] imgBytes = null)
    {
        GameObject item = Instantiate(furnitItem, trContent);
        item.name = id.ToString();
        //item.GetComponent<Deco_FurnitItem>().fbxJson = fbxJson;
        item.GetComponent<Deco_FurnitItem>().ID = id;
        item.GetComponent<Deco_FurnitItem>().ImageBytes = imgBytes;
        //item.GetComponentInChildren<Text>().text = fbxJson.furnitName;
    }

    public void OnPostClicked()
    {
        if (posting.activeSelf)
            posting.SetActive(false);
        else
        {
            posting.SetActive(true);
            screenCode.screen.SetActive(true);
            screenCode.isDark = true;
            screenCode.isStart = true;
            screenCode.alpha = 1;
        }
    }

    public void OnUploadClicked()
    {
        Deco_Json.Instance.PostFile(roomName, publicToggle.isOn, category.value, description);
    }

    void OnNameSubmit(string s)
    {
        roomName = s;
    }

    void OnDescSubmit(string s)
    {
        description = s;
    }

    // url의 배열을 서버에서 받아오는 함수
    IEnumerator OnGetJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // url 배열을 json으로 받아서 가져옴
                dataInfo[] data = JsonHelper.FromJson<dataInfo>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가져온 url 배열을 반복문으로 순회하며 스크린샷과 id를 가져오는 함수 실행
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("UrlList Download complete!");
            }
        }
    }

    // 가구의 스크린샷과 id를 받아오는 함수
    IEnumerator OnGetUrl(dataInfo info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 가져온 스크린샷과 id로 라이브러리에 가구 추가
                // Deco_FurnitItem
                AddContent(info.no, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
