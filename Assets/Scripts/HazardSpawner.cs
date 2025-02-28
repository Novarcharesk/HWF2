using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] hazardPrefabs; // Array of hazards to spawn
    [SerializeField] private Transform[] spawnPoints;    // Array of spawn locations

    public void SpawnHazards()
    {
        if (hazardPrefabs.Length == 0)
        {
            Debug.LogWarning("No hazard prefabs assigned to " + gameObject.name);
            return;
        }

        Debug.Log("Spawning hazards from " + gameObject.name); // Log when spawning starts

        // Spawn all hazards at corresponding spawn points (or at spawner position if no points)
        for (int i = 0; i < hazardPrefabs.Length; i++)
        {
            if (hazardPrefabs[i] == null)
            {
                Debug.LogWarning("Hazard prefab at index " + i + " is null in " + gameObject.name);
                continue;
            }

            Vector3 position = (spawnPoints.Length > i && spawnPoints[i] != null)
                ? spawnPoints[i].position
                : transform.position;
            Quaternion rotation = (spawnPoints.Length > i && spawnPoints[i] != null)
                ? spawnPoints[i].rotation
                : transform.rotation;

            GameObject spawnedHazard = Instantiate(hazardPrefabs[i], position, rotation);
            Debug.Log("Spawned hazard: " + spawnedHazard.name + " at position " + position); // Log each spawn
        }
    }
}