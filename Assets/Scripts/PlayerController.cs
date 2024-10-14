using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;         // Speed of player movement
    public float lookSensitivity = 2f;   // Sensitivity of mouse look
    public float maxLookX = 60f;         // Max rotation upwards
    public float minLookX = -60f;        // Max rotation downwards

    private float rotX;                  // Current X rotation (up/down)

    private CharacterController characterController; // Reference to CharacterController
    private Camera playerCamera;         // Reference to the camera

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Get the player's camera (it should be a child of the player)
        playerCamera = Camera.main;

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();         // Handle player movement
        CameraLook();   // Handle mouse camera rotation
    }

    // Handle player movement (WASD keys) and jumping
    void Move()
    {

        // Get input for movement on the X and Z axes
        float moveX = Input.GetAxis("Horizontal");  // A/D or left/right
        float moveZ = Input.GetAxis("Vertical");    // W/S or forward/backward

        // Move the player relative to their local direction (transform)
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

    }

    // Handle camera and player rotation (using mouse)
    void CameraLook()
    {
        // Get mouse movement for looking up/down and left/right
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Rotate the player left and right with the mouseX input (Y-axis rotation)
        transform.Rotate(Vector3.up * mouseX);

        // Calculate the new X rotation for looking up and down
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);  // Clamp rotation to prevent over-rotating

        // Apply the rotation to the player's camera (only affects the camera's X rotation)
        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }

}
