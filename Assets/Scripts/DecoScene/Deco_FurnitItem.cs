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
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnClicked()
    {
        Deco_PutObject.Instance.fbxJson = fbxJson;
        Deco_PutObject.Instance.LoadFBX();
    }
}
