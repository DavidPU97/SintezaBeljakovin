using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagramFollowPlayer : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform

    private float startPlaneZPlayer = -37.15f;
    public float endPlaneZPlayer = -15.86f;
    public float distanceToPlayer = 1.5f;

    // -15.86 start Z

    void Update()
    {
        // Get the player's Z position
        float playerZ = player.position.z;

        // Constrain the button's X position within the wall's boundaries
        float clampedZ = Mathf.Clamp(playerZ, startPlaneZPlayer, endPlaneZPlayer);

        // Update the button's position
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ - distanceToPlayer);
    }
}
