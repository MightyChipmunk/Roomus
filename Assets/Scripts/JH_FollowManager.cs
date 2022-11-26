using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class GetInfo
{
    public int memberNo;
    public string memberName;
    public string memberEmail;
}

public class JH_FollowManager : MonoBehaviour
{
    public GameObject followItem;

    public Transform followerContent;
    public Transform followContent;

    public InputField followInput;
    public InputField unfollowInput;
    // Start is called before the first frame update
    void Start()
    {
        followInput.onSubmit.AddListener(OnSubmit);
        //unfollowInput.onSubmit.AddListener(OnSubmitUn);

        // 잘못쓴거 아님, 네트워크에서 이렇게 줌
        StartCoroutine(OnGetFollow(UrlInfo.url + "/followers"));
        StartCoroutine(OnGetFollower(UrlInfo.url + "/follow"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSubmit(string s)
    {
        StartCoroutine(OnFollow(UrlInfo.url + "/member/" + s));
    }

    void OnSubmitUn(string s)
    {
        StartCoroutine(OnUnFollow(UrlInfo.url + "/member/" + s));
    }

    IEnumerator OnFollow(string url)
    {
        GetInfo getinfo = new GetInfo();
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // 토큰을 헤더에 설정
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

        using (UnityWebRequest www = UnityWebRequest.Post(UrlInfo._url + "Relations/follow", form))
        {
            // 토큰을 헤더에 설정
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                JH_PopUpUI.Instance.SetUI("", www.downloadHandler.text, false);
                AddFollowContent(getinfo.memberNo, getinfo.memberEmail, getinfo.memberName);
                Debug.Log("User Follow complete!");
            }
            www.Dispose();
        }
    }
    IEnumerator OnUnFollow(string url)
    {
        GetInfo getinfo = new GetInfo();
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // 토큰을 헤더에 설정
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
            // 토큰을 헤더에 설정
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
            }
            www.Dispose();
        }
    }

    IEnumerator OnGetFollow(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // 토큰을 헤더에 설정
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                try
                {
                    GetInfo[] getInfos = JsonHelper.FromJson<GetInfo>("{\"data\":" + www.downloadHandler.text + "}");
                    foreach (GetInfo info in getInfos)
                    {
                        AddFollowContent(info.memberNo, info.memberEmail, info.memberName);
                    }
                    Debug.Log("Get FollowList complete!");
                }
                catch (ArgumentException ex)
                {
                    Debug.Log("No Follow");
                }
            }
            www.Dispose();
        }
    }

    IEnumerator OnGetFollower(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // 토큰을 헤더에 설정
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                GetInfo[] getInfos = JsonHelper.FromJson<GetInfo>("{\"data\":" + www.downloadHandler.text + "}");
                foreach (GetInfo info in getInfos)
                {
                    AddFollowerContent(info.memberNo, info.memberEmail, info.memberName);
                }
                Debug.Log("Get FollowerList complete!");
            }
            www.Dispose();
        }
    }

    void AddFollowContent(int no, string email, string nickName)
    {
        GameObject obj = Instantiate(followItem, followContent);
        JH_FollowItem item = obj.GetComponent<JH_FollowItem>();
        item.memberNo = no;
        item.Email = email;
        item.NickName = nickName;
    }

    void AddFollowerContent(int no, string email, string nickName)
    {
        GameObject obj = Instantiate(followItem, followerContent);
        JH_FollowItem item = obj.GetComponent<JH_FollowItem>();
        item.memberNo = no;
        item.Email = email;
        item.NickName = nickName;
        item.Btn.gameObject.SetActive(false);
    }
}
