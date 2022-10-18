using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMesh : MonoBehaviour
{
    public float left;
    public float right;
    public float up;
    public float down;

    Vector3[] verts = new Vector3[16];

    int[] tris = {
         0,1,4, 1,5,4, 1,2,5, 2,6,5,
         2,3,6, 3,7,6, 3,0,7, 0,4,7,
         8,9,12, 9,13,12, 9,10,13, 10,14,13,
         10,11,14, 11,15,14, 11,8,15, 8,12,15
     };

    void Start()
    {
        Destroy(gameObject.GetComponent<MeshCollider>());

        verts[0] = - 0.5f * transform.right + 0.5f * transform.up;
        verts[1] = + 0.5f * transform.right + 0.5f * transform.up;
        verts[2] = + 0.5f * transform.right - 0.5f * transform.up;
        verts[3] = - 0.5f * transform.right - 0.5f * transform.up;
        verts[4] = left * transform.right + up * transform.up;
        verts[5] = right * transform.right + up * transform.up;
        verts[6] = right * transform.right + down * transform.up;
        verts[7] = left * transform.right + down * transform.up;

        verts[8] = + 0.5f * transform.right + 0.5f * transform.up;
        verts[9] = - 0.5f * transform.right + 0.5f * transform.up;
        verts[10] = - 0.5f * transform.right - 0.5f * transform.up;
        verts[11] = + 0.5f * transform.right - 0.5f * transform.up;
        verts[12] = right * transform.right + up * transform.up;
        verts[13] = left * transform.right + up * transform.up;
        verts[14] = left * transform.right + down * transform.up;
        verts[15] = right * transform.right + down * transform.up;

        MeshFilter mF = gameObject.GetComponent<MeshFilter>(); // as MeshFilter;
        Mesh msh = new Mesh();
        msh.vertices = verts;
        msh.triangles = tris;
        msh.RecalculateNormals();
        mF.mesh = msh;
        
        gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {

    }
}