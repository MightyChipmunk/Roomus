using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Deco_FurnitItem : MonoBehaviour
{
    public FBXJson fbxJson;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            //if (file.Name.Contains(fbxJson.furnitName) && !file.Name.Contains("txt"))
            //{
            //    byte[] data = File.ReadAllBytes(file.FullName);
            //    string path = Application.dataPath + "/Resources/" + file.Name;
            //    File.WriteAllBytes(path, data);
            //}

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
    }

    // Update is called once per frame
    void Update()
    {
        //if (Resources.Load<Texture2D>(fbxJson.furnitName + "ScreenShot"))
        //{
        //    Texture2D tex = Resources.Load<Texture2D>(fbxJson.furnitName + "ScreenShot");
        //    Rect rect = new Rect(0, 0, tex.width, tex.height);
        //    GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        //}
    }

    public void OnClicked()
    {
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        //foreach (FileInfo file in di.GetFiles())
        //{
        //    //if (file.Name.Contains(fbxJson.furnitName) && !file.Name.Contains("txt"))
        //    //    StartCoroutine(WaitForUpload(file));

        //    if (file.Name == fbxJson.furnitName + ".fbx")
        //    {

        //    }
        //}

        Deco_PutObject.Instance.fbxJson = fbxJson;
    }
}
