using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class showRoomInfos
{
    public int no;
    public string roomName;
    public string category;
    public string roomDescription;
    public string screenShotUrl;
}

public class Show_LoadRoomList : MonoBehaviour
{
    public static Show_LoadRoomList Instance;
    public RectTransform trContent;
    public GameObject showRoomItem;

    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    string roomName;
    public string RoomName
    {
        get
        {
            return roomName;
        }
        set
        {
            roomName = value;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // 네트워크의 방 리스트를 가져옴
        //StartCoroutine(OnGetJson("http://54.180.108.64:80/v1/products/"));

        // 로컬의 방 리스트를 가져옴
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInfo");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Extension.ToLower().CompareTo(".txt") == 0)
            {
                string jsonData = File.ReadAllText(file.FullName);
                byte[] imgData = File.ReadAllBytes(file.FullName.Substring(0, file.FullName.Length - 4) + ".png");
                ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
                //string fileName = file.Name.Substring(0, file.Name.Length - 4);
                AddContent(Random.Range(0, 1000), arrayJson.roomName, arrayJson.description, imgData, arrayJson.category);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddContent(int id, string roomName, string roomDesc, byte[] imgBytes, string category)
    {
        GameObject item = Instantiate(showRoomItem, trContent);
        item.name = roomName;
        item.GetComponent<Show_RoomItem>().ID = id;
        item.GetComponent<Show_RoomItem>().Category = category;
        item.GetComponent<Show_RoomItem>().roomName.text = roomName;
        item.GetComponent<Show_RoomItem>().roomDescription.text = roomDesc;
        item.GetComponent<Show_RoomItem>().ImageBytes = imgBytes;
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
                // url 배열을 json으로 받아서 가져옴
                showRoomInfos[] data = JsonHelper.FromJson<showRoomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가져온 url 배열을 반복문으로 순회하며 스크린샷과 id를 가져오는 함수 실행
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("UrlList Download complete!");
            }
        }
    }

    IEnumerator OnGetUrl(showRoomInfos info)
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
                AddContent(info.no, info.roomName, info.roomDescription, www.downloadHandler.data, info.category);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
