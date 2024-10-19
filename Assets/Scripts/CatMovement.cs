using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 3f;       // Speed at which the cat moves
    public float patrolRange = 5f;     // Distance within which the cat will roam
    public float changeDirectionTime = 2f; // Time interval to change direction
    public float rotationSpeed = 5f;   // Speed at which the cat rotates

    private Vector3 targetPosition;     // Current target position for the cat
    private float timer;                // Timer to track movement changes
    private Quaternion targetRotation;  // Target rotation for smooth turning
    public GameObject player;

    // Follow related variables
    private bool isFollowing = false;   // To check if the cat is following the player
    private int followIndex = 0;        // To set the order of the following cats
    public float followGap = .2f;      // Gap between the cats when following the player

    // Audio variables
    public AudioSource audioSource;      // Assign your AudioSource in the Inspector
    public AudioClip[] catSounds;        // Array to hold different cat sound clips
    

    private void Start()
    {
        // Set the initial target position for the cat
        player = GameObject.Find("Player");
        SetNewTargetPosition();
        timer = changeDirectionTime; // Initialize the timer
        
        // Start playing sounds with delays
        StartCoroutine(PlaySoundsWithDelays());
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isFollowing)
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
        else
        {
            FollowPlayer();
        }

        CheckForCatPickup();
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

    private void OnCollisionEnter(Collision collision)
    {
        // When the cat collides with an object, set a new random direction
        SetNewTargetPosition();
    }

    void CheckForCatPickup()
    {
        // Check the distance between the player and the cat
        if ((player.gameObject.transform.position - gameObject.transform.position).magnitude < 1 && !isFollowing)
        {
            // Highlight the cat if it's within pickup range
            gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    // New function to start following the player
    public void StartFollowing(int index)
    {
        isFollowing = true;
        followIndex = index;
    }

    // Make the cat follow the player
    private void FollowPlayer()
    {
        if (player != null)
        {
            // Calculate the position behind the player with a gap based on followIndex
            Vector3 followPosition = player.transform.position - player.transform.forward * followGap * followIndex;

            // Move smoothly towards the follow position
            transform.position = Vector3.Lerp(transform.position, followPosition, moveSpeed * Time.deltaTime);

            // Rotate to face the same direction as the player
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Coroutine to play sounds with delays
    private IEnumerator PlaySoundsWithDelays()
    {
    for (int i = 0; i < catSounds.Length; i++)
    {
        // Play the sound
        audioSource.clip = catSounds[i];
        audioSource.Play();

        // Wait for 1 second before playing the next sound
        yield return new WaitForSeconds(0f); // Fixed delay of 1 second
    }
    }
}