using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TriLibCore;

public class Show_Json : MonoBehaviourPun
{
    public static Show_Json Instance;
    public InputField loadInputField; 
    public Objects objects;
    ArrayJson arrayJson;
    ArrayJson arrayJsonLoad;
    Dictionary<Vector3, GameObject> objs = new Dictionary<Vector3, GameObject>();

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
                arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
                if (arrayJson.access)
                {
                    LoadFile(fileName);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        Deco_RoomInit.Instance.MakeRoom(arrayJsonLoad.XSize, arrayJsonLoad.YSize, arrayJsonLoad.ZSize, arrayJsonLoad.balcony, newRoom.transform);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJsonLoad.datas[i];
            LoadObject(info.idx, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        // id를 보냄
        // id에 해당하는 fbx 파일과 Json파일을 받음
        // 받은 정보와 포지션, 앵글, 스케일 값을 이용해서 생성

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Contains("txt") && !file.Name.Contains("meta"))
            {
                FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
                if (fbxJson.id == idx)
                {
                    StartCoroutine(WaitForFile(position, file.FullName.Substring(0, file.FullName.Length - 4) + ".fbx", position, eulerAngle, localScale, room, fbxJson));
                    //var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                    //AssetLoader.LoadModelFromFile(file.FullName.Substring(0, file.FullName.Length - 4) + ".fbx", OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
                }
            }
        }
    }

    IEnumerator WaitForFile(Vector3 pos, string path, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, FBXJson fbxJson)
    {
        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

        while (!objs.ContainsKey(pos))
        {
            yield return null;
        }

        GameObject obj = objs[pos];
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
        decoIdx.Idx = fbxJson.id;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (File.Exists(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"))
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"));
                go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = tex;
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