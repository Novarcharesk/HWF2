using UnityEngine;

public class FlameHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Box")) return;

        Debug.Log($"{other.gameObject.name} destroyed by {gameObject.name}");
        Destroy(other.gameObject);
    }
}
