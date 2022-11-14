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
    public int no;
    public string roomName;
    public bool access = false;
    public string category = "";
    public string description = "";
    public float xsize;
    public float ysize;
    public float zsize;
    public int door = 0;
    //public byte[] imgData;
    public List<SaveJsonInfo> datas;
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
        arrayJsonLoad = new ArrayJson();
        arrayJsonLoad.datas = new List<SaveJsonInfo>();

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
        arrayJson.xsize = x;
        arrayJson.ysize = y;
        arrayJson.zsize = z;
        arrayJson.door = bal;

        //StartCoroutine(FirstPost("", roomName, x, y, z, bal));
    }

    IEnumerator FirstPost(string url, string roomName, float x, float y, float z, int bal)
    {
        WWWForm form = new WWWForm();

        form.AddField("roomName", roomName);
        form.AddField("xsize", x.ToString());
        form.AddField("ysize", y.ToString());
        form.AddField("zsize", z.ToString());
        form.AddField("balcony", bal);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("roomInfo Download complete!");
            }
        }
    }

    public void SaveJson(GameObject go, int id)
    {
        SaveJsonInfo info;
        
        info = new SaveJsonInfo();
        info.id = id;
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
        if (roomName != null)
        {
            // 방 이름 변경
            arrayJson.roomName = roomName;
        }
        
        // JsonData를 로컬로 저장
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".txt", jsonData);

        // jsonData를 네트워크로 전달
        //StartCoroutine(OnPostJson("http://54.180.108.64:80/v1/products", arrayJson));
    }

    public IEnumerator WaitForScreenShot()
    {
        yield return new WaitForEndOfFrame();

        // 방의 스크린샷을 찍어서 바이너리 데이터로 저장
        byte[] imgBytes;
        Texture2D texture = new Texture2D(800, 800, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(560, 140, 800, 800), 0, 0, false);
        texture.Apply();
        imgBytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".png", imgBytes);

        if (!File.Exists(Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".png"))
        {
            yield return null;
        }

        Deco_UIManager.Instance.EndScreenShot();
    }

    // 방 정보를 서버에 Json 형식으로 업로드
    IEnumerator OnPostJson(string uri, ArrayJson arrayJson)
    {
        WWWForm form = new WWWForm();

        SaveJsonInfo[] datas = arrayJson.datas.ToArray();
        string datasString = JsonHelper.ToJson(datas);

        yield return new WaitForEndOfFrame();

        // 방의 스크린샷을 찍어서 바이너리 데이터로 저장
        byte[] imgBytes;
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
        imgBytes = texture.EncodeToPNG();

        form.AddBinaryData("screenShot", imgBytes);
        form.AddField("roomName", arrayJson.roomName);
        form.AddField("access", arrayJson.access.ToString());
        if (arrayJson.category != null)
            form.AddField("category", arrayJson.category);
        if (arrayJson.description != null)
            form.AddField("description", arrayJson.description);
        form.AddField("xsize", arrayJson.xsize.ToString());
        form.AddField("ysize", arrayJson.ysize.ToString());
        form.AddField("zsize", arrayJson.zsize.ToString());
        form.AddField("door", arrayJson.door.ToString());
        form.AddField("datas", datasString);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
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

        string path = Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".png";
        if (File.Exists(path))
        {
            File.WriteAllBytes(Application.dataPath + "/RoomInfo" + "/" + roomName + ".png", File.ReadAllBytes(path));
        }

        SaveNewFile(roomName);
    }

    //로컬로 방 불러오기
    public void LoadFile(string roomName)
    {
        if (roomName == null)
            return;
        //mapData.txt를 불러오기
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson 형태로 Json을 변환
        arrayJsonLoad = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson의 데이터로 방 생성
        if (arrayJsonLoad.xsize > 0)
        {
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
            Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door, newRoom.transform);

            //ArrayJson의 데이터를 가지고 오브젝트 생성
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        // 도면을 선택했을 시, 도면에 맞는 방 생성
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";

            //ArrayJson의 데이터를 가지고 오브젝트 생성
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        SaveRoomInfo(roomName, arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door);
    }

    // 네트워크로 방 불러오기
    public void LoadFile(int id)
    {
        // 방 가구의 정보들을 서버에서 받아옴
        StartCoroutine(LoadJson("http://54.180.108.64:80/v1/products/" + id.ToString()));
        //ArrayJson의 데이터로 방 생성
        if (arrayJsonLoad.xsize > 0)
        {
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
            Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door, newRoom.transform);

            //ArrayJson의 데이터를 가지고 오브젝트 생성
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        // 도면을 선택했을 시, 도면에 맞는 방 생성
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";

            //ArrayJson의 데이터를 가지고 오브젝트 생성
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        SaveRoomInfo(arrayJsonLoad.roomName, arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door);
    }

    void LoadObject(int id, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        StartCoroutine(WaitForDownLoad("http://54.180.108.64:80/v1/products/" + id.ToString(), position, eulerAngle, localScale, room, id));
    }

    // 서버에 가구 id를 요청해서 가구의 정보를 받아오고 생성하는 함수
    IEnumerator WaitForDownLoad(string uri, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, int id = 0)
    {
        FBXJson fbxJson = new FBXJson();

        //// 테스트용으로 임의의 값을 넣음
        //fbxJson.furnitName = "Test";
        //fbxJson.price = 100000;
        //fbxJson.location = true;
        //fbxJson.xsize = 1;
        //fbxJson.ysize = 0.75f;
        //fbxJson.zsize = 1;
        //fbxJson.no = 1;
        //fbxJson.category = "Bed";

        // 가구 ID로 요청해서 가구의 정보를 받아옴
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                FBXWrapper wrapper = JsonUtility.FromJson<FBXWrapper>(www.downloadHandler.text);
                fbxJson = wrapper.data;

                //FBXJson[] data = JsonHelper.FromJson<FBXJson>(www.downloadHandler.text);
                Debug.Log("FBXJson Download complete!");
            }
        }

        // 받아온 가구 정보를 사용해서 가구 생성
        using (UnityWebRequest www = UnityWebRequest.Get(fbxJson.fileUrl))
        //using (UnityWebRequest www = UnityWebRequest.Get("https://s3.ap-northeast-2.amazonaws.com/roomus-s3/product/zip/p_6ae2e248-91c5-4d9a-bc53-396346bcec04.octet-stream"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                string path = Application.persistentDataPath + fbxJson.furnitName + ".zip";

                if (!File.Exists(path))
                    File.WriteAllBytes(path, www.downloadHandler.data);

                while (!File.Exists(path))
                {
                    yield return null;
                }

                GameObject wrapper = new GameObject();
                wrapper.transform.parent = room;
                wrapper.transform.localPosition = position;
                wrapper.transform.localEulerAngles = eulerAngle;
                wrapper.transform.localScale = localScale;
                Deco_WrapperData wrapperData = wrapper.AddComponent<Deco_WrapperData>();
                wrapperData.jsonData = JsonUtility.ToJson(fbxJson);
                AssetLoaderZip.LoadModelFromZipFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, wrapper, assetLoaderOptions,
                    null, null);
                Debug.Log("FBX.zip Download complete!");
            }
        }
    }

    // 서버에 방 id로 요청해서 방의 정보를 Json 형식으로 받아오는 함수
    IEnumerator LoadJson(string uri)
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
        obj.transform.localPosition = assetLoaderContext.WrapperGameObject.transform.localPosition;
        obj.transform.localEulerAngles = assetLoaderContext.WrapperGameObject.transform.localEulerAngles;
        obj.transform.localScale = assetLoaderContext.WrapperGameObject.transform.localScale;
        GameObject go = obj.transform.GetChild(0).gameObject;
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.center = new Vector3(0, fbxJson.ysize / 2, 0);
        col.size = new Vector3(fbxJson.xsize, fbxJson.ysize, fbxJson.zsize);
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if (fbxJson.location)
            go.transform.localPosition = Vector3.zero;
        else if (!fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zsize / 2 + 0.01f);
        go.transform.localRotation = Quaternion.identity;
        Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
        decoIdx.Name = fbxJson.furnitName;
        decoIdx.Price = fbxJson.price;
        decoIdx.Category = fbxJson.category;
        
        SaveJson(obj, fbxJson.no);

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

