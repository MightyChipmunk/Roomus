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
}

public class JH_FollowManager : MonoBehaviour
{
    public InputField followInput;
    public InputField unfollowInput;
    // Start is called before the first frame update
    void Start()
    {
        followInput.onSubmit.AddListener(OnSubmit);
        unfollowInput.onSubmit.AddListener(OnSubmitUn);
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
                JH_PopUpUI.Instance.SetUI("", "User Follow complete!", false);
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
                JH_PopUpUI.Instance.SetUI("", "User UnFollow complete!", false);
                Debug.Log("User UnFollow complete!");
            }
            www.Dispose();
        }
    }
}
