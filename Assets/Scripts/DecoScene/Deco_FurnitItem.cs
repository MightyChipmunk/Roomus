using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Deco_FurnitItem : MonoBehaviour
{
    //public FBXJson fbxJson;
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    string category = "";
    public string Category { get { return category; } set { category = value; } }

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } set { imgBytes = value; } }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);

        // 받아온 이미지의 바이너리 데이터로 자신의 이미지 변경
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgBytes);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnClicked()
    {
        //Deco_PutObject.Instance.fbxJson = fbxJson;
        // 클릭하면 현재 선택한 가구의 ID가 뭔지 전달
        Deco_PutObject.Instance.LoadFBX(ID);
    }
}
