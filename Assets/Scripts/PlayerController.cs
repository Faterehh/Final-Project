using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;         // Speed of player movement
    public float lookSensitivity = 1f;   // Sensitivity of mouse look
    public float LookX = 50f;            // Rotation upwards and downwards
    public float pickUpRange = 1f;

    private float rotX;                  // Current X rotation (up/down)
    private CharacterController characterController; // Reference to CharacterController
    private Camera playerCamera;         // Reference to the camera
    private Animator playerAnim;         // Reference to the animator

    private List<CatMovement> followingCats = new List<CatMovement>(); // List of cats that are following the player
    public GameManager gameManager;      // Reference to gameManager
    public ParticleSystem destroyingParticle;

    public BoxCollider homeCollider;     // Reference to the box collider of the home

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("MoveSpeed", 0);

        // Get the player's camera (it should be a child of the player)
        playerCamera = Camera.main;
    }

    void Update()
    {
        Move();         // Handle player movement
        CameraLook();   // Handle mouse camera rotation
        HandleCatPickup(); // Handle the pickup of cats when pressing space
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");  // A/D or left/right
        float moveZ = Input.GetAxis("Vertical");    // W/S or forward/backward
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            playerAnim.SetFloat("MoveSpeed", moveSpeed);
        }
        else
        {
            playerAnim.SetFloat("MoveSpeed", 0);  // Stop movement animation
        }

        // Call the method to restrict the camera's position
        RestrictCameraPosition();
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -LookX, LookX);  // Clamp rotation to prevent over-rotating

        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }

    // Restrict the camera's position within the bounds of the home
    void RestrictCameraPosition()
    {
        if (homeCollider != null)
        {
            // Get the bounds of the home
            Bounds bounds = homeCollider.bounds;

            // Clamp the camera's position
            float clampedX = Mathf.Clamp(playerCamera.transform.position.x, bounds.min.x, bounds.max.x);
            float clampedY = Mathf.Clamp(playerCamera.transform.position.y, bounds.min.y, bounds.max.y);
            float clampedZ = Mathf.Clamp(playerCamera.transform.position.z, bounds.min.z, bounds.max.z);

            // Set the camera's position to the clamped values
            playerCamera.transform.position = new Vector3(clampedX, clampedY, clampedZ);
        }
    }

    // Handle cat pickup and follow behavior
    void HandleCatPickup()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CatMovement[] cats = FindObjectsOfType<CatMovement>(); // Find all cats in the scene

            foreach (CatMovement cat in cats)
            {
                // If the cat is within pickup range and not already following
                if ((cat.transform.position - transform.position).magnitude <= pickUpRange && !followingCats.Contains(cat))
                {
                    // Add the cat to the list and start following
                    followingCats.Add(cat);
                    cat.StartFollowing(followingCats.Count);  // Pass the index in the line

                    // Update the UI for following cats
                    if (gameManager != null)
                    {
                        gameManager.UpdateFollowingCatsText(followingCats.Count);
                    }
                }
            }
        }
    }

    // Handle collision with the door to make cats disappear and increment score
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GardenDoor") && followingCats.Count > 0)
        {
            // Play the particle effect at the door when the player has following cats
            if (destroyingParticle != null)
            {
                destroyingParticle.Play(); // Play the particle effect at the door
            }

            // Increment the score based on the number of cats
            int catsDisappeared = followingCats.Count;

            // Destroy all following cats
            foreach (CatMovement cat in followingCats)
            {
                Destroy(cat.gameObject);
            }

            followingCats.Clear();

            // Update the UI after cats disappear
            if (gameManager != null)
            {
                gameManager.AddScore(catsDisappeared);
                gameManager.UpdateFollowingCatsText(followingCats.Count);  // This will now show "0"
            }
        }
    }
}