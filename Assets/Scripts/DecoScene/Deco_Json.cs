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

        saveInputField.onSubmit.AddListener(SaveFile);
        loadInputField.onSubmit.AddListener(LoadFile);
    }

    private void Start()
    {

    }

    public void SaveRoomInfo(float x, float y, float z, int bal)
    {
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

    void SaveFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //arrayJson을 Json으로 변환
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData를 파일로 저장
        File.WriteAllText(Application.dataPath + "/" + roomName + ".txt", jsonData);
    }

    void LoadFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/" + roomName + ".txt");
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
        SaveRoomInfo(arrayJson.XSize, arrayJson.YSize, arrayJson.ZSize, arrayJson.balcony);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        //해당 위치에 BlueCube를 생성해서 놓는다.
        foreach (GameObject go in objects.datas)
        {
            if (go.GetComponent<Deco_Idx>().Idx == idx)
            {
                GameObject obj = Instantiate(go); 
                obj.transform.localPosition = position;
                obj.transform.localEulerAngles = eulerAngle;
                obj.transform.localScale = localScale;
                obj.transform.parent = room;
            }
        }
    }
}
