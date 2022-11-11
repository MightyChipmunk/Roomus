using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

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

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            string type = file.Name.Substring(file.Name.Length - 3, 3);
            if (type == "txt")
            {
                FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
                AddContent(fbxJson);
            }
        }

        library.transform.parent.gameObject.SetActive(false);

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
        if (library.transform.parent.gameObject.activeSelf)
            library.transform.parent.gameObject.SetActive(false);
        else
            library.transform.parent.gameObject.SetActive(true);    
    }

    void AddContent(FBXJson fbxJson)
    {
        GameObject item = Instantiate(furnitItem, trContent);
        item.name = fbxJson.furnitName;
        item.GetComponent<Deco_FurnitItem>().fbxJson = fbxJson;
        item.GetComponentInChildren<Text>().text = fbxJson.furnitName;
    }
/*
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
        }
    }
*/
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
}
