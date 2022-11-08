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
    //public Objects objects;
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
                    initPos -= (arrayJson.xSize / 2 + 3) * Vector3.right;
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
                    initPos += (arrayJson.xSize + 3) * Vector3.right;
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
        Deco_RoomInit.Instance.MakeRoom(arrayJson.xSize, arrayJson.ySize, arrayJson.zSize, arrayJson.door, newRoom.transform);
        //ArrayJson의 데이터를 가지고 오브젝트 생성
        for (int i = 0; i < arrayJson.datas.Count; i++)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.id, info.position, info.eulerAngle, info.localScale, newRoom.transform);
        }

        newRoom.AddComponent<PhotonView>();
        Show_InfoUI infoUI = newRoom.AddComponent<Show_InfoUI>();
        infoUI.x = arrayJson.xSize;
        infoUI.y = arrayJson.ySize;
        infoUI.category = arrayJson.category;
        infoUI.description = arrayJson.description;
    }
    void LoadObject(int idx, Vector3 position, Vector3 eulerAngle, Vector3 localScale, Transform room)
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Contains("txt") && !file.Name.Contains("meta"))
            {
                FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(file.FullName));
                //if (fbxJson.id == idx)
                //{
                //    foreach (FileInfo info in di.GetFiles())
                //    {
                //        if (info.Name.Contains(fbxJson.furnitName) && !info.Name.Contains("meta") && !info.Name.Contains("txt"))
                //        {
                //            byte[] data = File.ReadAllBytes(info.FullName);
                //            string path = Application.dataPath + "/Resources/" + info.Name;
                //            File.WriteAllBytes(path, data);

                //            StartCoroutine(WaitForUpload(info, fbxJson, idx, position, eulerAngle, localScale, room));
                //        }
                //    }
                //}
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