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
            if (file.Name.Contains(fbxJson.furnitName) && !file.Name.Contains("txt"))
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                string path = Application.dataPath + "/Resources/" + file.Name;
                File.WriteAllBytes(path, data);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Resources.Load<Texture2D>(fbxJson.furnitName + "ScreenShot"))
        {
            Texture2D tex = Resources.Load<Texture2D>(fbxJson.furnitName + "ScreenShot");
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        }
    }

    public void OnClicked()
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/LocalServer");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Contains(fbxJson.furnitName) && !file.Name.Contains("txt"))
                StartCoroutine(WaitForUpload(file));
        }

        Deco_PutObject.Instance.fbxJson = fbxJson;
    }

    IEnumerator WaitForUpload(FileInfo file)
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
            Deco_PutObject.Instance.objFactory = Resources.Load<GameObject>(path);
        }
    }
}
