using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    CharacterController cc;
    Animator anim;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float rotSpeed = 10f;
    [SerializeField]
    float gravity = -9.81f;
    [SerializeField]
    float jumpPower = 10;

    float yVelocity = 0;

    //도착 위치
    Vector3 receivePos;
    //회전되야 하는 값
    Quaternion receiveRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //데이터 보내기
        if (stream.IsWriting) // isMine == true
        {
            //position, rotation
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        //데이터 받기
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>(); 
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 dir = h * Camera.main.transform.right + v * Camera.main.transform.forward;
            dir.y = 0;
            dir.Normalize();

            if (dir.magnitude > 0.1f)
            {
                photonView.RPC("RPCAnimSetBool", RpcTarget.All, "IsMove", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
            }
            else
                photonView.RPC("RPCAnimSetBool", RpcTarget.All, "IsMove", false);

            cc.Move(speed * dir * Time.deltaTime);
            cc.Move(yVelocity * Vector3.up * Time.deltaTime);

            if (cc.collisionFlags == CollisionFlags.Below)
            {
                photonView.RPC("RPCAnimSetBool", RpcTarget.All, "Falling", false);
                yVelocity = -1;
            }
            else
            {
                photonView.RPC("RPCAnimSetBool", RpcTarget.All, "Falling", true);
                yVelocity += gravity * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && cc.collisionFlags == CollisionFlags.Below)
            {
                photonView.RPC("RPCAnimSetTrigger", RpcTarget.All, "Jump");
                yVelocity = jumpPower;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 15 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 15 * Time.deltaTime);
        }
    }

    [PunRPC]
    void RPCAnimSetBool(string s, bool value)
    {
        if (anim != null)
            anim.SetBool(s, value);
    }

    [PunRPC]
    void RPCAnimSetTrigger(string s)
    {
        anim.SetTrigger(s);
    }
}
