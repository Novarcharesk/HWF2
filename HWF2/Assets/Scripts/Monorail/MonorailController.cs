using UnityEngine;
using System.Collections;

public class MonorailGoalController : MonoBehaviour
{
    public Transform leftGoalMonorail;  // Monorail on the left side
    public Transform rightGoalMonorail; // Monorail on the right side
    public float moveSpeed = 10f;  // Speed of the monorail movement
    public float goalDuration = 5f; // How long a monorail stays as a goal

    private Vector3 leftGoalPosition;
    private Vector3 rightGoalPosition;
    private Vector3 leftStartPosition;
    private Vector3 rightStartPosition;

    private void Start()
    {
        // Define goal positions
        leftGoalPosition = Vector3.zero;
        rightGoalPosition = Vector3.zero;

        // Define start positions (offscreen)
        leftStartPosition = leftGoalPosition + Vector3.forward * 20f;
        rightStartPosition = rightGoalPosition + Vector3.forward * 20f;

        // Set initial positions
        leftGoalMonorail.localPosition = leftStartPosition;
        rightGoalMonorail.localPosition = rightStartPosition;

        // Start the monorail movement cycle
        StartCoroutine(GoalCycle());
    }

    private IEnumerator GoalCycle()
    {
        while (true)
        {
            // Move left monorail in and make it a goal
            yield return MoveMonorail(leftGoalMonorail, leftStartPosition, leftGoalPosition);
            yield return new WaitForSeconds(goalDuration);

            yield return MoveMonorail(rightGoalMonorail, rightStartPosition, rightGoalPosition);
            yield return MoveMonorail(leftGoalMonorail, leftGoalPosition, leftStartPosition);

            // Move right monorail in and make it a goal
            yield return new WaitForSeconds(goalDuration);
            yield return MoveMonorail(rightGoalMonorail, rightGoalPosition, rightStartPosition);
        }
    }

    private IEnumerator MoveMonorail(Transform monorail, Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            monorail.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        monorail.localPosition = end;
    }
}