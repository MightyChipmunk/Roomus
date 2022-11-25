using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class CommentPutDelete
{
    public int commentNo;
    public string comment;
    public string password;
}

public class JH_Comment : MonoBehaviour
{
    public string commentText;
    public string ID;
    public int commentId;

    Button modBtn;
    Button delBtn;

    InputField pwInput;
    InputField commentInput;

    string pw = "";
    string comment = "";
    
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Text").GetComponent<Text>().text = commentText;
        transform.Find("ID").GetComponent<Text>().text = ID;

        modBtn = transform.Find("ModBtn").GetComponent<Button>();
        modBtn.onClick.AddListener(OnClickMod);
        delBtn = transform.Find("DelBtn").GetComponent<Button>();
        delBtn.onClick.AddListener(OnClickDelete);

        pwInput = transform.Find("PWInput").GetComponent<InputField>();
        pwInput.onSubmit.AddListener(OnSubmit);
        pwInput.gameObject.SetActive(false);

        commentInput = transform.Find("ModInput").GetComponent<InputField>();
        commentInput.onSubmit.AddListener(OnSubmitComment);
        commentInput.gameObject.SetActive(false);
    }

    void OnSubmit(string s)
    {
        pw = s;
        pwInput.gameObject.SetActive(false);
    }

    void OnSubmitComment(string s)
    {
        comment = s;
        commentInput.gameObject.SetActive(false);
    }

    void OnClickMod()
    {
        pwInput.gameObject.SetActive(true);

        CommentPutDelete com = new CommentPutDelete();
        StopAllCoroutines();
        StartCoroutine(OnPutComment(UrlInfo.url + "/rooms/" + Show_Json.Instance.ID.ToString() + "/comments", com));
    }

    void OnClickDelete()
    {
        pwInput.gameObject.SetActive(true);

        CommentPutDelete com = new CommentPutDelete();
        StopAllCoroutines();
        StartCoroutine(OnDeleteComment(UrlInfo.url + "/rooms/" + Show_Json.Instance.ID.ToString() + "/comments", com));
    }

    IEnumerator OnDeleteComment(string url, CommentPutDelete com)
    {
        while (pw.Length <= 0)
        {
            yield return null;
        }

        com.password = pw;
        com.commentNo = commentId;

        string jsonData = JsonUtility.ToJson(com);

        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                JH_PopUpUI.Instance.SetUI("Warning!", "Wrong Password!", false);
                pw = "";
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Comment Delete complete!");
                Destroy(gameObject);
            }
        }
    }

    IEnumerator OnPutComment(string url, CommentPutDelete com)
    {
        while (pw.Length <= 0)
        {
            yield return null;
        }

        commentInput.gameObject.SetActive(true);

        while (comment.Length <= 0)
        {
            yield return null;
        }

        com.password = pw;
        com.comment = comment;
        com.commentNo = commentId;

        string jsonData = JsonUtility.ToJson(com);

        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                JH_PopUpUI.Instance.SetUI("Warning!", "Wrong Password!", false);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Comment Modify complete!");
                transform.Find("Text").GetComponent<Text>().text = comment;
            }
        }
    }
}
