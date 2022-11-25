using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeleteItem : MonoBehaviour
{
    //public FBXJson fbxJson;
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

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
        StartCoroutine(Delete(UrlInfo.url + "/products/" + id.ToString()));
    }

    IEnumerator Delete(string url)
    {

        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Delete complete!");
            }
        }

    }
}
