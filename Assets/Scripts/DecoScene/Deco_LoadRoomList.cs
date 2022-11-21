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
        // ��Ʈ��ũ�� �� ����Ʈ�� ������
        StartCoroutine(OnGetJson(UrlInfo.url + "/rooms"));

        // ���÷� �� ����Ʈ�� ������
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
                // url �迭�� json���� �޾Ƽ� ������
                roomInfos[] data = JsonHelper.FromJson<roomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // ������ url �迭�� �ݺ������� ��ȸ�ϸ� �̸��� id�� �������� �Լ� ����
                    AddContent(data[i].roomNo, data[i].roomName);
                }
                Debug.Log("Room UrlList Download complete!");
            }
        }
    }
}
