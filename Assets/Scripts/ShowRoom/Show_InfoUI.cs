using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Show_InfoUI : MonoBehaviourPun
{
    GameObject infoUI;
    GameObject player;
    GameObject canvas;

    public float x;
    public float y;
    public string category;
    public string description;

    bool cap;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        infoUI = canvas.transform.Find("RoomInfo").gameObject;
        player = GameObject.Find(PhotonNetwork.NickName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < Mathf.Sqrt(Mathf.Pow(x + 3, 2) + Mathf.Pow(y + 3, 2))/2)
        {
            canvas.GetComponent<Show_HideUI>().canHide = false;
            cap = true;
            infoUI.transform.Find("RoomName").GetComponent<Text>().text = gameObject.name;
            infoUI.transform.Find("Category").GetComponent<Text>().text = category;
            infoUI.transform.Find("Description").GetComponent<Text>().text = description;
        }
        else if (cap)
        {
            canvas.GetComponent<Show_HideUI>().canHide = true;
            cap = false;
        }
    }
}
