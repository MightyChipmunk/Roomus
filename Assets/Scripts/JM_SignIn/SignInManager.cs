using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[Serializable]
public class MyInfo
{
    public string userName;
    public int userNo;
    public string userRole;
    public string memberEmail;
}


public class SignInManager : MonoBehaviour
{
    // ID
    public InputField idInput;
    // PW
    public InputField pwInput;

    string id;
    string pw;

    // need to be used when requesting data / posting data etc. via http server
    // id : Authorization, Value : token
    // loginAPI.SetRequestHeader("Authorization", token);
    string token;
    // Start is called before the first frame update
    void Start()
    {
        idInput.onEndEdit.AddListener(UpdateID);
        pwInput.onEndEdit.AddListener(UpdatePW);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateID(string input)
    {
        id = input;
    }

    void UpdatePW(string input)
    {
        pw = input;
    }

    public void OnSignInClick()
    {
        //SceneManager.LoadScene("Main");

        // login test
        UserLoginInfo loginInfo = new UserLoginInfo();
        loginInfo.memberId = id;
        loginInfo.memberPwd = pw;
        string jsonLoginInfo = JsonUtility.ToJson(loginInfo);
        print(jsonLoginInfo);


        // login network connection
        StartCoroutine(UploadLoginInfo(UrlInfo._url + "login"));
        
        
        //UnityWebRequest loginAPI = UnityWebRequest.Post("http://192.168.0.6:8000/login", jsonLoginInfo);
        //loginAPI.SetRequestHeader("Content-Type", "application/json");
        //loginAPI.SendWebRequest();

    }

    public void OnSignUpClick()
    {
        SceneManager.LoadScene("SignUp");
    }


    IEnumerator UploadLoginInfo(string URL)
    {
        UserLoginInfo loginInfo = new UserLoginInfo();
        loginInfo.memberId = id;
        loginInfo.memberPwd = pw;
        string jsonLoginInfo = JsonUtility.ToJson(loginInfo);

        using (UnityWebRequest loginAPI = UnityWebRequest.Post(URL, jsonLoginInfo))
        {            
            byte[] jsonSend = new System.Text.UTF8Encoding().GetBytes(jsonLoginInfo);
            loginAPI.uploadHandler = new UploadHandlerRaw(jsonSend);
            loginAPI.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            loginAPI.SetRequestHeader("Content-Type", "application/json");
           

            yield return loginAPI.SendWebRequest();

            if (loginAPI.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(loginAPI.error);
                JH_PopUpUI.Instance.SetUI("Login Error", "ID or Password Incorrect", false);
            }
            else
            {
                token = loginAPI.GetResponseHeader("Authorization");
                Debug.Log(loginAPI.GetResponseHeader("Authorization"));

                //UserReturnInfo ex = (UserReturnInfo)loginAPI;
                //Debug.Log(ex);
                //UserReturnInfo returnInfo = JsonUtility.FromJson<UserReturnInfo>(loginAPIs)

                Debug.Log(loginAPI.downloadHandler.text);
                TokenManager.Instance.MyInfo = JsonUtility.FromJson<MyInfo>(loginAPI.downloadHandler.text);
                TokenManager.Instance.Token = token;
                TokenManager.Instance.ID = id;

                loginAPI.Dispose();

                SceneManager.LoadScene("Main");
            }
        }
    }   
}

public class UserLoginInfo
{
    public string memberId;
    public string memberPwd;
}

public class UserReturnInfo
{
    public string userName;
    public string userNo;
    public string userRole;
}
