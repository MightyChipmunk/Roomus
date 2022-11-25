using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class roomInfos
{
    public int roomNo;
    public bool access;
    public string category;
    public string roomName;
    public string screenShotUrl;
}

public class Deco_LoadRoomList : MonoBehaviour
{
    public static Deco_LoadRoomList Instance;
    public RectTransform trContent;
    public GameObject roomItem;

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
        if(Instance == null)
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
        // 네트워크로 방 리스트를 가져옴
        StartCoroutine(OnGetJson(UrlInfo.url + "/rooms"));

        // 로컬로 방 리스트를 가져옴
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInfo");
        //foreach (FileInfo File in di.GetFiles())
        //{
        //    if (File.Extension.ToLower().CompareTo(".txt") == 0)
        //    {
        //        string fileName = File.Name.Substring(0, File.Name.Length - 4);
        //        AddContent(Random.Range(0, 1000), fileName);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewRoom()
    {
        SceneManager.LoadScene("RoomCustom_Main");
    }

    public void LoadRoom()
    {
        if (RoomName != null)
            SceneManager.LoadScene("RoomDecoScene");
    }

    void AddContent(int id, string s, bool access, byte[] imgData)
    {
        GameObject item = Instantiate(roomItem, trContent);
        item.name = s;
        item.GetComponent<Deco_RoomItem>().ID = id;
        item.GetComponent<Deco_RoomItem>().roomName = s;
        item.GetComponent<Deco_RoomItem>().access = access;
        item.GetComponent<Deco_RoomItem>().imgData = imgData;
    }

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
                // url 배열을 json으로 받아서 가져옴
                roomInfos[] data = JsonHelper.FromJson<roomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가져온 url 배열을 반복문으로 순회하며 이름과 id를 가져오는 함수 실행
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("Room UrlList Download complete!");
            }
        }
    }

    IEnumerator OnGetUrl(roomInfos info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                AddContent(info.roomNo, info.roomName, info.access, www.downloadHandler.data);
                Debug.Log(www.error);
            }
            else
            {
                // 가져온 스크린샷과 id로 라이브러리에 가구 추가
                // Deco_FurnitItem
                AddContent(info.roomNo, info.roomName, info.access, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
