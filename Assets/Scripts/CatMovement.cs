using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed at which the cat moves
    public float patrolRange = 5f;     // Distance within which the cat will roam
    public float changeDirectionTime = 2f; // Time interval to change direction

    private Vector3 targetPosition;     // Current target position for the cat
    private float timer;                 // Timer to track movement changes

    private void Start()
    {
        // Set the initial target position for the cat
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
        // Move the cat towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
}
