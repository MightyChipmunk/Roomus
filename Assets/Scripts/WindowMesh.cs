using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMesh : MonoBehaviour
{
    Vector3[] verts = new Vector3[16];

    int[] tris = {
         0,1,4, 1,5,4, 1,2,5, 2,6,5,
         2,3,6, 3,7,6, 3,0,7, 0,4,7,
         8,9,12, 9,13,12, 9,10,13, 10,14,13,
         10,11,14, 11,15,14, 11,8,15, 8,12,15
     };
    void Start()
    {
        //MakeHole(-0.3f, -0.1f, 0.1f, -0.5f);
    }

    void Update()
    {

    }

    public void MakeHole(float left, float right, float up, float down)
    {
        Destroy(gameObject.GetComponent<MeshCollider>());

        verts[0] = -0.5f * Vector3.right + 0.5f * Vector3.up;
        verts[1] = +0.5f * Vector3.right + 0.5f * Vector3.up;
        verts[2] = +0.5f * Vector3.right - 0.5f * Vector3.up;
        verts[3] = -0.5f * Vector3.right - 0.5f * Vector3.up;
        verts[4] = left * Vector3.right + up * Vector3.up;
        verts[5] = right * Vector3.right + up * Vector3.up;
        verts[6] = right * Vector3.right + down * Vector3.up;
        verts[7] = left * Vector3.right + down * Vector3.up;

        verts[8] = +0.5f * Vector3.right + 0.5f * Vector3.up;
        verts[9] = -0.5f * Vector3.right + 0.5f * Vector3.up;
        verts[10] = -0.5f * Vector3.right - 0.5f * Vector3.up;
        verts[11] = +0.5f * Vector3.right - 0.5f * Vector3.up;
        verts[12] = right * Vector3.right + up * Vector3.up;
        verts[13] = left * Vector3.right + up * Vector3.up;
        verts[14] = left * Vector3.right + down * Vector3.up;
        verts[15] = right * Vector3.right + down * Vector3.up;

        MeshFilter mF = gameObject.GetComponent<MeshFilter>(); // as MeshFilter;
        Mesh msh = new Mesh();
        msh.vertices = verts;
        msh.triangles = tris;
        msh.RecalculateNormals();
        mF.mesh = msh;

        gameObject.AddComponent<MeshCollider>();
    }
}