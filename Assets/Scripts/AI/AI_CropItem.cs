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
        StartCoroutine(OnClickPost(""));
    }

    IEnumerator OnClickPost(string url)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("cropImage", texBytes);

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
