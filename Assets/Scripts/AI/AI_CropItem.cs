using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class SimFurnit
{
    public int id;
    public int distance;
}

public class AI_CropItem : MonoBehaviour
{
    public Texture2D texture;
    public byte[] texBytes;
    public string category;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        texBytes = texture.EncodeToJPG();
        text.text = category;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        StartCoroutine(OnClickPost(UrlInfo.faissUrl + "find_sim_image"));
    }

    IEnumerator OnClickPost(string url)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", texBytes, "Img", "application/jpg");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                SimFurnit[] sims = JsonHelper.FromJsons<SimFurnit>(www.downloadHandler.text);
                List<int> list = new List<int>();
                foreach (SimFurnit simFurnit in sims)
                {
                    list.Add(simFurnit.id);
                }
                Deco_UIManager.Instance.OnSimFurnit(list);
                Debug.Log("Form upload complete!");
            }
        }
    }
}
