using UnityEngine;

public class BounceHazard : MonoBehaviour
{
    [SerializeField] private float _bounceStrength = 1f;

    private PlayerController _player;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("collided with player");

        if (!_player)
        {
            if (!other.TryGetComponent<PlayerController>(out _player))
                Debug.Log($"Player controller not found when colliding with {gameObject.name}");
        }
        
        _player.Bounce(_bounceStrength);
    }
}
