using UnityEngine;

public class ButtonFollowPlayer : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform

    private float startPlaneXPlayer = -6.741f;
    private float endPlaneXPlayer = 1.76f;

    void Update()
    {
        // Get the player's X position
        float playerX = player.position.x;

        // Constrain the button's X position within the wall's boundaries
        float clampedX = Mathf.Clamp(playerX, startPlaneXPlayer, endPlaneXPlayer);

        // Update the button's position
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
