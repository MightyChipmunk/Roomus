using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JM_RoomUIManager : MonoBehaviour
{
    public GameObject moreInfo;
    public Transform infoOriginPos;
    public Transform infoShowPos;
    bool isInfoMove;
    bool isInfoShow;

    public GameObject comment;
    public Transform commentOriginPos;
    public Transform commentShowPos;
    bool isCommentMove;
    bool isCommentShow;

    public GameObject message;
    public Transform messageOriginPos;
    public Transform messageShowPos;
    bool isMessageMove;
    bool isMessageShow;

    public GameObject chat;
    public Transform chatOriginPos;
    public Transform chatShowPos;
    bool isChatMove;
    bool isChatShow;

    public Button likeBtn;
    bool isLike;
    ColorBlock colors;
    [SerializeField]
    Color likeColor; 

    void Start()
    {
        likeColor = new Color(241f/255f, 129f/255f, 113f/255f, 1);
        colors = likeBtn.transform.GetComponent<Button>().colors;

        moreInfo.SetActive(false);
        comment.SetActive(false);
        message.SetActive(false);
        isInfoShow = false;
        isCommentShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInfoMove)
        {
            //print(Vector3.Distance(moreInfo.transform.position, infoShowPos.position));
            if (isInfoShow)
            {
                moreInfo.SetActive(true);
                moreInfo.transform.position = Vector3.Lerp(moreInfo.transform.position, infoShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(infoShowPos.position, moreInfo.transform.position) <= 0.5f)
                {
                    moreInfo.transform.position = infoShowPos.position;
                    isInfoMove = false;
                }
            }
            if (!isInfoShow)
            {
                moreInfo.transform.position = Vector3.Lerp(moreInfo.transform.position, infoOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(moreInfo.transform.position, infoOriginPos.position) <= 0.5f)
                {
                    moreInfo.transform.position = infoOriginPos.position;
                    isInfoMove = false;
                    moreInfo.SetActive(false);
                    JM_FurnInfoManager.instance.Reset();
                }
            }
        }

        if (isCommentMove)
        {
            if (isCommentShow)
            {
                comment.SetActive(true);
                comment.transform.position = Vector3.Lerp(comment.transform.position, commentShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(comment.transform.position, commentShowPos.position) <= 0.5f)
                {
                    comment.transform.position = commentShowPos.position;
                    isCommentMove = false;
                }
            }
            if (!isCommentShow)
            {
                comment.transform.position = Vector3.Lerp(comment.transform.position, commentOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(comment.transform.position, commentOriginPos.position) <= 0.5f)
                {
                    comment.transform.position = commentOriginPos.position;
                    isCommentMove = false;
                    comment.SetActive(false);
                }
            }
        }

        if (isMessageMove)
        {
            if (isMessageShow)
            {
                message.SetActive(true);
                message.transform.position = Vector3.Lerp(message.transform.position, messageShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(message.transform.position, messageShowPos.position) <= 0.5f)
                {
                    message.transform.position = messageShowPos.position;
                    isMessageMove = false;
                }
            }
            if (!isMessageShow)
            {
                message.transform.position = Vector3.Lerp(message.transform.position, messageOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(message.transform.position, messageOriginPos.position) <= 0.5f)
                {
                    message.transform.position = messageOriginPos.position;
                    isMessageMove = false;
                    message.SetActive(false);
                }
            }
        }

        if (isLike)
        {
            colors.selectedColor = likeColor;
            likeBtn.transform.GetComponent<Button>().colors = colors;
        }
        if (!isLike)
        {
            colors.selectedColor = new Color(1, 1, 1, 1);
            likeBtn.transform.GetComponent<Button>().colors = colors;
        }
    }

   

    public void OnClickLibraryBtn()
    {
        isInfoMove = true;
        if (moreInfo.transform.position == infoOriginPos.position)
        {
            isInfoShow = true;
        }
        if (moreInfo.transform.position == infoShowPos.position)
        {
            isInfoShow = false;
        }
    }

    public void OnClickComment()
    {
        isCommentMove = true;
        if (comment.transform.position == commentOriginPos.position)
        {
            isCommentShow = true;
        }
        if (comment.transform.position == commentShowPos.position)
        {
            isCommentShow = false;
        }
    }
   
    public void OnClickMessage()
    {
        isMessageMove = true;
        if (message.transform.position == messageOriginPos.position)
        {
            isMessageShow = true;
        }
        if (message.transform.position == messageShowPos.position)
        {
            isMessageShow = false;
        }
    }

    public void OnClickChat()
    {
        isChatMove = true;
    }

    public void OnClickLike()
    {
        if (colors.selectedColor == likeColor)
        {
            isLike = false;
        }
        else isLike = true;
        

        StartCoroutine(Like(UrlInfo.url + "/rooms/" + Show_Json.Instance.ID.ToString() + "/likes", Show_Json.Instance.ID));
    }

    IEnumerator Like(string url, int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("roomNo", id.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(UnLike(url, id));
                Debug.Log(id.ToString() + www.error);
            }
            else
            {
                JH_PopUpUI.Instance.SetUI("", "Like Complete!", true, 0.3f);
                Debug.Log("Room Like complete!");
            }
        }
    }

    IEnumerator UnLike(string url, int id)
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
                JH_PopUpUI.Instance.SetUI("", "UnLike Complete!", true, 0.3f);
                Debug.Log("Room UnLike complete!");
            }
        }
    }
}
