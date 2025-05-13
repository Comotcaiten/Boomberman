using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // The player object to follow
    public float smoothSpeed = 0.125f; // Speed of the camera movement
    public Vector3 offset; // Offset from the player position

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found. Please assign the player object in the inspector.");
        }
        else
        {
            // Set the initial position of the camera
            transform.position = player.transform.position + offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.transform.position + offset;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
