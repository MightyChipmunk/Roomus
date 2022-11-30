using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PutDeleteTest : MonoBehaviour
{
    public GameObject prefab;
    public Transform trContent;

    string url = UrlInfo.url + "/products";

    private void Start()
    {
        StartCoroutine(OnGetJson(url));
    }

    public void OnPut()
    {
        //StartCoroutine(Put(url));
    }

    public void OnDelete()
    {
        //StartCoroutine(Delete(url));
    }

    IEnumerator Put(string url)
    {
        FBXJson fbxJson = new FBXJson();
        fbxJson.furnitName = "Test";
        fbxJson.price = 100000;
        fbxJson.category = "Table";

        string jsonData = JsonUtility.ToJson(fbxJson);

        UnityWebRequest www = UnityWebRequest.Put(url, jsonData);
        www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Put complete!");
        }
    }
    
    IEnumerator Delete(string url)
    {
        UnityWebRequest www = UnityWebRequest.Delete(url);
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
                // url ?עק?? json???? ???? ??????
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // ?????? url ?עק?? ????????? ?????? ????????? id?? ???????? ??? ????
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("UrlList Download complete!");
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
                // ?????? ????????? id?? ????‚צ???? ???? ???
                // Deco_FurnitItem
                AddContent(info.furnitName, info.no, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }

    void AddContent(string text, int id = 0, byte[] imgBytes = null)
    {
        GameObject item = Instantiate(prefab, trContent);
        GameObject txt = item.transform.GetChild(0).gameObject;
        txt.GetComponent<Text>().text = text;
        item.name = id.ToString();
        //item.GetComponent<Deco_FurnitItem>().fbxJson = fbxJson;
        item.GetComponent<DeleteItem>().ID = id;
        item.GetComponent<DeleteItem>().ImageBytes = imgBytes;
    }
}
