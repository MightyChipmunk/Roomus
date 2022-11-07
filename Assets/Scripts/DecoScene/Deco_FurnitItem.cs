using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Deco_FurnitItem : MonoBehaviour
{
    public FBXJson fbxJson;
    int id = 0;
    public int ID { get { return id; } set { id = value; } }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);

        //get
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            // 추후에 이름이 아닌 ID를 받는 방식으로 대체해야됨
            if (file.Name == fbxJson.furnitName + "ScreenShot.png")
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(data);
                Rect rect = new Rect(0, 0, tex.width, tex.height);
                GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
            }
        }

        //StartCoroutine(OnPostJson("http://192.168.0.243:8000/v1/products", id));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnClicked()
    {
        Deco_PutObject.Instance.fbxJson = fbxJson;
        Deco_PutObject.Instance.LoadFBX(/*id*/);
    }

    IEnumerator OnPostJson(string uri, int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, id.ToString()))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] data = www.downloadHandler.data;
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(data);
                Rect rect = new Rect(0, 0, tex.width, tex.height);
                GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
            }
        }
    }
}
