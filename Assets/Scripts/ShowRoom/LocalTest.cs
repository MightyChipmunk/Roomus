using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTest : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        player.name = PhotonNetwork.NickName;
        PhotonNetwork.Instantiate("CamFollow", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
