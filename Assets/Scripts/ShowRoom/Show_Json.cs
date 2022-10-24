using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Show_Json : MonoBehaviourPun
{
    public static Show_Json Instance;
    public InputField loadInputField; 
    public Objects objects;
    ArrayJson arrayJson;

    Vector3 initPos = new Vector3(0, 0, 20);
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        //loadInputField.onSubmit.AddListener(LoadFile);


        GameObject go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        go.name = PhotonNetwork.NickName;
        PhotonNetwork.Instantiate("CamFollow", Vector3.zero, Quaternion.identity);
    }

    void Start()
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInfo");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Extension.ToLower().CompareTo(".txt") == 0)
            {
                string fileName = file.Name.Substring(0, file.Name.Length - 4);
                //mapData.txt를 불러오기
                string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + fileName + ".txt");
                //ArrayJson 형태로 Json을 변환
                ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
                if (arrayJson.access)
                    initPos -= (arrayJson.XSize / 2 + 3) * Vector3.right;
            }
        }
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Extension.ToLower().CompareTo(".txt") == 0)
            {
                string fileName = file.Name.Substring(0, file.Name.Length - 4);
                //mapData.txt를 불러오기
                string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + fileName + ".txt");
                //ArrayJson 형태로 Json을 변환
                ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
                if (arrayJson.access)
                {
                    initPos += (arrayJson.XSize + 3) * Vector3.right;
                    LoadFile(fileName, initPos);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFile(string roomName, Vector3 pos)
    {
        if (roomName.Length == 0)
            return;
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson 형태로 Json을 변환
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson의 데이터로 방 생성
        GameObject newRoom = new GameObject(roomName);
        GameObject newWalls = new GameObject("Walls");
        newRoom.transform.position = pos;
        newRoom.transform.rotation = Quaternion.identity;
        newRoom.transform.localScale = Vector3.one;
        newWalls.transform.parent = newRoom.transform;
        newWalls.transform.localPosition = Vector3.zero;
        newWalls.transform.rotation = Quaternion.identity;
        newWalls.transform.localScale = Vector3.one;
        Deco_RoomInit.Instance.MakeRoom(arrayJson.XSize, arrayJson.YSize, arrayJson.ZSize, arrayJson.balcony, newRoom.transform);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }

        newRoom.AddComponent<PhotonView>();
        Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
        infoUI.x = arrayJson.XSize;
        infoUI.y = arrayJson.YSize;
        infoUI.category = arrayJson.category;
        infoUI.description = arrayJson.description;
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        //해당 위치에 BlueCube를 생성해서 놓는다.
        foreach (GameObject go in objects.datas)
        {
            if (go.GetComponent<Deco_Idx>().Idx == idx)
            {
                GameObject obj = Instantiate(go);
                obj.transform.parent = room;
                obj.transform.localPosition = position;
                obj.transform.localEulerAngles = eulerAngle;
                obj.transform.localScale = localScale;

                //Show_FurnitInfoUI furnitUi = obj.transform.GetChild(0).gameObject.AddComponent<Show_FurnitInfoUI>();
                //furnitUi.furnitName = obj.GetComponent<Deco_Idx>().Name;
                //furnitUi.price = obj.GetComponent<Deco_Idx>().Price;
                //furnitUi.category = obj.GetComponent<Deco_Idx>().Category;
            }
        }
    }
}