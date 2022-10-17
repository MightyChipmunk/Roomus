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
    }

    private void Start()
    {
        arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        saveInputField.onSubmit.AddListener(SaveFile);
        loadInputField.onSubmit.AddListener(LoadFile);
    }

    public void SaveJson(GameObject go, int idx)
    {
        SaveJsonInfo info;
        
        info = new SaveJsonInfo();
        info.idx = idx;
        info.position = go.transform.position;
        info.eulerAngle = go.transform.eulerAngles;
        info.localScale = go.transform.localScale;

        //ArrayJson �� datas �� �ϳ��� �߰�
        arrayJson.datas.Add(info);
    }

    void SaveFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //arrayJson�� Json���� ��ȯ
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData�� ���Ϸ� ����
        File.WriteAllText(Application.dataPath + "/" + roomName + ".txt", jsonData);
    }

    void LoadFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //mapData.txt�� �ҷ�����
        string jsonData = File.ReadAllText(Application.dataPath + "/" + roomName + ".txt");
        //ArrayJson ���·� Json�� ��ȯ
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson�� �����͸� ������ ������Ʈ ����
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale);
        }
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale)
    {
        //�ش� ��ġ�� BlueCube�� �����ؼ� ���´�.
        foreach (GameObject go in objects.datas)
        {
            if (go.GetComponent<Deco_Idx>().Idx == idx)
            {
                GameObject obj = Instantiate(go); 
                obj.transform.position = position;
                obj.transform.eulerAngles = eulerAngle;
                obj.transform.localScale = localScale;
                obj.transform.parent = GameObject.Find("Room").transform;
            }
        }
    }
}
