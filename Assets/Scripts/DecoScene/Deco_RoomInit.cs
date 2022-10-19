using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_RoomInit : MonoBehaviour
{
    float xSize;
    float ySize;
    float zSize;
    int balcony;

    Transform room;

    private void Awake()
    {
        xSize = Deco_GetXYZ.Instance.X;
        ySize = Deco_GetXYZ.Instance.Y;
        zSize = Deco_GetXYZ.Instance.Z;
        balcony = Deco_GetXYZ.Instance.Balcony;
    }

    private void Start()
    {
        room = GameObject.Find("Room").transform;
        MakeRoom(xSize, ySize, zSize, balcony);
    }
    
    public void MakeRoom(float x, float y, float z, int bal)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject frontWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject celling = GameObject.CreatePrimitive(PrimitiveType.Quad);
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
                WindowMesh win1 = frontWall.AddComponent<WindowMesh>();
                win1.MakeHole(-0.15f, 0.15f, 0.1f, -0.5f);
                break;
            case 2:
                WindowMesh win2 = backWall.AddComponent<WindowMesh>();
                win2.MakeHole(-0.15f, 0.15f, 0.1f, -0.5f);
                break;
            case 3:
                WindowMesh win3 = rightWall.AddComponent<WindowMesh>();
                win3.MakeHole(-0.15f, 0.15f, 0.1f, -0.5f);
                break;
            case 4:
                WindowMesh win4 = leftWall.AddComponent<WindowMesh>();
                win4.MakeHole(-0.15f, 0.15f, 0.1f, -0.5f);
                break;
        }
    }
}
