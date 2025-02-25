using UnityEngine;

public class GoalZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            EventManager.OnScored?.Invoke();
        }
    }
}
