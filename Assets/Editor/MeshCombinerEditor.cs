using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class MeshCombinerEditor : EditorWindow
{
    [MenuItem("Tools/Combine Selected Meshes")]
    static void CombineSelectedMeshes()
    {
        // Get all selected GameObjects in the Hierarchy
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected. Please select GameObjects to combine.");
            return;
        }

        CombineInstance[] combine = new CombineInstance[selectedObjects.Length];
        int i = 0;

        foreach (GameObject obj in selectedObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

            if (meshFilter == null)
            {
                Debug.LogWarning("One or more selected objects do not have a MeshFilter.");
                continue;
            }

            combine[i].mesh = meshFilter.sharedMesh;
            combine[i].transform = meshFilter.transform.localToWorldMatrix;
            i++;
        }

        // Create a new GameObject for the combined mesh
        GameObject combinedObject = new GameObject("CombinedMesh");
        MeshFilter meshFilterCombined = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer meshRendererCombined = combinedObject.AddComponent<MeshRenderer>();

        // Create and assign the combined mesh
        Mesh combinedMesh = new Mesh();
        // Set the index format to support more vertices if needed
        combinedMesh.indexFormat = IndexFormat.UInt32; // Allow more than 65535 vertices

        combinedMesh.CombineMeshes(combine);
        meshFilterCombined.mesh = combinedMesh;

        // Use the material from the first selected object
        if (selectedObjects[0].GetComponent<MeshRenderer>() != null)
        {
            meshRendererCombined.material = selectedObjects[0].GetComponent<MeshRenderer>().sharedMaterial;
        }

        // Log the result
        Debug.Log("Meshes combined successfully.");
    }
}
