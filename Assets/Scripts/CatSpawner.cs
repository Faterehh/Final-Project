using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject catPrefab; // Reference to the cat prefab

    public int numberOfCats = 10; // Number of cats to spawn
    public BoxCollider spawnArea;  // Reference to the patrol area
   

    private void Start()
    {
        SpawnCats();
    }

   void SpawnCats()
{
    // Get the bounds of the BoxCollider
    Bounds bounds = spawnArea.bounds;

    for (int i = 0; i < numberOfCats; i++)
    {
        // Generate a random position inside the BoxCollider's bounds
        Vector3 randomPosition = GetRandomPositionWithinBounds(bounds);

        // Spawn the cat at the random position
        Instantiate(catPrefab, randomPosition, Quaternion.identity);
    }
}
Vector3 GetRandomPositionWithinBounds(Bounds bounds)
{
    // Generate random x, y, and z coordinates within the bounds of the BoxCollider
    float randomX = Random.Range(bounds.min.x, bounds.max.x);
    float randomY = bounds.min.y; // Set y to the ground or fixed height (e.g., 7)
    float randomZ = Random.Range(bounds.min.z, bounds.max.z);

    return new Vector3(randomX, randomY, randomZ);
}

}
