using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Deco_UIManager : MonoBehaviour
{
    public static Deco_UIManager Instance;

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

        foreach (GameObject go in Deco_Json.Instance.objects.datas)
        {
            AddContent(go.GetComponent<Deco_Idx>().Name);
        }

        library.SetActive(false);

        nameField.onEndEdit.AddListener(OnNameSubmit);
        descriptField.onEndEdit.AddListener(OnDescSubmit);

        publicToggle.onValueChanged.AddListener(OnPublicChanged);
        privateToggle.onValueChanged.AddListener(OnPrivateChanged);
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

    void AddContent(string contentName)
    {
        GameObject item = Instantiate(furnitItem, trContent);
        item.name = contentName;
        item.GetComponentInChildren<Text>().text = contentName;
    }

    public void OnPostClicked()
    {
        if (posting.activeSelf)
            posting.SetActive(false);
        else
            posting.SetActive(true);
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
}
