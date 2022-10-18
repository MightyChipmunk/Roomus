using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMesh : MonoBehaviour
{
    Vector3[] verts = new Vector3[8];

    int[] tris = {
         0,1,4, 1,5,4, 1,2,5, 2,6,5,
         2,3,6, 3,7,6, 3,0,7, 0,4,7,
         7,4,0, 7,0,3, 6,7,3, 6,3,2,
         5,6,2, 5,2,1, 4,5,1, 4,1,0,
     };

    void Start()
    {
        Destroy(gameObject.GetComponent<MeshCollider>());

        verts[0] = - 0.5f * transform.right + 0.5f * transform.up;
        verts[1] = + 0.5f * transform.right + 0.5f * transform.up;
        verts[2] = + 0.5f * transform.right - 0.5f * transform.up;
        verts[3] = - 0.5f * transform.right - 0.5f * transform.up;
        verts[4] = - 0.25f * transform.right + 0.25f * transform.up;
        verts[5] = + 0.25f * transform.right + 0.25f * transform.up;
        verts[6] = + 0.25f * transform.right - 0.25f * transform.up;
        verts[7] = - 0.25f * transform.right - 0.25f * transform.up;

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