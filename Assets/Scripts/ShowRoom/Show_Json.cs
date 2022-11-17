using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TriLibCore;
using UnityEngine.Networking;

public class Show_Json : MonoBehaviourPun
{
    public static Show_Json Instance;

    GameObject player;

    //public Objects objects;
    ArrayJson arrayJsonLoad;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        player.name = PhotonNetwork.NickName;
        if (player.GetComponent<PhotonView>().IsMine)
        {
            ChangeLayersRecursively(player.transform, "Player");
        }
        PhotonNetwork.Instantiate("CamFollow", Vector3.zero, Quaternion.identity);
    }

    public void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }

    void Start()
    {
        // ���÷� �� �ҷ�����
        LoadFile(Show_LoadRoomList.Instance.RoomName);
        // ��Ʈ��ũ�� �� �ҷ�����
        //LoadFile(Show_LoadRoomList.Instance.ID);
        Destroy(Show_LoadRoomList.Instance.gameObject);
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
        JH_MoreInfoManager.Instance.arrayJson = arrayJsonLoad;
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
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }

            newRoom.AddComponent<PhotonView>();
            Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
            infoUI.x = arrayJsonLoad.xsize;
            infoUI.y = arrayJsonLoad.ysize;
            infoUI.category = arrayJsonLoad.category;
            infoUI.description = arrayJsonLoad.description;
        }
        // ������ �������� ��, ���鿡 �´� �� ����
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";
            //newRoom.transform.position = new Vector3(1.2f, 0, 1.9f);

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }

            newRoom.AddComponent<PhotonView>();
            Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
            infoUI.x = arrayJsonLoad.xsize;
            infoUI.y = arrayJsonLoad.ysize;
            infoUI.roomName = arrayJsonLoad.roomName;
            infoUI.category = arrayJsonLoad.category;
            infoUI.description = arrayJsonLoad.description;
        }
    }

    // ��Ʈ��ũ�� �� �ҷ�����
    public void LoadFile(int id)
    {
        // �� ������ �������� �������� �޾ƿ�
        StartCoroutine(LoadJson("http://54.180.108.64:80/v1/products/" + id.ToString()));
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
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
            }

            newRoom.AddComponent<PhotonView>();
            Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
            infoUI.x = arrayJsonLoad.xsize;
            infoUI.y = arrayJsonLoad.ysize;
            infoUI.category = arrayJsonLoad.category;
            infoUI.description = arrayJsonLoad.description;
        }
        // ������ �������� ��, ���鿡 �´� �� ����
        else
        {
            Destroy(GameObject.Find("Room"));

            GameObject newRoom = Instantiate(Resources.Load<GameObject>("Room" + arrayJsonLoad.door.ToString()));
            newRoom.name = "Room";

            player.transform.position = new Vector3(1.2f, 0, 2f);

            //ArrayJson�� �����͸� ������ ������Ʈ ����
            for (int i = 0; i < arrayJsonLoad.datas.Count; i++)
            {
                SaveJsonInfo info = arrayJsonLoad.datas[i];
                LoadObject(info.id, info.position, info.eulerAngle, info.localScale * 10, newRoom.transform);
            }

            newRoom.AddComponent<PhotonView>();
            Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
            infoUI.x = arrayJsonLoad.xsize;
            infoUI.y = arrayJsonLoad.ysize;
            infoUI.category = arrayJsonLoad.category;
            infoUI.description = arrayJsonLoad.description;
        }
    }

    void LoadObject(int id, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        StartCoroutine(WaitForDownLoad("http://54.180.108.64:80/v1/products/" + id.ToString(), position, eulerAngle, localScale, room, id));
    }

    // ������ ���� id�� ��û�ؼ� ������ ������ �޾ƿ��� �����ϴ� �Լ�
    IEnumerator WaitForDownLoad(string uri, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room, int id = 0)
    {
        FBXJson fbxJson = new FBXJson();

        //// �׽�Ʈ������ ������ ���� ����
        //fbxJson.furnitName = "Test";
        //fbxJson.price = 100000;
        //fbxJson.location = true;
        //fbxJson.xsize = 1;
        //fbxJson.ysize = 0.75f;
        //fbxJson.zsize = 1;
        //fbxJson.no = 1;
        //fbxJson.category = "Bed";

        // ���� ID�� ��û�ؼ� ������ ������ �޾ƿ�
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

        // �޾ƿ� ���� ������ ����ؼ� ���� ����
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

    // ������ �� id�� ��û�ؼ� ���� ������ Json �������� �޾ƿ��� �Լ�
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

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
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