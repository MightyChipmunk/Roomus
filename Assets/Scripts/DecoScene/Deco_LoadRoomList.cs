using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Deco_LoadRoomList : MonoBehaviour
{
    public static Deco_LoadRoomList Instance;
    public RectTransform trContent;
    public GameObject roomItem;

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
        Directory.CreateDirectory(Application.dataPath + "/RoomInfo");
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInfo");
        foreach (FileInfo File in di.GetFiles())
        {
            if (File.Extension.ToLower().CompareTo(".txt") == 0)
            {
                string fileName = File.Name.Substring(0, File.Name.Length - 4);
                AddContent(fileName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddContent(string s)
    {
        GameObject item = Instantiate(roomItem, trContent);
        item.name = s;
        item.GetComponentInChildren<Text>().text = s;
    }
}
