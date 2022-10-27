using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Deco_FurnitItem : MonoBehaviour
{
    //int id;
    //public int Id { get { return id; } set { id = value; } }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClicked()
    {
        //Deco_PutObject.objFactory = obj;

        DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath);
        foreach (FileInfo file in di.GetFiles())
        {
            byte[] data = File.ReadAllBytes(file.FullName);
            string path = Application.dataPath + "/Resources/" + file.Name;
            File.WriteAllBytes(path, data);
        }
        di = new DirectoryInfo(Application.dataPath + "/Resources");
        foreach (FileInfo file in di.GetFiles())
        {
            string type = file.Name.Substring(file.Name.Length - 3, 3);
            if (type == "fbx")
            {
                StartCoroutine(WaitForUpload(file));
            }
        }
    }

    IEnumerator WaitForUpload(FileInfo file)
    {
        string path = file.FullName.Substring(0, file.FullName.Length - 4);
        FBXJson fbxJson = JsonUtility.FromJson<FBXJson>(File.ReadAllText(path + ".txt"));
        path = file.Name.Substring(0, file.Name.Length - 4);
        GameObject parent = new GameObject(path);
        parent.transform.position = Vector3.zero;
        parent.transform.rotation = Quaternion.identity;
        parent.transform.localScale = Vector3.one;

        while (true)
        {
            if (Resources.Load<GameObject>(path))
                break;

            yield return null;
        }

        GameObject go = Instantiate(Resources.Load<GameObject>(path));
        go.transform.parent = parent.transform;
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.center = new Vector3(0, fbxJson.ySize / 2, 0);
        col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        go.transform.localPosition = Vector3.zero;
        Deco_Idx decoIdx = parent.AddComponent<Deco_Idx>();
        decoIdx.Name = fbxJson.furnitName;
        decoIdx.Price = fbxJson.price;
        decoIdx.Category = fbxJson.category;
        decoIdx.Idx = UnityEngine.Random.Range(0, 1000);
    }
}
