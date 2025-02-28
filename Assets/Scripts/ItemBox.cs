using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private float respawnTime = 30f;         // Time before box respawns (optional, for future use)
    [SerializeField] private GameObject boxVisual;           // The visible part of the box
    [SerializeField] private HazardSpawner HazardSpawners;   // Reference to the spawner (can be left unassigned in prefab)

    private bool isActive = true;
    private HazardSpawner cachedHazardSpawners; // Cache the spawner for efficiency

    void Awake()
    {
        // Try to find the HazardSpawners in the scene if not assigned in Inspector
        if (HazardSpawners == null)
        {
            HazardSpawners = FindObjectOfType<HazardSpawner>();
            if (HazardSpawners != null)
            {
                Debug.Log("Found HazardSpawners dynamically: " + HazardSpawners.name);
            }
            else
            {
                Debug.LogError("Could not find HazardSpawners in the scene!");
            }
        }
        cachedHazardSpawners = HazardSpawners; // Cache it
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Log collision
        Debug.Log("ItemBox isActive: " + isActive); // Log active state
        Debug.Log("Player tag check: " + other.CompareTag("Player")); // Log tag check
        if (isActive && other.CompareTag("Player"))
        {
            if (cachedHazardSpawners != null) // Use cached reference
            {
                Debug.Log("HazardSpawners assigned: " + cachedHazardSpawners.name); // Log spawner assignment
                Debug.Log("HazardSpawners active: " + cachedHazardSpawners.gameObject.activeInHierarchy); // Log if spawner is active
                HazardSpawner spawnerCheck = cachedHazardSpawners.GetComponent<HazardSpawner>();
                if (spawnerCheck != null)
                {
                    Debug.Log("HazardSpawner component found and valid");
                }
                else
                {
                    Debug.LogError("HazardSpawner component is missing on " + cachedHazardSpawners.name);
                }
                Debug.Log("Triggering HazardSpawner to spawn hazards"); // Log before triggering
                cachedHazardSpawners.SpawnHazards(); // Tell the spawner to do its job
            }
            else
            {
                Debug.LogError("HazardSpawners is not assigned or script is missing on " + gameObject.name);
                // Check if the component exists but script is missing
                HazardSpawner spawner = GetComponent<HazardSpawner>(); // Quick check
                if (spawner == null)
                {
                    Debug.LogWarning("No HazardSpawner component found on this GameObject or referenced GameObject");
                }
            }
            Deactivate();
        }
        else
        {
            Debug.LogWarning("Collision conditions not met: isActive=" + isActive + ", PlayerTag=" + other.CompareTag("Player"));
        }
    }

    private void Deactivate()
    {
        isActive = false;
        if (boxVisual != null) boxVisual.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        // Instead of respawning here, let ItemBoxSpawner handle respawning
        Destroy(gameObject, 0.1f); // Destroy after a small delay to ensure collision is processed
    }
}