using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class ControllerTrackingHider : MonoBehaviour
{
    public InputActionProperty trackingStateInput;

    private bool isTracked = false;

    void Update()
    {
        if (trackingStateInput != null && trackingStateInput.action != null)
        {
            var trackingState = trackingStateInput.action.ReadValue<int>();
            bool currentlyTracked = (trackingState & (int)InputTrackingState.Position | (int)InputTrackingState.Rotation) != 0;

            if (currentlyTracked != isTracked)
            {
                isTracked = currentlyTracked;
                gameObject.SetActive(isTracked);
            }
        }
    }
}
