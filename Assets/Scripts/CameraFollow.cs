using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // The player object to follow
    public float smoothSpeed = 0.125f; // Speed of the camera movement
    public Vector3 offset; // Offset from the player position


    public float minPosX; // Minimum position x limits for the camera
    public float maxPosX; // Maximum position x limits for the camera


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


        AlignCameraLeftToWorldZero();
        AlignCameraRighttToWorldZero(new Vector3(31.0f, 0f, 0f));
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

        // Update the camera's position
        transform.position = smoothedPosition;

    }

    private Vector3 CalculatorCameraPosition(Vector3 playerPos)
    {
        Vector3 newPos = new Vector3(playerPos.x, 0, 0);
        Vector3 cameraPos = newPos + offset;
        if (cameraPos.x < minPosX)
        {
            cameraPos.x = minPosX;
        }

        if (cameraPos.x > maxPosX)
        {
            cameraPos.x = maxPosX;
        }
        return cameraPos;
    }

    void AlignCameraLeftToWorldZero(float targetViewportX = 0.05f)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Không tìm thấy camera chính!");
            return;
        }

        // Vị trí world mà bạn muốn canh (ở đây là Vector3.zero)
        Vector3 worldPoint = Vector3.zero;

        // Tính Viewport hiện tại của world point
        Vector3 currentViewportPoint = cam.WorldToViewportPoint(worldPoint);
        float currentViewportX = currentViewportPoint.x;

        Debug.Log("Viewport X hiện tại của (0,0,0): " + currentViewportX);

        float deltaViewportX = targetViewportX - currentViewportX;

        // Chuyển đổi chênh lệch viewport sang khoảng cách thế giới
        float worldUnitsToMove = deltaViewportX * cam.orthographicSize * 2 * cam.aspect;

        // Dịch chuyển camera theo trục X
        cam.transform.position -= new Vector3(worldUnitsToMove, 0, 0);

        minPosX = cam.transform.position.x;

        Debug.Log($"Left: {cam.transform.position}");
    }


    void AlignCameraRighttToWorldZero(Vector3 worldPoint, float targetViewportX = 0.95f)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Không tìm thấy camera chính!");
            return;
        }

        // Vị trí world mà bạn muốn canh (ở đây là Vector3.zero)
        if (worldPoint == Vector3.zero || worldPoint == null)
        {
            worldPoint = new Vector3(31.0f, 0f, 0f);
        }

        // Tính Viewport hiện tại của world point
        Vector3 currentViewportPoint = cam.WorldToViewportPoint(worldPoint);
        float currentViewportX = currentViewportPoint.x;

        Debug.Log("Right: Viewport X hiện tại của (0,0,0): " + currentViewportX);

        float deltaViewportX = targetViewportX - currentViewportX;

        // Chuyển đổi chênh lệch viewport sang khoảng cách thế giới
        float worldUnitsToMove = deltaViewportX * cam.orthographicSize * 2 * cam.aspect;

        // Dịch chuyển camera theo trục X
        Vector3 position = cam.transform.position;
        position -= new Vector3(worldUnitsToMove, 0, 0);

        position.x = Mathf.Round(position.x * 100f) / 100f - 1f; // Làm tròn đến 2 chữ số thập phân

        maxPosX = position.x;

        Debug.Log("Right: " + position);
    }


}
