using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class UrlInfo
{
    public const string url = "http://54.180.108.64:80/v1";
    public const string _url = "http://54.180.108.64:80/";
    public const string chatUrl = "http://34.64.60.123:5000/";
    public const string cropUrl = "http://34.64.70.4:5000/";
    //public const string url = "http://192.168.0.243:8000/v1";
    //public const string _url = "http://192.168.0.243:8000/";
    //public const string url = "http://172.16.20.63:8000/v1";
}

[Serializable]
public class SaveRoomNo
{
    public int data;
}

[Serializable]
public class AdvLightInfo
{
    public Vector4 shadowVal = new Vector4();
    public Vector4 midtoneVal = new Vector4();
    public Vector4 highlightVal = new Vector4();
    public float contrast = 0;
    public float postExposure = 0;
    public float hueShift = 0;
    public float saturation = 0;
    public float temp = 0;
    public float tint = 0;
    public Color colorFilter;
}

[Serializable]
public class LightInfo
{
    public bool spot;
    public float innerAngle;
    public float outerAngle;
    public Color color;
    public float intensity;
    public float range;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

[Serializable]
public class SaveJsonInfo
{
    public int idx;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

[Serializable]
public class ArrayJsonWrapper
{
    public string statusCode;
    public string message;
    public ArrayJson data;
}

[Serializable]
public class SaveJsonInfo_Get
{
    public int funitureArrangementNo;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

[Serializable]
public class ArrayJson
{
    public int roomNo;
    public string roomName;
    public bool access = false;
    public string category = "";
    public string description = "";
    public float xsize;
    public float ysize;
    public float zsize;
    public int door = 0;
    public List<SaveJsonInfo> datas;
    public List<LightInfo> lights;
    public AdvLightInfo filter;
}

[Serializable]
public class ArrayJson_First
{
    public int roomNo;
    public string roomName;
    public bool access = false;
    public string category = "";
    public string description = "";
    public float xsize;
    public float ysize;
    public float zsize;
    public int door = 0;
}

[Serializable]
public class ArrayJson_Get
{
    public int roomNo;
    public string roomName = "";
    public bool access = false;
    public string category = "";
    public string description = "";
    public float xsize;
    public float ysize;
    public float zsize;
    public int door = 0;
    //public byte[] imgData;
    public List<SaveJsonInfo> furnitureArrangementList;
}

public class Deco_Json : MonoBehaviour
{

    public InputField saveInputField;
    public InputField loadInputField;

    public GameObject ptLight;
    public GameObject sptLight;

    public static Deco_Json Instance { get; set; }
    ArrayJson arrayJson;
    ArrayJson arrayJsonLoad;
    SaveRoomNo saveRoomNo;
    public AdvLightInfo advLightInfo;

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
        arrayJson.lights = new List<LightInfo>();
        arrayJsonLoad = new ArrayJson();
        arrayJsonLoad.datas = new List<SaveJsonInfo>();
        advLightInfo = new AdvLightInfo();

        saveRoomNo = new SaveRoomNo();

        // ���� �ҷ��� ��� �ҷ��� ���� ID�� ����
        if (Deco_LoadRoomList.Instance != null)
            saveRoomNo.data = Deco_LoadRoomList.Instance.ID;
    }

    private void Start()
    {
        //LightInfo light = new LightInfo();
        //string test = JsonUtility.ToJson(light, true);
        //File.WriteAllText(Application.dataPath + "/test.txt", test);
        Directory.CreateDirectory(Application.dataPath + "/RoomInfo");
    }

    // ���� �۾����� ���� ������ ����
    public void SaveRoomInfo(string roomName, float x, float y, float z, int bal)
    {
        arrayJson.roomName = roomName;
        arrayJson.xsize = x;
        arrayJson.ysize = y;
        arrayJson.zsize = z;
        arrayJson.door = bal;
    }

    // ó�� ���� ������ �� ��Ʈ��ũ�� ���� �̸�, ũ��, �� ������ �ѱ�
    public IEnumerator FirstPost(string url, string roomName, float x, float y, float z, int bal)
    {
        WWWForm form = new WWWForm();
        form.AddField("roomName", roomName);
        form.AddField("access", "false");
        form.AddField("category", "");
        form.AddField("description", "");
        form.AddField("xsize", x.ToString());
        form.AddField("ysize", y.ToString());
        form.AddField("zsize", z.ToString());
        form.AddField("door", bal.ToString());

        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(800, 800, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(560, 140, 800, 800), 0, 0, false);
        texture.Apply();
        byte[] img = texture.EncodeToPNG();

        form.AddBinaryData("screenShot", img);


        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                saveRoomNo = JsonUtility.FromJson<SaveRoomNo>(www.downloadHandler.text);
                Debug.Log("roomInfo Upload complete!");
            }
            www.Dispose();
        }
    }

    public void SaveJson(GameObject go, int id)
    {
        SaveJsonInfo info;
        
        info = new SaveJsonInfo();
        info.idx = id;
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

    public void SaveLightJson(LightInfo info)
    {
        //ArrayJson �� datas �� �ϳ��� �߰�
        arrayJson.lights.Add(info);
    }

    // ���� ������ �� �� �̸��� ��ũ���� ����
    public void SaveNewFile(string roomName)
    {
        if (roomName != null)
        {
            // �� �̸� ����
            arrayJson.roomName = roomName;
        }

        //// JsonData�� ���÷� ����
        //string jsonData = JsonUtility.ToJson(arrayJson, true);
        //File.WriteAllText(Application.dataPath + "/RoomInfo" + "/" + arrayJson.roomName + ".txt", jsonData);
        //// ��ũ������ ���÷� ����
        //string path = Application.dataPath + "/RoomInfo/" + arrayJson.roomName + ".png";
        //File.WriteAllBytes(path, imgData);

        // ���� �������� �� ���� ������ ���� ��ġ ������ ���� jsonData�� ��ũ������ ��Ʈ��ũ�� ����
        StartCoroutine(OnPutJson(UrlInfo.url + "/rooms", saveRoomNo.data, arrayJson));
    }

    // ������ �� ������ ������ Json �������� ���ε�
    IEnumerator OnPutJson(string uri, int id, ArrayJson arrayJson)
    {
        if (Deco_UIManager.Instance.ImageBytes == null)
        {
            JH_PopUpUI.Instance.SetUI("Warning!", "ScreenShot is not Captured!", false);
            yield break;
        }

        ArrayJson_First firstJson = new ArrayJson_First();
        firstJson.roomNo = id;
        if (arrayJson.roomName != null)
            firstJson.roomName = arrayJson.roomName;
        firstJson.access = arrayJson.access;
        firstJson.category = arrayJson.category;
        if (arrayJson.description != null)
            firstJson.description = arrayJson.description;
        firstJson.xsize = arrayJson.xsize;
        firstJson.ysize = arrayJson.ysize;
        firstJson.zsize = arrayJson.zsize;
        firstJson.door = arrayJson.door;

        string jsonData = JsonUtility.ToJson(firstJson, true);
        Debug.Log(jsonData);

        using (UnityWebRequest www = UnityWebRequest.Put(uri + "/" + id.ToString(), jsonData))
        {
            {
                www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

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
                    JH_PopUpUI.Instance.SetUI("", "Room Upload Complete!", false, 0.5f, "Main");
                    Debug.Log("Room Put complete!");
                    yield return new WaitForSeconds(1f);
                }
            }

            www.Dispose();
        }

        SaveJsonInfo[] datas = arrayJson.datas.ToArray();
        string datasString = JsonHelper.ToJsons(datas);
        Debug.Log(datasString);

        using (UnityWebRequest www = UnityWebRequest.Put(uri + "/" + id.ToString() + "/furniture", datasString))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(datasString);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Furnit Put complete!");
                }
            }

            www.Dispose();
        }

        LightInfo[] lights = arrayJson.lights.ToArray();
        string lightsString = JsonHelper.ToJsonl(lights);
        Debug.Log(lightsString);

        using (UnityWebRequest www = UnityWebRequest.Post(uri + "/" + id.ToString() + "/lightings", lightsString))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(lightsString);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Lights Put complete!");
                }
            }

            www.Dispose();
        }

        WWWForm form = new WWWForm();
        form.AddBinaryData("screenShot", Deco_UIManager.Instance.ImageBytes);

        using (UnityWebRequest www = UnityWebRequest.Post(uri + "/" + id.ToString() + "/screenShot", form))
        {
            {
                www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("ScreenShot Put complete!");
                }
            }

            www.Dispose();
        }

        string filterinfo = JsonUtility.ToJson(advLightInfo, true);
        Debug.Log(filterinfo);

        using (UnityWebRequest www = UnityWebRequest.Put(uri + "/" + id.ToString() + "/filter", filterinfo))
        {
            {
                byte[] jsonSend = new System.Text.UTF8Encoding().GetBytes(filterinfo);
                www.uploadHandler = new UploadHandlerRaw(jsonSend);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Filter Put complete!");
                }
            }

            www.Dispose();
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

    //���÷� �� �ҷ�����
    public void LoadFile(string roomName)
    {
        if (roomName == null)
            return;
        //mapData.txt�� �ҷ�����
        string jsonData = File.ReadAllText(Application.dataPath + "/RoomInfo" + "/" + roomName + ".txt");
        //ArrayJson ���·� Json�� ��ȯ
        arrayJsonLoad = JsonUtility.FromJson<ArrayJson>(jsonData);
        //ArrayJson�� �����ͷ� �� ����
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

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        // ������ �������� ��, ���鿡 �´� �� ����
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }
        }
        SaveRoomInfo(roomName, arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door);
    }

    // ��Ʈ��ũ�� �� �ҷ�����
    public void LoadFile(int id)
    {
        // �� ������ �������� �������� �޾ƿ�
        StartCoroutine(LoadJson(UrlInfo.url + "/rooms/" + id.ToString()));
    }

    void LoadObject(int id, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        StartCoroutine(WaitForDownLoad(UrlInfo.url + "/products/" + id.ToString(), position, eulerAngle, localScale, room, id));
    }

    void LoadLight(LightInfo info)
    {
        GameObject go;
        if (info.spot)
        {
            go = Instantiate(sptLight);
            Light light = go.GetComponent<Light>();
            light.innerSpotAngle = info.innerAngle;
            light.spotAngle = info.outerAngle;
            light.color = info.color;
            light.intensity = info.intensity;
            light.range = info.range;
            go.transform.position = info.position;
            go.transform.eulerAngles = info.eulerAngle;
            go.transform.localScale = info.localScale;
        }
        else
        {
            go = Instantiate(ptLight);
            Light light = go.GetComponent<Light>();
            //light.innerSpotAngle = info.innerAngle;
            //light.spotAngle = info.outerAngle;
            light.color = info.color;
            light.intensity = info.intensity;
            light.range = info.range;
            go.transform.position = info.position;
            go.transform.eulerAngles = info.eulerAngle;
            go.transform.localScale = info.localScale;
        }

        SaveLightJson(info);
    }

    // ������ ���� id�� ��û�ؼ� ������ ������ �޾ƿ��� �����ϴ� �Լ�
    IEnumerator WaitForDownLoad(string uri, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, int id = 0)
    {
        FBXJson fbxJson = new FBXJson();

        // ���� ID�� ��û�ؼ� ������ ������ �޾ƿ�
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

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
            www.Dispose();
        }

        // �޾ƿ� ���� ������ ����ؼ� ���� ����
        using (UnityWebRequest www = UnityWebRequest.Get(fbxJson.fileUrl))
        {
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                string path = Application.persistentDataPath + "/" +fbxJson.no.ToString() + ".zip";

                if (!File.Exists(path))
                    File.WriteAllBytes(path, www.downloadHandler.data);

                while (!File.Exists(path))
                {
                    yield return null;
                }

                if (!Directory.Exists(Application.dataPath + "/LocalServer/" + fbxJson.no + "/"))
                    Directory.CreateDirectory(Application.dataPath + "/LocalServer/" + fbxJson.no + "/");
                ZipManager.UnZipFiles(path, Application.dataPath + "/LocalServer/" + fbxJson.no + "/", "", false);

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
            www.Dispose();
        }
    }

    // ������ �� id�� ��û�ؼ� ���� ������ Json �������� �޾ƿ��� �Լ�
    IEnumerator LoadJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                ArrayJsonWrapper wrapper = JsonUtility.FromJson<ArrayJsonWrapper>(www.downloadHandler.text);
                arrayJsonLoad = wrapper.data;
                Debug.Log("ArrayJson Download complete!");
            }
            www.Dispose();
        }

        //ArrayJson�� �����ͷ� �� ����
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

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }

            for (int i = 0; i < arrayJsonLoad.lights.Count; i++)
            {
                LightInfo info = arrayJsonLoad.lights[i];
                LoadLight(info);
            }

            AdvLightInfo filter = arrayJsonLoad.filter;
            PostProcessTest.Instance.SetRoomFilter(filter.shadowVal, filter.midtoneVal, filter.highlightVal,
                filter.contrast, filter.postExposure, filter.hueShift, filter.saturation, filter.colorFilter, filter.temp, filter.tint);
            arrayJson.filter = filter;
        }
        // ������ �������� ��, ���鿡 �´� �� ����
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }

            for (int i = 0; i < arrayJsonLoad.lights.Count; i++)
            {
                LightInfo info = arrayJsonLoad.lights[i];
                LoadLight(info);
            }

            AdvLightInfo filter = arrayJsonLoad.filter;
            PostProcessTest.Instance.SetRoomFilter(filter.shadowVal, filter.midtoneVal, filter.highlightVal, 
                filter.contrast, filter.postExposure, filter.hueShift, filter.saturation, filter.colorFilter, filter.temp, filter.tint);
            arrayJson.filter = filter;
        }
        SaveRoomInfo(arrayJsonLoad.roomName, arrayJsonLoad.xsize, arrayJsonLoad.ysize, arrayJsonLoad.zsize, arrayJsonLoad.door);
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
        col.size = new Vector3(fbxJson.xsize, fbxJson.ysize, fbxJson.zsize);
        //col.center = new Vector3(0, fbxJson.ysize / 2, 0);

        if (go.transform.up.x > 0)
            col.center += go.transform.up * fbxJson.xsize / 2;
        else if (go.transform.up.y > 0)
            col.center += go.transform.up * fbxJson.ysize / 2;
        else if (go.transform.up.z > 0)
            col.center += go.transform.up * fbxJson.zsize / 2;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if (fbxJson.location)
            go.transform.localPosition = Vector3.zero;
        else if (!fbxJson.location)
            go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zsize / 2 + 0.01f);
        //go.transform.localRotation = Quaternion.identity;
        Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
        decoIdx.Name = fbxJson.furnitName;
        decoIdx.Price = fbxJson.price;
        decoIdx.Category = fbxJson.category;
        
        SaveJson(obj, fbxJson.no);

        MaterialLoader.Instance.ChangeMat(go.transform, Application.dataPath + "/LocalServer/" + fbxJson.no.ToString());

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

