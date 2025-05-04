using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class Polygon2Dto3DMesh : MonoBehaviour
{
    public Material meshMaterial;
    public float extrusionDepth = 0.1f;

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        PolygonCollider2D poly2D = GetComponent<PolygonCollider2D>();
        if (poly2D == null || poly2D.points.Length < 3) return;

        Vector2[] points = poly2D.points;
        Triangulator triangulator = new Triangulator(points);
        int[] indices = triangulator.Triangulate();

        int vertCount = points.Length;
        Vector3[] vertices = new Vector3[vertCount * 2];
        int[] triangles = new int[indices.Length * 2 + vertCount * 6];

        // Create front and back vertices
        for (int i = 0; i < vertCount; i++)
        {
            vertices[i] = new Vector3(points[i].x, points[i].y, -extrusionDepth / 2); // front
            vertices[i + vertCount] = new Vector3(points[i].x, points[i].y, extrusionDepth / 2); // back
        }

        // Front face triangles
        for (int i = 0; i < indices.Length; i += 3)
        {
            triangles[i] = indices[i];
            triangles[i + 1] = indices[i + 1];
            triangles[i + 2] = indices[i + 2];
        }

        // Back face triangles (reversed)
        int offset = indices.Length;
        for (int i = 0; i < indices.Length; i += 3)
        {
            triangles[offset + i] = indices[i + 2] + vertCount;
            triangles[offset + i + 1] = indices[i + 1] + vertCount;
            triangles[offset + i + 2] = indices[i] + vertCount;
        }

        // Side faces
        offset += indices.Length;
        for (int i = 0; i < vertCount; i++)
        {
            int next = (i + 1) % vertCount;

            // 2 triangles per side quad
            triangles[offset++] = i;
            triangles[offset++] = i + vertCount;
            triangles[offset++] = next;

            triangles[offset++] = next;
            triangles[offset++] = i + vertCount;
            triangles[offset++] = next + vertCount;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject meshObj = GameObject.Find(name + "_3DMesh") ?? new GameObject(name + "_3DMesh");
        meshObj.transform.position = transform.position;
        meshObj.transform.rotation = transform.rotation;
        meshObj.transform.localScale = transform.localScale;

        MeshFilter mf = meshObj.GetComponent<MeshFilter>();
        if (mf == null) mf = meshObj.AddComponent<MeshFilter>();
        MeshRenderer mr = meshObj.GetComponent<MeshRenderer>();
        if (mr == null) mr = meshObj.AddComponent<MeshRenderer>();

        MeshCollider mc = meshObj.GetComponent<MeshCollider>();
        if (mc == null) mc = meshObj.AddComponent<MeshCollider>();

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh;
        mr.sharedMaterial = meshMaterial;
    }
}
