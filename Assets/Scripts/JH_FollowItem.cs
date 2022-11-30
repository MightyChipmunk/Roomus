using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JH_FollowItem : MonoBehaviour
{
    public int memberNo;
    public string Email;
    public string NickName;

    public Text id;
    public Text nickName;
    public Button Btn;

    // Start is called before the first frame update
    void Start()
    {
        id.text = Email;
        nickName.text = NickName;

        Btn.onClick.AddListener(UnFollow);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (JM_ScreenManager.instance.isSceneChange)
        {
            SceneManager.LoadScene("ShowRoom_New");
        }
    }

    void UnFollow()
    {
        StartCoroutine(OnUnFollow(UrlInfo.url + "/member/" + NickName));
    }

    IEnumerator OnUnFollow(string url)
    {
        GetInfo getinfo = new GetInfo();
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // ��ū�� ����� ����
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string data = www.downloadHandler.text.Substring(1, www.downloadHandler.text.Length - 2);
                try
                {
                    getinfo = JsonUtility.FromJson<GetInfo>(data);
                }
                catch (ArgumentException e)
                {
                    JH_PopUpUI.Instance.SetUI("Warning!", "User Not Exists!", false);
                    yield break;
                }

                Debug.Log("Get Info complete!");
            }
        }

        WWWForm form = new WWWForm();
        form.AddField("userNo", getinfo.memberNo.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(UrlInfo._url + "Relations/unfollow", form))
        {
            // ��ū�� ����� ����
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                JH_PopUpUI.Instance.SetUI("", www.downloadHandler.text, false);
                Debug.Log("User UnFollow complete!");
                Destroy(gameObject);
            }
            www.Dispose();
        }
    }

    void OnClick()
    {
        TokenManager.Instance.roomTypeP = RoomType.User;
        TokenManager.Instance.MemberNo = memberNo;

        JM_ScreenManager.instance.Darken();
        //SceneManager.LoadScene("ShowRoom_New");
    }
}
