using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PutDeleteTest : MonoBehaviour
{
    [SerializeField]
    string url;

    public void OnPut()
    {
        StartCoroutine(Put(url));
    }

    public void OnDelete()
    {
        StartCoroutine(Delete(url));
    }

    IEnumerator Put(string url)
    {
        FBXJson fbxJson = new FBXJson();
        fbxJson.furnitName = "Test";
        fbxJson.price = 100000;
        fbxJson.category = "Table";

        string jsonData = JsonUtility.ToJson(fbxJson);

        UnityWebRequest www = UnityWebRequest.Put(url, jsonData);

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
