using UnityEngine;

public class HideObjectsInZone : MonoBehaviour
{
    public GameObject objectToHide; // Assign the objects to hide in the Inspector
    public bool onlyHide = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the zone
        {
            objectToHide.SetActive(false); // Disable the GameObject
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !onlyHide) // Check if the player left the zone
        {
            objectToHide.SetActive(true); // Re-enable the GameObject
        }
    }
}
