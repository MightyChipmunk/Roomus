using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JH_MyPageManager : MonoBehaviour
{
    public GameObject likePrefab;
    public Transform content;

    public GameObject likedPrefab;
    public Transform likedContent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnGetJson(UrlInfo.url + "/rooms"));
        StartCoroutine(OnGetLike(UrlInfo.url + "/products/myLikes"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnGetJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // url �迭�� json���� �޾Ƽ� ������
                roomInfos[] data = JsonHelper.FromJson<roomInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    AddContent(data[i].roomName, data[i].cntLikes);
                }
                Debug.Log("Room UrlList Download complete!");
            }
        }
    }

    void AddContent(string roomName, int likes)
    {
        GameObject obj = Instantiate(likePrefab, content);
        obj.GetComponent<LikeItem>().roomName = roomName;
        obj.GetComponent<LikeItem>().likes = likes;
    }

    void AddContent(byte[] imgData, string furnitName)
    {
        GameObject obj = Instantiate(likedPrefab, likedContent);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgData);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        obj.GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        obj.transform.Find("Text").GetComponent<Text>().text = furnitName;
    }

    IEnumerator OnGetLike(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // �������� �����͸� �޾ƿ�
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // �������� �����Ϳ� �ִ� ��ũ���� url�� �ٽ� Get ��û�� ����
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("Furnit UrlList Download complete!");
            }
        }
    }

    IEnumerator OnGetUrl(furnitInfos info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // �޾ƿ� ��ũ������ �̸� ���� ������ �̿��� ���̺귯���� �߰�
                AddContent(www.downloadHandler.data, info.furnitName);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }
}
