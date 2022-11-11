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
    public int no;
    public string category;
    public string roomName;
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
        //StartCoroutine(OnGetJson("http://192.168.0.243:8000/v1/products"));
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
        if (RoomName.Length > 0)
            SceneManager.LoadScene("RoomDecoScene");
    }

    void AddContent(int id, string s)
    {
        GameObject item = Instantiate(roomItem, trContent);
        item.name = s;
        item.GetComponentInChildren<Text>().text = s;
        item.GetComponent<Deco_RoomItem>().ID = id;
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
                roomInfos[] data = JsonHelper.FromJson<roomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가져온 url 배열을 반복문으로 순회하며 스크린샷과 id를 가져오는 함수 실행
                    AddContent(data[i].no, data[i].roomName);
                }
                Debug.Log("UrlList Download complete!");
            }
        }
    }
}
