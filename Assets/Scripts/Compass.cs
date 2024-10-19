using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform player;          // Reference to the player's transform
    public Transform gardenDoor;      // Reference to the gardenDoor's transform

    public float distanceToDoor;

    void Update()
    {
        distanceToDoor = (gardenDoor.position - player.position).magnitude;
        if (distanceToDoor > 3f)
        {
            // Calculate the direction to the gardenDoor
            Vector3 directionToDoor = gardenDoor.position - player.position;
            directionToDoor.y = 0; // Ignore y-axis for horizontal direction
            directionToDoor.Normalize(); // Normalize to get the direction vector

            // Calculate the angle in degrees relative to the player's forward direction
            float angle = Mathf.Atan2(directionToDoor.x, directionToDoor.z) * Mathf.Rad2Deg;

            // Rotate the compass image to point toward the gardenDoor
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -angle);
            
        }
    }
}