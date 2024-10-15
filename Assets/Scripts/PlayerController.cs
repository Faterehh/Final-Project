using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;         // Speed of player movement
    public float lookSensitivity = 1f;   // Sensitivity of mouse look
    public float LookX = 50f;         // Rotation upwards and downwards
       
    public float pickUpRange = 5f;
    private float rotX;                  // Current X rotation (up/down)

    private CharacterController characterController; // Reference to CharacterController
    private Camera playerCamera;         // Reference to the camera
    private Animator playerAnim;  // Reference to the animator
    
    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("MoveSpeed",0);

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

    // If there's any input (moveX or moveZ), move the player, else stop
    if (moveDirection.magnitude > 0)
    {
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        playerAnim.SetFloat("MoveSpeed", moveSpeed);
    }
    else
    {
        playerAnim.SetFloat("MoveSpeed", 0);  // Stop movement animation
    }
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
        rotX = Mathf.Clamp(rotX, -LookX, LookX);  // Clamp rotation to prevent over-rotating

        // Apply the rotation to the player's camera (only affects the camera's X rotation)
        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }
    // void CheckForCatPickup(){
    //     if((gameObject.transform.position - cat.gameObject.transform.position).magnitude < 1 ){
    //         cat.gameObject.GetComponent<Outline>().enabled = true;
    //     } else{
    //         cat.gameObject.GetComponent<Outline>().enabled = false;
    //     }
        
    // }
   // Cast a ray from the camera to the mouse position
//             Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
//             RaycastHit hit;

//             // Check if the ray hits an object
//             if (Physics.Raycast(ray, out hit))
//             {
//                 // Check if the hit object has the tag "Cat"
//                 if (hit.collider.CompareTag("Cat"))
//                 {
//                     // Calculate distance to the cat
//                     float distanceToCat = Vector3.Distance(transform.position, hit.transform.position);

//                     // Check if the distance is less than the pickup range
//                     if (distanceToCat < pickUpRange)
//                     {
//                         PickUpCat(hit.collider.gameObject); // Pick up the cat
//                     }
//                         // Implement logic to pick up the cat
//     void PickUpCat(GameObject cat)
//     {
//         // For example, destroy the cat or deactivate it
//         Destroy(cat); // or use cat.SetActive(false); to keep it in the scene
//         Debug.Log("Picked up a cat!");
//     }
//                 }
//             }
//         }

// }
}
