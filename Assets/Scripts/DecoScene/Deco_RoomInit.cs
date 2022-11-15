using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_RoomInit : MonoBehaviour
{
    public static Deco_RoomInit Instance;
    //public GameObject balconyFac;

    string roomName;
    float xSize;
    float ySize;
    float zSize;
    int balcony;

    public Material wallMat;
    public Material floorMat;
    public Material cellingMat;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 새 방을 만들었을 때
        if (Deco_GetXYZ.Instance != null)
        {
            roomName = Deco_GetXYZ.Instance.RoomName;
            xSize = Deco_GetXYZ.Instance.X;
            ySize = Deco_GetXYZ.Instance.Y;
            zSize = Deco_GetXYZ.Instance.Z;
            balcony = Deco_GetXYZ.Instance.Balcony;
        }
        // 도면을 불러왔을 때
        else if (JH_RoomDecoManager.Instance != null)
        {
            roomName = "Room" + JH_RoomDecoManager.Instance.SelectedRoom.ToString();
            xSize = -1;
            ySize = -1;
            zSize = -1;
        }
    }

    private void Start()
    {
        // 새 방을 만들었을 때
        if (Deco_GetXYZ.Instance != null)
        {
            MakeRoom(xSize, ySize, zSize, balcony, GameObject.Find("Room").transform);
            Deco_Json.Instance.SaveRoomInfo(roomName, xSize, ySize, zSize, balcony);
            Deco_Json.Instance.FirstPost("roomUrl", roomName, xSize, ySize, zSize, balcony);
            //Deco_Json.Instance.SaveNewFile(roomName);
            Destroy(Deco_GetXYZ.Instance.gameObject);
        }
        // 방을 불러왔을 때
        else if (Deco_LoadRoomList.Instance != null)    
        {
            // 네트워크로 불러오기
            Deco_Json.Instance.LoadFile(Deco_LoadRoomList.Instance.ID);
            // 로컬로 불러오기
            //Deco_Json.Instance.LoadFile(Deco_LoadRoomList.Instance.RoomName);
            Destroy(Deco_LoadRoomList.Instance.gameObject);
        }
        // 도면 방을 생성했을 때
        else if (JH_RoomDecoManager.Instance != null)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Room" + JH_RoomDecoManager.Instance.SelectedRoom.ToString()));
            Deco_Json.Instance.SaveRoomInfo(roomName, -1, -1, -1, JH_RoomDecoManager.Instance.SelectedRoom);
            Deco_Json.Instance.FirstPost("roomUrl", roomName, xSize, ySize, zSize, balcony);
            go.name = "Room";
            Destroy(JH_RoomDecoManager.Instance.gameObject);
        }
    }
    
    public void MakeRoom(float x, float y, float z, int bal, Transform room)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
        floor.GetComponent<Renderer>().material = floorMat;
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        leftWall.GetComponent<Renderer>().material = wallMat;
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        rightWall.GetComponent<Renderer>().material = wallMat;
        GameObject frontWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        frontWall.GetComponent<Renderer>().material = wallMat;
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backWall.GetComponent<Renderer>().material = wallMat;       
        GameObject celling = GameObject.CreatePrimitive(PrimitiveType.Quad);
        celling.GetComponent<Renderer>().material = cellingMat;
        floor.transform.parent = room;
        floor.layer = LayerMask.NameToLayer("Floor");
        floor.name = "Floor";
        leftWall.transform.parent = room.transform.Find("Walls");
        leftWall.layer = LayerMask.NameToLayer("Wall");
        leftWall.name = "Wall";
        rightWall.transform.parent = room.transform.Find("Walls");
        rightWall.layer = LayerMask.NameToLayer("Wall");
        rightWall.name = "Wall";
        frontWall.transform.parent = room.transform.Find("Walls");
        frontWall.layer = LayerMask.NameToLayer("Wall");
        frontWall.name = "Wall";
        backWall.transform.parent = room.transform.Find("Walls");
        backWall.layer = LayerMask.NameToLayer("Wall");
        backWall.name = "Wall";
        celling.transform.parent = room;
        celling.name = "Celling";

        floor.transform.localPosition = Vector3.zero;
        floor.transform.localEulerAngles = new Vector3(90, 0, 0);
        floor.transform.localScale = new Vector3(x, y, 1);

        leftWall.transform.localPosition = new Vector3(x / 2, z / 2, 0);
        leftWall.transform.localEulerAngles = new Vector3(0, 90, 0);
        leftWall.transform.localScale = new Vector3(y, z, 1);
        rightWall.transform.localPosition = new Vector3(-x / 2, z / 2, 0);
        rightWall.transform.localEulerAngles = new Vector3(0, -90, 0);
        rightWall.transform.localScale = new Vector3(y, z, 1);
        frontWall.transform.localPosition = new Vector3(0, z / 2, y / 2);
        frontWall.transform.localEulerAngles = new Vector3(0, 0, 0);
        frontWall.transform.localScale = new Vector3(x, z, 1);
        backWall.transform.localPosition = new Vector3(0, z / 2, -y / 2);
        backWall.transform.localEulerAngles = new Vector3(0, 180, 0);
        backWall.transform.localScale = new Vector3(x, z, 1);

        celling.transform.localPosition = new Vector3(0, z, 0);
        celling.transform.localEulerAngles = new Vector3(-90, 0, 0);
        celling.transform.localScale = new Vector3(x, y, 1);

        switch (bal)
        {
            case 1:
                DoorMesh win1 = frontWall.AddComponent<DoorMesh>();
                win1.MakeHole(-0.15f / frontWall.transform.localScale.x * 3.5f, 0.15f / frontWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 2:
                DoorMesh win2 = backWall.AddComponent<DoorMesh>();
                win2.MakeHole(-0.15f / backWall.transform.localScale.x * 3.5f, 0.15f / backWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 3:
                DoorMesh win3 = rightWall.AddComponent<DoorMesh>();
                win3.MakeHole(-0.15f / rightWall.transform.localScale.x * 3.5f, 0.15f / rightWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 4:
                DoorMesh win4 = leftWall.AddComponent<DoorMesh>();
                win4.MakeHole(-0.15f / leftWall.transform.localScale.x * 3.5f, 0.15f / leftWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
        }
    }
}
