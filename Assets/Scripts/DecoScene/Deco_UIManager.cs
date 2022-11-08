using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class UrlJson
{
    public List<string> datas = new List<string>();
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

        StartCoroutine(OnGetJson("url 배열을 담은 서버url"));

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
                UrlJson urlJson = JsonUtility.FromJson<UrlJson>(www.downloadHandler.text);
                for (int i = 0; i < urlJson.datas.Count; i++)
                {
                    StartCoroutine(OnGetUrl(urlJson.datas[i]));
                }
            }
        }
    }

    IEnumerator OnGetUrl(string uri)
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
                AddContent(Int32.Parse(www.downloadHandler.text), www.downloadHandler.data);
            }
        }
    }
}
