using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        StartCoroutine(OnClickPost("네트워크 서버"));
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
                Debug.Log("Form upload complete!");
            }
        }
    }
}
