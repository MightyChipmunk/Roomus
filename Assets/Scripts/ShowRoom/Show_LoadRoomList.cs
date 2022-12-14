using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class showRoomInfos
{
    public int roomNo;
    public bool access;
    public string roomName;
    public string category;
    public string description;
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
    public string RoomName { get { return roomName; } set { roomName = value; } }



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
        switch (TokenManager.Instance.roomTypeP)
        {
            case RoomType.All:
                StartCoroutine(OnGetJson(UrlInfo.url + "/rooms"));
                break;
            case RoomType.Liked:
                StartCoroutine(OnGetJson(UrlInfo.url + "/rooms/mylikes"));
                break;
            case RoomType.User:
                StartCoroutine(OnGetJson(UrlInfo.url + "/rooms?criteria=memberNo&value=" + TokenManager.Instance.MemberNo.ToString()));
                break;
        }

        // 로컬의 방 리스트를 가져옴
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInfo");
        //int id = 0;
        //foreach (FileInfo file in di.GetFiles())
        //{
        //    if (file.Extension.ToLower().CompareTo(".txt") == 0)
        //    {
        //        string jsonData = File.ReadAllText(file.FullName);
        //        byte[] imgData = File.ReadAllBytes(file.FullName.Substring(0, file.FullName.Length - 4) + ".png");
        //        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        //        //string fileName = file.Name.Substring(0, file.Name.Length - 4);
        //        AddContent(id++, arrayJson.roomName, arrayJson.description, imgData, arrayJson.category);
        //    }
        //}
    }
    public void OnClickCategory(string s)
    {
        for (int i = 0; i < trContent.transform.childCount; i++)
        {
            Show_RoomItem sr;
            LocalTestItem lt;
            if (trContent.transform.GetChild(i).TryGetComponent<Show_RoomItem>(out sr) && sr.Category != s)
                trContent.transform.GetChild(i).gameObject.SetActive(false);
            else if (trContent.transform.GetChild(i).TryGetComponent<Show_RoomItem>(out sr) && sr.Category == s)
                trContent.transform.GetChild(i).gameObject.SetActive(true);
            else if (trContent.transform.GetChild(i).TryGetComponent<LocalTestItem>(out lt) && lt.Category != s)
                trContent.transform.GetChild(i).gameObject.SetActive(false);
            else if (trContent.transform.GetChild(i).TryGetComponent<LocalTestItem>(out lt) && lt.Category == s)
                trContent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddContent(int id, string roomName, string roomDesc, string category, byte[] imgBytes = null)
    {
        GameObject item = Instantiate(showRoomItem, trContent);
        item.name = roomName;
        item.GetComponent<Show_RoomItem>().ID = id;
        item.GetComponent<Show_RoomItem>().Category = category;
        item.GetComponent<Show_RoomItem>().roomName.text = roomName;
        item.GetComponent<Show_RoomItem>().roomDescription.text = roomDesc;
        if (imgBytes != null)
            item.GetComponent<Show_RoomItem>().ImageBytes = imgBytes;
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
                showRoomInfos[] data = JsonHelper.FromJson<showRoomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가져온 url 배열을 반복문으로 순회하며 스크린샷과 id를 가져오는 함수 실행
                    if (data[i].access && data[i].screenShotUrl != null)
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
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                AddContent(info.roomNo, info.roomName, info.description, info.category);
                Debug.Log(www.error);
            }
            else
            {
                // 가져온 스크린샷과 id로 라이브러리에 가구 추가
                // Deco_FurnitItem
                AddContent(info.roomNo, info.roomName, info.description, info.category, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
