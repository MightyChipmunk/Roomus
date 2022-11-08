using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class SaveJsonInfo
{
    public int id;
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
    public float xSize;
    public float ySize;
    public float zSize;
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
    ArrayJson arrayJsonLoad;

    Dictionary<Vector3, GameObject> objs = new Dictionary<Vector3, GameObject>();

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
        arrayJson.xSize = x;
        arrayJson.ySize = y;
        arrayJson.zSize = z;
        arrayJson.balcony = bal;
    }

    public void SaveJson(GameObject go, int idx)
    {
        SaveJsonInfo info;
        
        info = new SaveJsonInfo();
        info.id = idx;
        info.position = go.transform.position;
        info.eulerAngle = go.transform.eulerAngles;
        info.localScale = go.transform.localScale;

        //ArrayJson �� datas �� �ϳ��� �߰�
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

    public void SaveNewFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //�� �̸� ����
        arrayJson.roomName = roomName;
        //arrayJson�� Json���� ��ȯ
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData�� ���Ϸ� ����
        File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt", jsonData);
        //jsonData�� ��Ʈ��ũ�� ����
        StartCoroutine(OnPostJson("http://192.168.0.243:8000/v1/products", jsonData));
    }

    IEnumerator OnPostJson(string uri, string jsonData)
    {
        //WWWForm form = new WWWForm();
        //form.AddField("", jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
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
        //mapData.txt�� �ҷ�����
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson ���·� Json�� ��ȯ
        arrayJsonLoad = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson�� �����ͷ� �� ����
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
        Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony, newRoom.transform);
        SaveRoomInfo(roomName, arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony);
        //ArrayJson�� �����͸� ������ ������Ʈ ����
        for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJsonLoad.datas[i];
            LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }

    public void LoadFile(int id)
    {
        StartCoroutine(LoadJson("http://192.168.0.243:8000/v1/products", id));
        //ArrayJson�� �����ͷ� �� ����
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
        Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony, newRoom.transform);
        SaveRoomInfo(arrayJsonLoad.roomName, arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony);
        //ArrayJson�� �����͸� ������ ������Ʈ ����
        for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJsonLoad.datas[i];
            LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }

    void LoadObject(int id, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        // id�� ����
        // id�� �ش��ϴ� fbx ���ϰ� Json������ ����
        // ���� ������ ������, �ޱ�, ������ ���� �̿��ؼ� ����

        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        //foreach (FileInfo file in di.GetFiles())
        //{
        //    if (file.Name.Contains("txt") && !file.Name.Contains("meta"))
        //    {
        //        FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
        //        //if (fbxJson.id == idx)
        //        //{
        //        //    StartCoroutine(WaitForFile(file.FullName.Substring(0, file.FullName.Length - 4) + ".fbx", position, eulerAngle, localScale, room, fbxJson));
        //        //}
        //    }
        //}

        StartCoroutine(WaitForDownLoad("http://192.168.0.243:8000/v1/products", position, eulerAngle, localScale, room, id));
    }

    IEnumerator WaitForFile(string path, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, FBXJson fbxJson)
    {
        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

        while (!objs.ContainsKey(position))
        {
            yield return null;
        }

        GameObject obj = objs[position];
        obj.transform.parent = room;
        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = eulerAngle;
        obj.transform.localScale = localScale;
        GameObject go = obj.transform.GetChild(0).gameObject;
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.center = new Vector3(0, fbxJson.ySize / 2, 0);
        col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if (fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward;
        else if (!fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f);
        go.transform.localRotation = Quaternion.identity;
        Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
        decoIdx.Name = fbxJson.furnitName;
        decoIdx.Price = fbxJson.price;
        decoIdx.Category = fbxJson.category;
        //decoIdx.Idx = fbxJson.id;

        //for (int i = 0; i < go.transform.childCount; i++)
        //{
        //    go.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
        //}

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (File.Exists(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"))
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"));
                go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = tex;
            }
        }

        SaveJson(obj, obj.GetComponent<Deco_Idx>().Id);
    }

    IEnumerator WaitForDownLoad(string uri, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, int id = 0)
    {
        using (UnityWebRequest www = AssetDownloader.CreateWebRequest(uri, AssetDownloader.HttpRequestMethod.Post, id.ToString() + "fbx"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                AssetDownloader.LoadModelFromUri(www, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions,
                    null, null);
            }
        }

        FBXJson fbxJson = new FBXJson();

        using (UnityWebRequest www = UnityWebRequest.Post(uri, id.ToString() + "json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                fbxJson = JsonUtility.FromJson<FBXJson>(www.downloadHandler.text);
            }
        }

        while (!objs.ContainsKey(position))
        {
            yield return null;
        }

        GameObject obj = objs[position];
        obj.transform.parent = room;
        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = eulerAngle;
        obj.transform.localScale = localScale;
        GameObject go = obj.transform.GetChild(0).gameObject;
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.center = new Vector3(0, fbxJson.ySize / 2, 0);
        col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if (fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward;
        else if (!fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f);
        go.transform.localRotation = Quaternion.identity;
        Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
        decoIdx.Name = fbxJson.furnitName;
        decoIdx.Price = fbxJson.price;
        decoIdx.Category = fbxJson.category;
        //decoIdx.Idx = fbxJson.id;

        //for (int i = 0; i < go.transform.childCount; i++)
        //{
        //    go.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
        //}

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (File.Exists(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"))
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"));
                go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = tex;
            }
        }

        SaveJson(obj, obj.GetComponent<Deco_Idx>().Id);
    }

    IEnumerator LoadJson(string uri, int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, id.ToString() + "roomJson"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                arrayJsonLoad = JsonUtility.FromJson<ArrayJson>(www.downloadHandler.text);
            }
        }
    }

    #region Trilib
    /// <summary>
    /// Called when any error occurs.
    /// </summary>
    /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
    private void OnError(IContextualizedError obj)
    {
        Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
    }

    /// <summary>
    /// Called when the Model loading progress changes.
    /// </summary>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    /// <param name="progress">The loading progress.</param>
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log($"Loading Model. Progress: {progress:P}");
    }

    int objIdx = 0;
    /// <summary>
    /// Called when the Model (including Textures and Materials) has been fully loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");
        objs.Add(arrayJsonLoad.datas[objIdx++].position, assetLoaderContext.RootGameObject);
    }

    /// <summary>
    /// Called when the Model Meshes and hierarchy are loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Model loaded. Loading materials.");
    }
    #endregion
}

