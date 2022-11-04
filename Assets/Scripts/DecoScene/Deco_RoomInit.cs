using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_RoomInit : MonoBehaviour
{
    public static Deco_RoomInit Instance;
    public GameObject balconyFac;

    string roomName;
    float xSize;
    float ySize;
    float zSize;
    int balcony;

    public Material testMat;

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
    }

    private void Start()
    {
        // 새 방을 만들었을 때
        if (Deco_GetXYZ.Instance != null)
        {
            MakeRoom(xSize, ySize, zSize, balcony, GameObject.Find("Room").transform);
            Deco_Json.Instance.SaveRoomInfo(roomName, xSize, ySize, zSize, balcony);
            Destroy(Deco_GetXYZ.Instance.gameObject);
        }
        // 방을 불러왔을 때
        else if (Deco_LoadRoomList.Instance != null)    
        {
            Deco_Json.Instance.LoadFile(Deco_LoadRoomList.Instance.RoomName);
            Destroy(Deco_LoadRoomList.Instance.gameObject);
        }
    }
    
    public void MakeRoom(float x, float y, float z, int bal, Transform room)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
        floor.GetComponent<Renderer>().material = testMat;
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        leftWall.GetComponent<Renderer>().material = testMat;
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        rightWall.GetComponent<Renderer>().material = testMat;
        GameObject frontWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        frontWall.GetComponent<Renderer>().material = testMat;
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backWall.GetComponent<Renderer>().material = testMat;       
        GameObject celling = GameObject.CreatePrimitive(PrimitiveType.Quad);
        celling.GetComponent<Renderer>().material = testMat;
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
                //GameObject balc1 = Instantiate(balconyFac);
                //balc1.transform.parent = room;
                //balc1.transform.localScale = new Vector3(frontWall.transform.localScale.x, z, 3);
                //balc1.transform.forward = frontWall.transform.forward;
                //balc1.transform.position = frontWall.transform.position;
                win1.MakeHole(-0.15f / frontWall.transform.localScale.x * 3.5f, 0.15f / frontWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 2:
                DoorMesh win2 = backWall.AddComponent<DoorMesh>();
                //GameObject balc2 = Instantiate(balconyFac);
                //balc2.transform.parent = room;
                //balc2.transform.localScale = new Vector3(backWall.transform.localScale.x, z, 3);
                //balc2.transform.forward = backWall.transform.forward;
                //balc2.transform.position = backWall.transform.position;
                win2.MakeHole(-0.15f / backWall.transform.localScale.x * 3.5f, 0.15f / backWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 3:
                DoorMesh win3 = rightWall.AddComponent<DoorMesh>();
                //GameObject balc3 = Instantiate(balconyFac);
                //balc3.transform.parent = room;
                //balc3.transform.localScale = new Vector3(rightWall.transform.localScale.x, z, 3);
                //balc3.transform.forward = rightWall.transform.forward;
                //balc3.transform.position = rightWall.transform.position;
                win3.MakeHole(-0.15f / rightWall.transform.localScale.x * 3.5f, 0.15f / rightWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
            case 4:
                DoorMesh win4 = leftWall.AddComponent<DoorMesh>();
                //GameObject balc4 = Instantiate(balconyFac);
                //balc4.transform.parent = room;
                //balc4.transform.localScale = new Vector3(leftWall.transform.localScale.x, z, 3);
                //balc4.transform.forward = leftWall.transform.forward;
                //balc4.transform.position = leftWall.transform.position;
                win4.MakeHole(-0.15f / leftWall.transform.localScale.x * 3.5f, 0.15f / leftWall.transform.localScale.x * 3.5f, 0.1f / frontWall.transform.localScale.y * 3.5f);
                break;
        }
    }
}
