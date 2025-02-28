using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private float respawnTime = 30f;         // Time before box respawns
    [SerializeField] private GameObject boxVisual;           // The visible part of the box
    [SerializeField] private HazardSpawner HazardSpawners;   // Reference to the spawner

    private bool isActive = true;
    private float timer;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Log collision
        Debug.Log("ItemBox isActive: " + isActive); // Log active state
        Debug.Log("Player tag check: " + other.CompareTag("Player")); // Log tag check
        if (isActive && other.CompareTag("Player"))
        {
            if (HazardSpawners != null)
            {
                Debug.Log("HazardSpawners assigned: " + HazardSpawners.name); // Log spawner assignment
                Debug.Log("HazardSpawners active: " + HazardSpawners.gameObject.activeInHierarchy); // Log if spawner is active
                HazardSpawner spawnerCheck = HazardSpawners.GetComponent<HazardSpawner>();
                if (spawnerCheck != null)
                {
                    Debug.Log("HazardSpawner component found and valid");
                }
                else
                {
                    Debug.LogError("HazardSpawner component is missing on " + HazardSpawners.name);
                }
                Debug.Log("Triggering HazardSpawner to spawn hazards"); // Log before triggering
                HazardSpawners.SpawnHazards(); // Tell the spawner to do its job
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

    void Update()
    {
        if (!isActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Respawn();
            }
        }
    }

    private void Deactivate()
    {
        isActive = false;
        if (boxVisual != null) boxVisual.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        timer = respawnTime;
    }

    private void Respawn()
    {
        isActive = true;
        if (boxVisual != null) boxVisual.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}