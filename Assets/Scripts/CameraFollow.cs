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
            transform.position = CalculatorCameraPosition(player.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        // // Calculate the desired position of the camera
        Vector3 desiredPosition = CalculatorCameraPosition(player.transform.position);

        // // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // // Clamp để không vượt ra khỏi giới hạn map
        // float clampedX = Mathf.Clamp(smoothedPosition.x, minPos.x, maxPos.x);
        // float clampedY = Mathf.Clamp(smoothedPosition.y, minPos.y, maxPos.y);

        // transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);

        // Update the camera's position
        transform.position = smoothedPosition;

    }

    private Vector3 CalculatorCameraPosition(Vector3 playerPos)
    {
        Vector3 newPos = new Vector3(playerPos.x, 0, 0);
        Vector3 cameraPos = newPos + offset;
        return cameraPos;
    }
}
