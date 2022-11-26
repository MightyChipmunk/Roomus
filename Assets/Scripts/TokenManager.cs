using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType
{
    All,
    Liked,
    User,
}

public class TokenManager : MonoBehaviour
{
    string token;
    public string Token { get { return token; } set { token = value; } }

    string id;
    public string ID { get { return id; } set { id = value; } }

    RoomType roomType = RoomType.All;
    public RoomType roomTypeP { get { return roomType; } set { roomType = value; } }

    int memberNo = 0;
    public int MemberNo { get { return memberNo; } set { memberNo = value; } }

    MyInfo myInfo;
    public MyInfo MyInfo { get { return myInfo; } set { myInfo = value; } }

    public static TokenManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
