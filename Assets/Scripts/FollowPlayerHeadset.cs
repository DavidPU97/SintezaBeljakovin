using UnityEngine;

public class FollowPlayerHeadset : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform playerCamera;                  // Assign Main Camera here
    public float distanceFromCamera = 1.5f;         // How far in front of the player
    public float heightOffset = -0.3f;              // Adjust up/down positioning
    public bool followRotation = true;             // Should it face the camera?

    private void Start()
    {
        if (playerCamera == null)
        {
            // Try to auto-find the Main Camera
            Camera mainCam = Camera.main;
            if (mainCam != null)
                playerCamera = mainCam.transform;
            else
                Debug.LogWarning("Player Camera not assigned in FollowPlayerHeadset.");
        }
    }

    private void LateUpdate()
    {
        if (playerCamera == null)
            return;

        // Position directly in front of the camera, with height offset
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * distanceFromCamera;
        targetPosition.y += heightOffset;
        transform.position = targetPosition;

        // Optionally rotate to always face the player
        if (followRotation)
        {
            Vector3 lookDirection = playerCamera.position - transform.position;
            lookDirection.y = 0; // Keep upright if you want
            transform.rotation = Quaternion.LookRotation(-lookDirection.normalized);
        }
    }
}
