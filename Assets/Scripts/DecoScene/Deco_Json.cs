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

    public void SaveNewFile(string roomName)
    {
        if (roomName.Length == 0)
            return;
        //방 이름 변경
        arrayJson.roomName = roomName;
        //arrayJson을 Json으로 변환
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //jsonData를 파일로 저장
        //File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt", jsonData);
        // jsonData를 네트워크로 전달
        StartCoroutine(OnPostJson("http://192.168.0.243:8000/v1/products", jsonData));
    }

    // 방 정보를 서버에 Json 형식으로 업로드
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
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson 형태로 Json을 변환
        arrayJsonLoad = JsonUtility.FromJson<ArrayJson>(jsonData);
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
        Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony, newRoom.transform);
        SaveRoomInfo(roomName, arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJsonLoad.datas[i];
            LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }

    public void LoadFile(int id)
    {
        // 방 가구의 정보들을 서버에서 받아옴
        StartCoroutine(LoadJson("http://192.168.0.243:8000/v1/products", id));
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
        Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony, newRoom.transform);
        SaveRoomInfo(arrayJsonLoad.roomName, arrayJsonLoad.xSize, arrayJsonLoad.ySize, arrayJsonLoad.zSize, arrayJsonLoad.balcony);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJsonLoad.datas[i];
            LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }

    void LoadObject(int id, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        StartCoroutine(WaitForDownLoad("http://192.168.0.243:8000/v1/products", position, eulerAngle, localScale, room, id));
    }

    // 서버에 가구 id를 요청해서 가구의 정보를 받아오고 생성하는 함수
    IEnumerator WaitForDownLoad(string uri, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, int id = 0)
    {
        FBXJson fbxJson = new FBXJson();

        // id로 요청해서 Json 형식으로 정보를 가져옴
        using (UnityWebRequest www = UnityWebRequest.Post(uri, id.ToString()))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                fbxJson = JsonUtility.FromJson<FBXJson>(www.downloadHandler.text);
                Debug.Log("FBXJson Download complete!");
            }
        }

        // 가져온 Json 데이터의 url로 Get 요청을 해서 가구의 zip파일을 가져오고 생성함
        using (UnityWebRequest www = AssetDownloader.CreateWebRequest(fbxJson.url, AssetDownloader.HttpRequestMethod.Get))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                GameObject wrapper = new GameObject();
                wrapper.transform.parent = room;
                wrapper.transform.localPosition = position;
                wrapper.transform.localEulerAngles = eulerAngle;
                wrapper.transform.localScale = localScale;
                Deco_WrapperData wrapperData = wrapper.AddComponent<Deco_WrapperData>();
                wrapperData.jsonData = JsonUtility.ToJson(fbxJson);
                AssetDownloader.LoadModelFromUri(www, OnLoad, OnMaterialsLoad, OnProgress, OnError, wrapper, assetLoaderOptions,
                    null, null);
                Debug.Log("FBX.zip Download complete!");
            }
        }
    }

    // 서버에 방 id로 요청해서 방의 정보를 Json 형식으로 받아오는 함수
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
                Debug.Log("ArrayJson Download complete!");
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

    /// <summary>
    /// Called when the Model (including Textures and Materials) has been fully loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");

        FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(assetLoaderContext.WrapperGameObject.GetComponent<Deco_WrapperData>().jsonData);

        GameObject obj = assetLoaderContext.RootGameObject;
        obj.transform.parent = assetLoaderContext.WrapperGameObject.transform.parent;
        obj.transform.localPosition = assetLoaderContext.WrapperGameObject.transform.position;
        obj.transform.localEulerAngles = assetLoaderContext.WrapperGameObject.transform.eulerAngles;
        obj.transform.localScale = assetLoaderContext.WrapperGameObject.transform.localScale;
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
        
        SaveJson(obj, obj.GetComponent<Deco_Idx>().Id);

        Destroy(assetLoaderContext.WrapperGameObject);
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

