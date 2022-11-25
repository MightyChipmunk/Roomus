using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SignUpManager : MonoBehaviour
{

    // sign up tab
    public GameObject signUp;
    public GameObject signUpCompl;
    public GameObject avatarCustom;
    public GameObject mannequinCustom;
    public GameObject done;

    public InputField fNameInput;
    public InputField lNameInput;
    public InputField emailInput;
    public InputField idInput;
    public InputField pwInput;

    string fName;
    string lName;
    string email;
    string id;
    string pw;

    //public UserInfoList userInfoList;

    public AllUserInfo alluserInfo;

    public GameObject screenManager;
    JM_ScreenManager screenCode;


    // Start is called before the first frame update
    void Start()
    {
        alluserInfo = new AllUserInfo();
        //alluserInfo.datas = new List<string>();       

        fNameInput.onEndEdit.AddListener(UpdateFName);
        lNameInput.onEndEdit.AddListener(UpdateLName);
        emailInput.onEndEdit.AddListener(UpdateEmail);
        idInput.onEndEdit.AddListener(UpdateID);
        pwInput.onEndEdit.AddListener(UpdatePW);

        screenCode = screenManager.GetComponent<JM_ScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateFName(string input)
    {
        fName = input;
    }

    void UpdateLName(string input)
    {
        lName = input;
    }

    void UpdateEmail(string input)
    {
        email = input;
    }

    void UpdateID(string input)
    {
        id = input;
    }

    void UpdatePW(string input)
    {
        pw = input;
    }

    public void OnRegisterClick()
    {
        AllUserInfo allInfo;
        allInfo = new AllUserInfo();
        allInfo.datas = new List<UserInfo>();

        UserInfo myInfo = new UserInfo();
        //myInfo.fName = fName;
        //myInfo.lName = lName;
        //myInfo.email = email;
        //myInfo.id = id;
        //myInfo.pw = pw;
        myInfo.memberName = fName + " " + lName;
        myInfo.memberEmail = email;
        myInfo.pwd = pw;
        myInfo.memberId = id;

        string jsonRegisterInfo = JsonUtility.ToJson(myInfo, true);

        File.WriteAllText(Application.dataPath + "/asdf.txt", jsonRegisterInfo);

        StartCoroutine(OnSignUp(UrlInfo._url + "guest/signup", jsonRegisterInfo));

        // file path{
        //string path = Application.dataPath + "/" + "UserInfoFile" + ".txt";
        //string jsonMyInfo = JsonUtility.ToJson(myInfo);
        //print(jsonMyInfo);

        //allInfo.datas.Add(myInfo);
        
        //string jsonInfo = JsonUtility.ToJson(allInfo, true);
        //print(jsonInfo);

        //File.WriteAllText(path, jsonInfo);

        // bring file content if file already exists
        //if (File.Exists(path))
        //{
            // bring up file content
            //string originalUserInfo = File.ReadAllText(path);
            //userInfoList = JsonUtility.FromJson<UserInfoList>(originalUserInfo);
        //}        

        // test before network connection
        /*
        string jsonInfo = JsonUtility.ToJson(myInfo);

        userInfoList.datas.Add(jsonInfo);

        File.WriteAllText(path, jsonInfo);
        */

        signUp.SetActive(false);
        signUpCompl.SetActive(true);

        // screen dark --> bright
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
        screenCode.screen.SetActive(true);
    }

    IEnumerator OnSignUp(string url, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                JH_PopUpUI.Instance.SetUI("", "Sign Up Complete!", false);
                Debug.Log("SignUp complete!");
            }
            www.Dispose();
        }
    }

    public void OnSignInNextClick()
    {
        signUpCompl.SetActive(false);
        avatarCustom.SetActive(true);

        // screen dark --> bright
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
        screenCode.screen.SetActive(true);
    }

    public void OnAvatarSkipClick()
    {
        avatarCustom.SetActive(false);
        mannequinCustom.SetActive(true);

        // screen dark --> bright
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
        screenCode.screen.SetActive(true);
    }

    public void OnMannequinSkipClick()
    {
        mannequinCustom.SetActive(false);
        done.SetActive(true);

        // screen dark --> bright
        screenCode.isDark = true;
        screenCode.isStart = true;
        screenCode.alpha = 1;
        screenCode.screen.SetActive(true);
    }

    public void OnDoneSkipClick()
    {
        SceneManager.LoadScene("Main");
        //done.SetActive(false);
    }
}

public class UserInfo
{
    //public string fName;
    //public string lName;
    //public string email;
    //public string id;
    //public string pw;   

    public string memberId;
    public string pwd;
    public string memberName;
    public string memberEmail;
}

public class AllUserInfo
{
    public List<UserInfo> datas;
}
