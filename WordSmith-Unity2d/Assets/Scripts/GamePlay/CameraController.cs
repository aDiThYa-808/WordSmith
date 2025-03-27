using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    // Camera boundaries
    [SerializeField] private float minX; // Left boundary
    [SerializeField] private float maxX; // Right boundary

    private void Update()
    {
        // Calculate the desired camera position
        float targetX = player.position.x + lookAhead;

        // Clamp the position within the boundaries
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Update camera position
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smoothly adjust lookAhead based on player's direction
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }
}
