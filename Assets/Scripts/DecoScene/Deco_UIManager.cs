using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class furnitInfos
{
    public int no;
    public string furnitName;
    public string category;
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
    public Image screenShotImage;
    public InputField searchInput;

    string roomName;
    public string RoomName { get { return roomName; } set { roomName = value; } }
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

        StartCoroutine(OnGetJson(UrlInfo.url + "/products"));

        //library.transform.parent.gameObject.SetActive(false);

        nameField.onEndEdit.AddListener(OnNameSubmit);
        descriptField.onEndEdit.AddListener(OnDescSubmit);

        publicToggle.onValueChanged.AddListener(OnPublicChanged);
        privateToggle.onValueChanged.AddListener(OnPrivateChanged);

        searchInput.onSubmit.AddListener(OnSubmitSearch);

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

    void AddContent(string furnitName, string category, int id = 0, byte[] imgBytes = null)
    {
        GameObject item = Instantiate(furnitItem, trContent);
        item.name = id.ToString();
        //item.GetComponent<Deco_FurnitItem>().fbxJson = fbxJson;
        item.GetComponent<Deco_FurnitItem>().ID = id;
        item.GetComponent<Deco_FurnitItem>().FurnitName = furnitName;
        if (category != null)
            item.GetComponent<Deco_FurnitItem>().Category = category;
        item.GetComponent<Deco_FurnitItem>().ImageBytes = imgBytes;
        item.GetComponentInChildren<Text>().text = "";
    }

    public void OnPostClicked()
    {
        //screenCode.Darken();
        //screenCode.isTabChange = true;
        if (posting.activeSelf)
            posting.SetActive(false);
        else
        {
            posting.SetActive(true);
            //StartCoroutine(Deco_Json.Instance.WaitForScreenShot());
        }
    }

    public void ChangeTabToPost()
    {
        if (posting.activeSelf)
            posting.SetActive(false);
        else
        {
            posting.SetActive(true);
            //StartCoroutine(Deco_Json.Instance.WaitForScreenShot());
        }
    }

    //public void EndScreenShot()
    //{
    //    posting.SetActive(true);
    //    screenCode.screen.SetActive(true);
    //    screenCode.isDark = true;
    //    screenCode.isStart = true;
    //}

    public void OnUploadClicked()
    {
        Deco_Json.Instance.PostFile(roomName, publicToggle.isOn, category.value, description);
    }

    public void OnScreenShotClicked()
    {
        StartCoroutine(WaitForScreenShot());
    }
    
    public void OnPostScreenShotClicked()
    {
        //if (imgBytes != null)
        //    Deco_Json.Instance.PostScreenShot(imgBytes);
    }

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } }
    public IEnumerator WaitForScreenShot()
    {
        yield return new WaitForEndOfFrame();

        // ?????? ??????????????? ????????? ???????????? ???????????? ??????
        Texture2D texture = new Texture2D(800, 800, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(560, 140, 800, 800), 0, 0, false);
        texture.Apply();
        imgBytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/screenShot.png", imgBytes);
        imgBytes = File.ReadAllBytes(Application.dataPath + "/screenShot.png");

        screenShotImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
    }

    void OnNameSubmit(string s)
    {
        roomName = s;
    }

    void OnDescSubmit(string s)
    {
        description = s;
    }

    public void OnClickCategory(string s)
    {
        if (s == "")
        {
            for (int i = 0; i < trContent.transform.childCount; i++)
            {
                trContent.transform.GetChild(i).gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < trContent.transform.childCount; i++)
        {
            if (trContent.transform.GetChild(i).GetComponent<Deco_FurnitItem>().Category != s)
                trContent.transform.GetChild(i).gameObject.SetActive(false);
            else
                trContent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnSubmitSearch(string s)
    {
        if (s == "")
        {
            for (int i = 0; i < trContent.transform.childCount; i++)
            {
                trContent.transform.GetChild(i).gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < trContent.transform.childCount; i++)
        {
            if (trContent.transform.GetChild(i).GetComponent<Deco_FurnitItem>().FurnitName.ToLower().Contains(s.ToLower()))
                trContent.transform.GetChild(i).gameObject.SetActive(true);
            else
                trContent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnSimFurnit(List<int> id)
    {
        for (int i = 0; i < trContent.transform.childCount; i++)
        {
            if (id.Contains(trContent.transform.GetChild(i).GetComponent<Deco_FurnitItem>().ID))
                trContent.transform.GetChild(i).gameObject.SetActive(true);
            else
                trContent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // ???????????? ???????????? url??? ?????? ?????????
    IEnumerator OnGetJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // ???????????? ???????????? ?????????
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // ???????????? ???????????? ?????? ???????????? url??? ?????? Get ????????? ??????
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("Furnit UrlList Download complete!");
            }
        }
    }

    // ?????? ????????? ???????????? url??? Get ????????? ??????
    IEnumerator OnGetUrl(furnitInfos info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // ????????? ??????????????? ?????? ?????? ????????? ????????? ?????????????????? ??????
                // Deco_FurnitItem
                AddContent(info.furnitName, info.category, info.no, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
