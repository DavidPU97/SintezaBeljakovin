using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMeshColliders : MonoBehaviour
{
    public float yThreshold = 0f; // The Y coordinate threshold
    public bool disableObjects = false;
    public GameObject ParentObject;

    void Start()
    {
        Transform children = ParentObject.transform;

        foreach (Transform child in children)
        {
            GameObject obj = child.gameObject;
            if (obj.TryGetComponent(out MeshCollider meshCollider))
            {
                if (obj.transform.position.y > yThreshold)
                {
                    Debug.Log("Name: " + obj.name + " Y: " + obj.transform.position.y);
                    meshCollider.enabled = true;
                    if (disableObjects)
                    {
                        Debug.Log("Name: " + obj.name + " Y: " + obj.transform.position.y + " Active: " + obj.activeSelf);
                        obj.SetActive(false);
                    }
                }
            }

        }
    }
}
