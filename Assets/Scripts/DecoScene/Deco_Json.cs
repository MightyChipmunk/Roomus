using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveJsonInfo
{
    public int idx;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

[Serializable]
public class ArrayJson
{
    public string roomName;
    public bool access;
    public string category;
    public string description;
    public float XSize;
    public float YSize;
    public float ZSize;
    public int balcony;
    public List<SaveJsonInfo> datas;
}

[Serializable]
public class Objects
{
    public List<GameObject> datas;
}

public class Deco_Json : MonoBehaviour
{
    public InputField saveInputField;
    public InputField loadInputField;

    public static Deco_Json Instance { get; set; }
    ArrayJson arrayJson;

    public Objects objects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        //ObjectAdd();

        if (saveInputField != null)
            saveInputField.onSubmit.AddListener(SaveNewFile);
        if (loadInputField != null)
            loadInputField.onSubmit.AddListener(LoadFile);
    }

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/RoomInfo");
    }

    public void SaveRoomInfo(string roomName, float x, float y, float z, int bal)
    {
        arrayJson.roomName = roomName;
        arrayJson.XSize = x;
        arrayJson.YSize = y;
        arrayJson.ZSize = z;
        arrayJson.balcony = bal;
    }

    public void SaveJson(GameObject go, int idx)
    {
        SaveJsonInfo info;
        
        info = new SaveJsonInfo();
        info.idx = idx;
        info.position = go.transform.position;
        info.eulerAngle = go.transform.eulerAngles;
        info.localScale = go.transform.localScale;

        //ArrayJson 의 datas 에 하나씩 추가
        arrayJson.datas.Add(info);
    }

    public void DeleteJson(GameObject go)
    {
        foreach (SaveJsonInfo info in arrayJson.datas)
        {
            if (info.position == go.transform.position)
            {
                arrayJson.datas.Remove(info);
                return;
            }
        }
    }

    void SaveNewFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //방 이름 변경
        arrayJson.roomName = roomName;
        //arrayJson을 Json으로 변환
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData를 파일로 저장
        File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt", jsonData);
    }

    public void SaveFile()
    {
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData를 파일로 저장
        File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".txt", jsonData);
    }

    public void PostFile(string roomName, bool access, int category, string desc)
    {
        arrayJson.access = access;
        switch (category)
        {
            case 0:
                arrayJson.category = "Living Room";
                break;
            case 1:
                arrayJson.category = "Bedroom";
                break;
            case 2:
                arrayJson.category = "Bathroom";
                break;
            case 3:
                arrayJson.category = "Kitchen";
                break;
            case 4:
                arrayJson.category = "Library";
                break;
            case 5:
                arrayJson.category = "Entire House";
                break;
        }
        arrayJson.description = desc;

        SaveNewFile(roomName);
    }

    public void LoadFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson 형태로 Json을 변환
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson의 데이터로 방 생성
        Destroy(GameObject.Find("Room"));
        GameObject newRoom = new GameObject("Room");
        GameObject newWalls = new GameObject("Walls");
        newRoom.transform.position = Vector3.zero; 
        newRoom.transform.rotation = Quaternion.identity; 
        newRoom.transform.localScale = Vector3.one;
        newWalls.transform.parent = newRoom.transform;
        newWalls.transform.position = Vector3.zero;
        newWalls.transform.rotation = Quaternion.identity;
        newWalls.transform.localScale = Vector3.one;
        Deco_RoomInit.Instance.MakeRoom(arrayJson.XSize, arrayJson.YSize, arrayJson.ZSize, arrayJson.balcony, newRoom.transform);
        SaveRoomInfo(roomName, arrayJson.XSize, arrayJson.YSize, arrayJson.ZSize, arrayJson.balcony);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Contains("txt") && !file.Name.Contains("meta"))
            {
                FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
                if (fbxJson.id == idx)
                {
                    foreach (FileInfo info in di.GetFiles())
                    {
                        if (info.Name.Contains(fbxJson.furnitName) && !info.Name.Contains("meta") && !info.Name.Contains("txt"))
                        {
                            byte[] data = File.ReadAllBytes(info.FullName);
                            string path = Application.dataPath + "/Resources/" + info.Name;
                            File.WriteAllBytes(path, data);

                            StartCoroutine(WaitForUpload(info, fbxJson, idx, position, eulerAngle, localScale, room));
                        }
                    }
                }
            }
        }
    }

    IEnumerator WaitForUpload(FileInfo file, FBXJson fbxJson, int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        string path = file.Name.Substring(0, file.Name.Length - 4);

        while (true)
        {
            if (Resources.Load<GameObject>(path))
                break;

            yield return null;
        }

        if (path == fbxJson.furnitName)
        {
            GameObject obj = new GameObject(fbxJson.furnitName);
            obj.transform.parent = room;
            obj.transform.localPosition = position;
            obj.transform.localEulerAngles = eulerAngle;
            obj.transform.localScale = localScale;
            GameObject go = Instantiate(Resources.Load<GameObject>(fbxJson.furnitName));
            BoxCollider col = go.AddComponent<BoxCollider>();
            col.center = new Vector3(0, fbxJson.ySize / 2, 0);
            col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            go.transform.parent = obj.transform;
            if (fbxJson.location)
                go.transform.localPosition = Vector3.zero + Vector3.forward;
            else if (!fbxJson.location)
                go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f);
            go.transform.localEulerAngles = Resources.Load<GameObject>(fbxJson.furnitName).transform.eulerAngles;
            Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
            decoIdx.Name = fbxJson.furnitName;
            decoIdx.Price = fbxJson.price;
            decoIdx.Category = fbxJson.category;
            decoIdx.Idx = fbxJson.id;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (File.Exists(Application.dataPath + "/Resources/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"))
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }
            }
        }
    }
}

