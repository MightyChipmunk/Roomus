using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AI_CropItem : MonoBehaviour
{
    public Texture texture;
    public byte[] texBytes;
    public string category;

    // Start is called before the first frame update
    void Start()
    {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        GetComponent<Image>().sprite = Sprite.Create((Texture2D)texture, rect, new Vector2(0.3f, 0.3f));

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
