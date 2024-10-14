using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject catPrefab; // Reference to the cat prefab

    public int numberOfCats; // Number of cats to spawn
    public Transform patrolArea;  // Reference to the patrol area

    private void Start()
    {
        SpawnCats();
    }

    void SpawnCats()
    {
        if(numberOfCats == 0){
                     Vector3 spawnPosition = new Vector3(
                patrolArea.position.x + Random.Range(-patrolArea.localScale.x, patrolArea.localScale.x),
                7,
                patrolArea.position.z + Random.Range(-patrolArea.localScale.z, patrolArea.localScale.z)
            );

            Instantiate(catPrefab, spawnPosition, Quaternion.identity);   
        }
        
    }
}
