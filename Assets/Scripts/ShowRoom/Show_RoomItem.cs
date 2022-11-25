using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Show_RoomItem : MonoBehaviour
{
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    string category = "";
    public string Category { get { return category; } set { category = value; } }

    [SerializeField]
    bool localTest = false;

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } set { imgBytes = value; } }

    public Text roomName;
    public Text roomDescription;
    public Image roomImage;

    public Button button;
    public Button likeButton;

    public InputField commentInput;
    public Transform commentContent;
    public GameObject commentUI;

    // Start is called before the first frame update
    void Start()
    {
        if (imgBytes != null)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(imgBytes);
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            roomImage.sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        }
        button.onClick.AddListener(OnClicked);
        likeButton.onClick.AddListener(OnClickLike);
        commentInput.onSubmit.AddListener(SubmitComment);

        StartCoroutine(OnGetComment(UrlInfo.url + "/rooms/" + ID.ToString() + "/comments"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubmitComment(string input)
    {
        StartCoroutine(OnPostComment(UrlInfo.url + "/rooms/" + ID.ToString() + "/comments", ID, input));
        Refresh();
    }

    void Refresh()
    {
        commentInput.Select();
        commentInput.text = "";
    }

    public void OnClicked()
    {
        Show_LoadRoomList.Instance.RoomName = gameObject.name;
        Show_LoadRoomList.Instance.ID = ID;
        Show_LoadRoomList.Instance.localTest = localTest;
        SceneManager.LoadScene("Test_Connect");
    }

    public void OnClickLike()
    {
        StartCoroutine(Like(UrlInfo.url + "/rooms/" + ID.ToString() + "/likes", ID));
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
                iTween.ScaleTo(likeButton.gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

                Debug.Log("Furnit UnLike complete!");
            }
        }
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
                    if (!info.delete)
                        AddContent(info.memberId, info.comment);
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
                AddContent(TokenManager.Instance.ID, comment);
                Debug.Log("Comment Post complete!");
            }
        }
    }

    void AddContent(string id, string text)
    {
        GameObject obj = Instantiate(commentUI, commentContent.transform);
        obj.transform.Find("Text").GetComponent<Text>().text = text;
        obj.transform.Find("ID").GetComponent<Text>().text = id;
    }
}
