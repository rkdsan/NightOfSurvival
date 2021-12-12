using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshTest : MonoBehaviour
{

    Mesh m;

    // Use this for initialization
    void Start()
    {

        if (GetComponent<SkinnedMeshRenderer>())
            m = GetComponent<SkinnedMeshRenderer>().sharedMesh;

        for (int i = 0; i < m.vertices.Length; i++)
        {
            m.vertices[i] = new Vector3(m.vertices[i].x + 100f, m.vertices[i].y, m.vertices[i].z);
        }
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        List<int> triangles = new List<int>();

        List<Vector2> uv1 = new List<Vector2>();
        List<Vector4> tangents = new List<Vector4>();
        List<BoneWeight> boneWeights = new List<BoneWeight>();
        List<Matrix4x4> bindposes = new List<Matrix4x4>();

        for (int i = 0; i < m.vertices.Length; i++)
        {
            vertices.Add(m.vertices[i]);
            normals.Add(m.normals[i]);
            uv1.Add(m.uv[i]);
            tangents.Add(m.tangents[i]);
        }

        for (int i = m.triangles.Length - 1; i >= 0; i--)
        {
            triangles.Add(m.triangles[i]);
        }

        for (int i = 0; i < m.boneWeights.Length; i++)
        {
            boneWeights.Add(m.boneWeights[i]);
        }

        for (int i = 0; i < m.bindposes.Length; i++)
        {
            bindposes.Add(m.bindposes[i]);
        }

        m.Clear();

        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uv1.ToArray();
        m.normals = normals.ToArray();
        m.tangents = tangents.ToArray();
        m.boneWeights = boneWeights.ToArray();
        m.bindposes = bindposes.ToArray();
    }

}

