using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public bool gender;
    public GameObject screenManager;
    JM_ScreenManager screenCode;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        screenCode = screenManager.GetComponent<JM_ScreenManager>();
    }

    private void Update()
    {
        if (screenCode.isSceneChange)
        {
            CreateRoom();
        }
    }

    //�� ����
    public void CreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 0;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        PhotonNetwork.CreateRoom("Room" + Show_LoadRoomList.Instance.ID.ToString(), roomOptions);
    }

    //���� �����Ǹ� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //�� ������ ���� �ɶ� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
        JoinRoom();
    }

    //�� ���� ��û
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room" + Show_LoadRoomList.Instance.ID.ToString());
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");

        if (Show_LoadRoomList.Instance != null /*&& Show_LoadRoomList.Instance.localTest*/)
            PhotonNetwork.LoadLevel("JH_ShowRoom");
        else
            PhotonNetwork.LoadLevel("JH_ShowRoom");
    }

    //�� ������ ���� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    public void OnClickMan()
    {
        gender = true;
        screenCode.Darken();
        //CreateRoom();
    }

    public void OnClickWoman()
    {
        gender = false;
        screenCode.Darken();
        //CreateRoom();
    }
}
