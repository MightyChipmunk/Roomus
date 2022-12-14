using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JM_MyPageManager : MonoBehaviour
{
    public Transform pwUIPos;
    public Transform pwUIOriginPos;

    public Text editPWTxt;
    public Text donePWTxt;
    public Text donePWOriginTxt;

    string originPW;
    string newPW;
    string againPW;

    // password submit ui related
    public GameObject pwEnterUI;
    bool isPWEnterMove;
    bool isPWEnter;
    public InputField enterPWInput;
    string pwInput;
    public GameObject enterPWCaution;
    bool isCorrectPW;
    bool isEnterPWShow;

    // password change ui related
    public GameObject pwUI;
    bool isPWMove;   
    bool isPWEdit;
    public InputField newPWInput;
    public InputField reEnterPWInput;
    public Button pwDoneBtn;
    public GameObject reCheckPWCaution;
    public GameObject reEnterPWCaution;

    // password change completed ui related
    public GameObject pwDoneUI;
    bool isPWDoneMove;
    bool isPWGood;

    public Text ID;
    public Text Name;
    public Text Email;
    public Text Status;

    void Start()
    {
        isEnterPWShow = false;
        // Network Connection and bring password info in the future
        GetPW();
        pwEnterUI.transform.position = pwUIOriginPos.position;
        pwUI.transform.position = pwUIOriginPos.position;
        pwDoneUI.transform.position = pwUIOriginPos.position;

        enterPWInput.onSubmit.AddListener(CheckPW);
        enterPWInput.onEndEdit.AddListener(UpdateEnterPW);
        newPWInput.onEndEdit.AddListener(UpdatePW);
        reEnterPWInput.onEndEdit.AddListener(CheckNewPW);

        enterPWCaution.SetActive(false);
        reCheckPWCaution.SetActive(false);
        reEnterPWCaution.SetActive(false);

        pwDoneBtn.interactable = false;

        ID.text = TokenManager.Instance.ID;
        Name.text = TokenManager.Instance.MyInfo.userName;
        Email.text = TokenManager.Instance.MyInfo.memberEmail;
        Status.text = TokenManager.Instance.MyInfo.userRole;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPWEnterMove)
        {
            if (isPWEnter)
            {
                pwEnterUI.transform.position = Vector3.Lerp(pwEnterUI.transform.position, pwUIPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwEnterUI.transform.position, pwUIPos.position) < 0.5f)
                {
                    pwEnterUI.transform.position = pwUIPos.position;
                    isPWEnterMove = false;
                }
            }
            if (!isPWEnter)
            {
                RefreshPWEnter();
                pwEnterUI.transform.position = Vector3.Lerp(pwEnterUI.transform.position, pwUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwEnterUI.transform.position, pwUIOriginPos.position) < 0.5f)
                {
                    pwEnterUI.transform.position = pwUIOriginPos.position;
                    isPWEnterMove = false;
                }
            }
        }

        if (isPWMove)
        {
            if (isPWEdit)
            {
                pwUI.transform.position = Vector3.Lerp(pwUI.transform.position, pwUIPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwUI.transform.position, pwUIPos.position) < 0.5f)
                {
                    pwUI.transform.position = pwUIPos.position;
                    isPWMove = false;
                }
            }
            if (!isPWEdit)
            {
                RefreshPW();
                pwUI.transform.position = Vector3.Lerp(pwUI.transform.position, pwUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwUI.transform.position, pwUIOriginPos.position) < 0.5f)
                {
                    pwUI.transform.position = pwUIOriginPos.position;
                    isPWMove = false;
                }
            }
        }
        if (isPWDoneMove)
        {
            if (isPWGood)
            {
                pwDoneUI.transform.position = Vector3.Lerp(pwDoneUI.transform.position, pwUIPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwDoneUI.transform.position, pwUIPos.position) < 0.5f)
                {
                    pwDoneUI.transform.position = pwUIPos.position;
                    isPWDoneMove = false;
                }
            }
            if (!isPWGood)
            {
                pwDoneUI.transform.position = Vector3.Lerp(pwDoneUI.transform.position, pwUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwDoneUI.transform.position, pwUIOriginPos.position) < 0.5f)
                {
                    pwDoneUI.transform.position = pwUIOriginPos.position;
                    isPWDoneMove = false;
                }
            }
            
        }
    }

    // network connection needed
    public void GetPW()
    {
        originPW = donePWOriginTxt.GetComponent<Text>().text;

    }

    IEnumerator OnGetPW(string url, string pw)
    {
        // ???????? ????
        WWWForm form = new WWWForm();
        // ?????????? string?? pw ????
        form.AddField("confirmPass", pw);
        
        bool confirm = false;

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ?????? ?????? ????
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                confirm = Convert.ToBoolean(www.downloadHandler.text);
                Debug.Log("Confirm Password complete!");
            }
            www.Dispose();
        }

        if (confirm)
        {
            isPWMove = true;
            isPWEdit = true;
            editPWTxt.text = pw;
            enterPWCaution.SetActive(false);
            originPW = pw;
        }
        else enterPWCaution.SetActive(true);
    }

    public void OnClickEditPW()
    {
        isPWEnterMove = true;
        isPWEnter = true;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnClickDoneEnterPW()
    {
        print(pwInput);
        CheckPW(pwInput);
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void UpdateEnterPW(string input)
    {
        pwInput = input;
    }

    public void OnClickShowPW()
    {
        if (enterPWInput.GetComponent<InputField>().contentType == InputField.ContentType.Standard)
        {
            enterPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Password;
        }
        else enterPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnClickShowEnterPW()
    {
        if (newPWInput.GetComponent<InputField>().contentType == InputField.ContentType.Standard)
        {
            newPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Password;
        }
        else newPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnClickShowRePW()
    {
        if (reEnterPWInput.GetComponent<InputField>().contentType == InputField.ContentType.Standard)
        {
            reEnterPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Password;
        }
        else reEnterPWInput.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void CheckPW(string input)
    {
        StartCoroutine(OnGetPW(UrlInfo._url + "member/passcheck", input));
    }
    
    public void UpdatePW(string input)
    {
        newPW = input;
        if (editPWTxt.GetComponent<Text>().text == newPW)
        {
            reCheckPWCaution.SetActive(true);
        }
        else reCheckPWCaution.SetActive(false);
    }

    public void CheckNewPW(string input)
    {
        againPW = input;
        if (newPW == againPW)
        {
            pwDoneBtn.interactable = true;
            reEnterPWCaution.SetActive(false);
        }
        else reEnterPWCaution.SetActive(true);        
    }

    IEnumerator OnNewPassword(string url, string newPW)
    {
        // ???????? ????
        WWWForm form = new WWWForm();
        // ?????????? string?? pw ????
        form.AddField("changePass", newPW);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ?????? ?????? ????
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                UpdatePWInfo(newPW);
                isPWDoneMove = true;
                isPWGood = true;
                Debug.Log("Change Password complete!");
            }
            www.Dispose();
        }
    }


    public void OnClickDonePW()
    {
        if (!reEnterPWCaution.activeSelf)
            StartCoroutine(OnNewPassword(UrlInfo._url + "member/passChange", newPW));
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnClickBackPW()
    {
        isPWEnterMove = true;
        isPWEnter = false;
        
        isPWMove = true;
        isPWEdit = false;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnClickBackPWEnter()
    {
        isPWEnterMove = true;
        isPWEnter = false;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    void RefreshPWEnter()
    {
        enterPWInput.Select();
        enterPWInput.text = "";
        enterPWCaution.SetActive(false);
    }

    public void OnClickClosePW()
    {
        isPWMove = true;
        isPWEdit = false;
        isPWDoneMove = true;
        isPWGood = false;
        isPWEnterMove = true;
        isPWEnter = false;
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    void UpdatePWInfo(string pw)
    {
        editPWTxt.GetComponent<Text>().text = pw;
        donePWTxt.GetComponent<Text>().text = pw;
        donePWOriginTxt.GetComponent<Text>().text = originPW;
    }

    void RefreshPW()
    {
        newPWInput.Select();
        reEnterPWInput.Select();
        newPWInput.text = "";
        reEnterPWInput.text = "";
    }

    public void OnClickChangeStatus()
    {
        StartCoroutine(OnChangeStatus(UrlInfo._url + "member/rankUp"));
    }

    IEnumerator OnChangeStatus(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("empty", "empty");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ?????? ?????? ????
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Status.text = "SELLER";
                Debug.Log("Change Status complete!");
            }
            www.Dispose();
        }
    }
}
