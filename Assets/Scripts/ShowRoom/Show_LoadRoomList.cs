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
        StartCoroutine(OnGetJson("http://192.168.0.243:8000/v1/products"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddContent(int id, string roomName, string roomDesc, byte[] imgBytes)
    {
        GameObject item = Instantiate(showRoomItem, trContent);
        item.name = roomName;
        item.GetComponent<Show_RoomItem>().ID = id;
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
                // url �迭�� json���� �޾Ƽ� ������
                showRoomInfos[] data = JsonHelper.FromJson<showRoomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // ������ url �迭�� �ݺ������� ��ȸ�ϸ� ��ũ������ id�� �������� �Լ� ����
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
                // ������ ��ũ������ id�� ���̺귯���� ���� �߰�
                // Deco_FurnitItem
                AddContent(info.no, info.roomName, info.roomDescription, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
