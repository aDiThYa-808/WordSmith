using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera movement settings")]
    [SerializeField] private float FollowSpeed = 2f;
    [SerializeField] private float yOffset = 1f;
    public Transform target;

    // Camera boundaries
    [Header("Boundaries")]
    [SerializeField] private float minX, maxX; // Left and right boundaries
    [SerializeField] private float minY, maxY; // Bottom and top boundaries

    // Update is called once per frame
    void Update()
    {
        // Create the desired camera position
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

        // Clamp the camera position to stay within boundaries
        float clampedX = Mathf.Clamp(newPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(newPos.y, minY, maxY);

        // Set the camera's position smoothly
        transform.position = Vector3.Slerp(transform.position, new Vector3(clampedX, clampedY, newPos.z), FollowSpeed * Time.deltaTime);
    }
}
