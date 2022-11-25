using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JH_FollowItem : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Destroy(gameObject);
            }
            www.Dispose();
        }
    }
}
