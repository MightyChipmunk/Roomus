using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    //이전 Content의 H
    float prevContentH;
    //ScorllView의 RectTransform
    public RectTransform trScrollView;

    [PunRPC]
    void AddContent(string s, string nickName)
    {
        GameObject obj = Instantiate(chatItem, chatContent);
        obj.GetComponent<Text>().text = nickName + ": " + s;

        StartCoroutine(AutoScrollBottom());
    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H 보다 Content H 값이 커지면(스크롤 가능상태)
        if (chatContent.sizeDelta.y > trScrollView.sizeDelta.y)
        {
            //4. Content가 바닥에 닿아있었다면
            if (chatContent.anchoredPosition.y >= prevContentH - trScrollView.sizeDelta.y)
            {
                //5. Content의 y값을 다시 설정해주자
                chatContent.anchoredPosition = new Vector2(0, chatContent.sizeDelta.y - trScrollView.sizeDelta.y);
            }
        }
    }
}
