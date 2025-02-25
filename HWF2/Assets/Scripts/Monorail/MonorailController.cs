using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonorailController : MonoBehaviour
{
    [System.Serializable]
    public class Monorail
    {
        public Transform monorailTransform; // Reference to the monorail GameObject
        public Transform startPoint; // Start position
        public Transform endPoint; // End position
    }

    public List<Monorail> monorails = new List<Monorail>();
    public float totalGameTime = 60f; // Total duration for all monorails to complete their cycles
    public AnimationCurve movementCurve; // Optional curve for smooth motion

    private void Start()
    {
        StartCoroutine(ControlMonorails());
    }

    private IEnumerator ControlMonorails()
    {
        float segmentTime = totalGameTime / monorails.Count; // Time each monorail gets

        foreach (var monorail in monorails)
        {
            yield return MoveMonorail(monorail, segmentTime);
        }
    }

    private IEnumerator MoveMonorail(Monorail monorail, float segmentTime)
    {
        float moveTime = segmentTime * 0.5f; // Half of the time for moving
        float waitTime = segmentTime * 0.5f; // Half of the time for waiting

        // Move to position
        yield return MoveBetweenPoints(monorail.monorailTransform, monorail.startPoint.position, monorail.endPoint.position, moveTime);

        // Wait at the position
        yield return new WaitForSeconds(waitTime);

        // Move back to start
        yield return MoveBetweenPoints(monorail.monorailTransform, monorail.endPoint.position, monorail.startPoint.position, moveTime);
    }

    private IEnumerator MoveBetweenPoints(Transform obj, Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            if (movementCurve != null)
            {
                t = movementCurve.Evaluate(t);
            }

            obj.position = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.position = end;
    }
}