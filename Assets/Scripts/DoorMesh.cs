using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMesh : MonoBehaviour
{

    public Material material;

    Vector3[] verts =
    {
         new Vector3(-0.5f, 0.5f, 0.0f), new Vector3( 0.5f, 0.5f, 0.0f),
         new Vector3( 0.5f,-0.5f, 0.0f), new Vector3(-0.5f,-0.5f, 0.0f),
         new Vector3(-0.2f, 0.3f, 0.0f), new Vector3( 0.2f, 0.3f, 0.0f),
         new Vector3( 0.2f,-0.5f, 0.0f), new Vector3(-0.2f,-0.5f, 0.0f),
         new Vector3(-0.2f, 0.3f, 0.0f), new Vector3( 0.2f, 0.3f, 0.0f),
         new Vector3( 0.2f,-0.5f, 0.0f), new Vector3(-0.2f,-0.5f, 0.0f),
         new Vector3(-0.2f, 0.3f, 0.1f), new Vector3( 0.2f, 0.3f, 0.1f),
         new Vector3( 0.2f,-0.5f, 0.1f), new Vector3(-0.2f,-0.5f, 0.1f),
         new Vector3(-0.2f, 0.3f, 0.1f), new Vector3( 0.2f, 0.3f, 0.1f),
         new Vector3( 0.2f,-0.5f, 0.1f), new Vector3(-0.2f,-0.5f, 0.1f)
     };

    //Vector3[] verts = new Vector3[20];

    int[] tris = 
    {
         0,1,4, 1,5,4, 1,2,5, 2,6,5,
         3,0,7, 0,4,7, // <- two triangles less in the door mesh
         8,9,12, 9,13,12, 9,10,13, 10,14,13,
         10,11,14, 11,15,14, 11,8,15, 8,12,15
         //,16,17,19, 17,18,19 // <- these triangles close the door 
     };

    void Start()
    {

    }

    public void MakeHole(float left, float right, float up)
    {
        verts[4] = left * Vector3.right + up * Vector3.up;
        verts[5] = right * Vector3.right + up * Vector3.up;
        verts[6] = right * Vector3.right + 0.5f * Vector3.down;
        verts[7] = left * Vector3.right + 0.5f * Vector3.down;
        verts[8] = left * Vector3.right + up * Vector3.up;
        verts[9] = right * Vector3.right + up * Vector3.up;
        verts[10] = right * Vector3.right + 0.5f * Vector3.down;
        verts[11] = left * Vector3.right + 0.5f * Vector3.down;
        verts[12] = left * Vector3.right + up * Vector3.up + 0.1f * Vector3.forward;
        verts[13] = right * Vector3.right + up * Vector3.up + 0.1f * Vector3.forward;
        verts[14] = right * Vector3.right + 0.5f * Vector3.down + 0.1f * Vector3.forward;
        verts[15] = left * Vector3.right + 0.5f * Vector3.down + 0.1f * Vector3.forward;
        verts[16] = left * Vector3.right + up * Vector3.up + 0.1f * Vector3.forward; ;
        verts[17] = right * Vector3.right + up * Vector3.up + 0.1f * Vector3.forward;
        verts[18] = right * Vector3.right + 0.5f * Vector3.down + 0.1f * Vector3.forward;
        verts[19] = left * Vector3.right + 0.5f * Vector3.down + 0.1f * Vector3.forward;

        MeshFilter mF = gameObject.GetComponent<MeshFilter>(); // as MeshFilter;
        Mesh msh = new Mesh();
        msh.vertices = verts;
        msh.triangles = tris;
        msh.RecalculateNormals();
        mF.mesh = msh;

        Destroy(gameObject.GetComponent<Collider>());
        gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {

    }
}