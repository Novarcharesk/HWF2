using UnityEngine;

public class BounceHazard : MonoBehaviour
{
    [SerializeField] private float _bounceStrength = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.TryGetComponent<PlayerController>(out var player))
            Debug.Log($"Player controller not found when colliding with {gameObject.name}");

        player.Bounce(_bounceStrength);
    }
}
