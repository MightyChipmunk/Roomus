using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class TextData
{
    public string filter_text;
}

public class Show_ChatManager : MonoBehaviourPun
{
    public RectTransform chatContent;
    public GameObject chatItem;
    public InputField chatInput;
    // Start is called before the first frame update
    void Start()
    {
        chatInput.onSubmit.AddListener(OnSubmit);
    }

    void OnSubmit(string s)
    {
        photonView.RPC("AddContent", RpcTarget.All, s, PhotonNetwork.NickName);
        chatInput.text = "";
        chatInput.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //���� Content�� H
    float prevContentH;
    //ScorllView�� RectTransform
    public RectTransform trScrollView;

    [PunRPC]
    void AddContent(string s, string nickName)
    {
        GameObject obj = Instantiate(chatItem, chatContent);
        obj.GetComponent<Text>().text = nickName + ": ";
        StartCoroutine(AutoScrollBottom());

        StartCoroutine(filter("http://34.27.186.152:5000/filter/", s, obj, nickName));

    }
    string filterText;
    IEnumerator filter(string url, string s, GameObject obj, string nickName)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + s))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                TextData textData = JsonUtility.FromJson<TextData>(www.downloadHandler.text);
                filterText = textData.filter_text;
                obj.GetComponent<Text>().text = nickName + ": " + filterText;
                Debug.Log("Text DownLoad complete!");
            }
        }
    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H ���� Content H ���� Ŀ����(��ũ�� ���ɻ���)
        if (chatContent.sizeDelta.y > trScrollView.sizeDelta.y)
        {
            //4. Content�� �ٴڿ� ����־��ٸ�
            if (chatContent.anchoredPosition.y >= prevContentH - trScrollView.sizeDelta.y)
            {
                //5. Content�� y���� �ٽ� ����������
                chatContent.anchoredPosition = new Vector2(0, chatContent.sizeDelta.y - trScrollView.sizeDelta.y);
            }
        }
    }
}
