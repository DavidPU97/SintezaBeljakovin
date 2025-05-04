using UnityEngine;

public class InflateMeshCollider : MonoBehaviour
{
    public float inflateAmount = 0.1f; // Amount to expand the collider

    void Start()
    {
        // Get the MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            //Debug.LogError("No MeshFilter found on the GameObject!");
            return;
        }

        // Create a copy of the original mesh
        Mesh originalMesh = meshFilter.sharedMesh;
        Mesh colliderMesh = Instantiate(originalMesh);

        // Inflate the collider mesh by moving vertices outward along their normals
        Vector3[] vertices = colliderMesh.vertices;
        Vector3[] normals = colliderMesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * inflateAmount; // Expand vertices along their normals
        }

        // Update the collider mesh
        colliderMesh.vertices = vertices;
        colliderMesh.RecalculateBounds();
        colliderMesh.RecalculateNormals();

        // Assign the inflated mesh to the MeshCollider
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            //Debug.LogError("No MeshCollider found on the GameObject!");
            return;
        }

        meshCollider.sharedMesh = colliderMesh; // Assign the inflated mesh to the collider

        //Debug.Log("Mesh Collider successfully inflated!");
    }
}
