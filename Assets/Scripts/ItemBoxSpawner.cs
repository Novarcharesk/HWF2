using UnityEngine;

public class ItemBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemBoxPrefab;      // The ItemBox prefab to spawn
    [SerializeField] private Transform[] spawnPoints;       // Array of potential spawn locations
    [SerializeField] private float minSpawnInterval = 20f;  // Minimum time between spawns
    [SerializeField] private float maxSpawnInterval = 40f;  // Maximum time between spawns
    [SerializeField] private bool spawnOnStart = true;      // Whether to spawn immediately on start

    private float spawnTimer;
    private GameObject currentItemBox; // Track the currently spawned ItemBox

    void Start()
    {
        if (spawnOnStart && spawnPoints.Length > 0 && itemBoxPrefab != null)
        {
            SpawnItemBox();
        }
        ResetSpawnTimer();
    }

    void Update()
    {
        if (currentItemBox == null) // If the ItemBox is collected or destroyed
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnItemBox();
                ResetSpawnTimer();
            }
        }
    }

    private void SpawnItemBox()
    {
        if (spawnPoints.Length == 0 || itemBoxPrefab == null)
        {
            Debug.LogWarning("No spawn points or item box prefab assigned to " + gameObject.name);
            return;
        }

        // Choose a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn the ItemBox at the random location
        currentItemBox = Instantiate(itemBoxPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawned ItemBox at " + spawnPoint.position);
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}