using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class CommentInfo
{
    public int commentNo;
    public string memberId;
    public string comment;
}

public class JH_CommentManager : MonoBehaviour
{
    public InputField commentInput;
    public GameObject commentContent;
    public GameObject commentSubmitUI;

    // Start is called before the first frame update
    void Start()
    {
        commentInput.onSubmit.AddListener(SubmitComment);

        StartCoroutine(OnGetComment(UrlInfo.url + "/rooms/" + Show_Json.Instance.ID.ToString() + "/comments"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SubmitComment(string input)
    {
        StartCoroutine(OnPostComment(UrlInfo.url + "/rooms/" + Show_Json.Instance.ID.ToString() + "/comments", Show_Json.Instance.ID, input));
        Refresh();
    }

    void Refresh()
    {
        commentInput.Select();
        commentInput.text = "";
    }

    IEnumerator OnGetComment(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                CommentInfo[] infos = JsonHelper.FromJson<CommentInfo>(www.downloadHandler.text);

                foreach (CommentInfo info in infos)
                {
                    AddContent(info.commentNo, info.memberId, info.comment);
                }

                Debug.Log("Comment Get complete!");
            }
        }
    }

    IEnumerator OnPostComment(string url, int id, string comment)
    {
        CommentInfo info = new CommentInfo();
        info.comment = comment;
        string jsonData = JsonUtility.ToJson(info);
        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(id.ToString() + www.error);
            }
            else
            {
                AddContent(info.commentNo, TokenManager.Instance.ID, comment);
                Debug.Log("Comment Post complete!");
            }
        }
    }

    void AddContent(int commentId, string id, string text)
    {
        GameObject obj = Instantiate(commentSubmitUI, commentContent.transform);
        JH_Comment com = obj.GetComponent<JH_Comment>();
        com.commentText = text;
        com.ID = id;
        com.commentId = commentId;
        //obj.transform.Find("Text").GetComponent<Text>().text = text;
        //obj.transform.Find("ID").GetComponent<Text>().text = id;
    }
}
