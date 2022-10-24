using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    [SerializeField]
    float rotSpeed = 100f;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(PhotonNetwork.NickName);
        if (!photonView.IsMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    float mx;
    float my;
    // Update is called once per frame
    void LateUpdate()
    {
        // 마우스 위치에 따른 각도 조절
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        mx += mh * rotSpeed * Time.deltaTime;
        my += mv * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);

        transform.position = player.transform.position; 
    }
}
