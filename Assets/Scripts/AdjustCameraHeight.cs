using Unity.XR.CoreUtils;
using UnityEngine;

public class AdjustCameraHeight : MonoBehaviour
{
    public XROrigin xrOrigin;
    public float desiredHeight = 1.7f;

    void Start()
    {
        // Get the current camera's position
        Vector3 cameraInRigSpace = xrOrigin.CameraInOriginSpacePos;

        // Adjust just the Y
        Vector3 newPosition = new Vector3(0, desiredHeight + cameraInRigSpace.y, 0);

        // Move the whole rig to apply the offset
        xrOrigin.MoveCameraToWorldLocation(newPosition);
    }
}
