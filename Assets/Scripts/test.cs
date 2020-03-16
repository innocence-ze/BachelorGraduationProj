using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float length = 1;

    public Vector3 bias;
    public Vector2[] uv;
    public Vector3[] nor;
    private void Start()
    {
        uv = gameObject.GetComponent<MeshFilter>().mesh.uv;
        nor = gameObject.GetComponent<MeshFilter>().mesh.normals;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取网格法线
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (var i = 0; i < normals.Length; i++)
        {
            Vector3 pos = vertices[i];
            pos.x *= transform.localScale.x;
            pos.y *= transform.localScale.y;
            pos.z *= transform.localScale.z;
            pos += transform.position + bias;

            Debug.DrawLine
            (
                pos,
                pos + normals[i] * length, Color.red);
        }
    }

}
