using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed at which the cat moves
    public float patrolRange = 5f;     // Distance within which the cat will roam
    public float changeDirectionTime = 2f; // Time interval to change direction
    public float rotationSpeed = 5f;   // Speed at which the cat rotates

    private Vector3 targetPosition;     // Current target position for the cat
    private float timer;                // Timer to track movement changes
    private Quaternion targetRotation;  // Target rotation for smooth turning
    public GameObject player;

    private void Start()
    {
        // Set the initial target position for the cat
        player = GameObject.Find("Player");
        SetNewTargetPosition();
        timer = changeDirectionTime; // Initialize the timer
    }

    private void Update()
    {
        // Move the cat towards the target position
        MoveToTarget();

        // Count down the timer and change direction if it reaches zero
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetNewTargetPosition(); // Change the target position
            timer = changeDirectionTime; // Reset the timer
        }
    }

    private void MoveToTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Keep the movement on the same y-axis level

        // Only move and rotate if the direction is significant
        if (direction.magnitude > 0.1f)
        {
            // Smoothly rotate the cat towards the target direction
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the cat towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        CheckForCatPickup();
    }

    private void SetNewTargetPosition()
    {
        // Calculate a random target position within the patrol area
        targetPosition = new Vector3(
            transform.position.x + Random.Range(-patrolRange, patrolRange),
            transform.position.y, // Keep the same height
            transform.position.z + Random.Range(-patrolRange, patrolRange)
        );
    }

    // When the cat collides with something, change its direction
    private void OnCollisionEnter(Collision collision)
    {
        // When the cat collides with an object, set a new random direction
        SetNewTargetPosition();
    }
        void CheckForCatPickup(){
        if((player.gameObject.transform.position - gameObject.transform.position).magnitude < 1 ){
            gameObject.GetComponent<Outline>().enabled = true;
        } else{
            gameObject.GetComponent<Outline>().enabled = false;
        }
        
    }
}