using UnityEngine;
using System.Collections.Generic;

public class DynamicGlowDiagonal : MonoBehaviour
{
    public float maxEmission = 2f;       // Maximum emission intensity multiplier
    public float minEmission = 0.2f;     // Minimum emission intensity multiplier
    public float range = 5f;             // Distance within which emission is affected

    public Transform startObject;        // Object representing the lowest point in movement
    public Transform endObject;          // Object representing the highest point in movement
    public float moveSpeed = 2f;         // Speed of movement
    public float horizontalWidth = 5f;   // Width of the glowing effect

    private Dictionary<Material, Color> originalEmissionColors = new Dictionary<Material, Color>();
    private List<Renderer> emissiveObjects = new List<Renderer>();

    private Vector3 moveDirection;
    private Vector3 currentPosition;

    void Start()
    {
        // Find all objects tagged "Wall"
        GameObject[] emissiveObjectsArray = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject obj in emissiveObjectsArray)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                emissiveObjects.Add(rend);
            }
        }

        // Store original emission colors
        foreach (Renderer rend in emissiveObjects)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[mat] = mat.GetColor("_EmissionColor");
                }
            }
        }

        // Set movement direction and start position
        moveDirection = (endObject.position - startObject.position).normalized;
        currentPosition = startObject.position;
    }

    void Update()
    {
        if (startObject == null || endObject == null) return;

        // Move the glow effect from start to end
        currentPosition += moveDirection * moveSpeed * Time.deltaTime;

        // If the object reaches the end, reset to the start position
        if (Vector3.Distance(currentPosition, endObject.position) < 0.1f)
        {
            currentPosition = startObject.position;
        }

        // Update emission based on proximity to the moving horizontal band
        UpdateEmission();
    }

    void UpdateEmission()
    {
        foreach (Renderer rend in emissiveObjects)
        {
            Vector3 objectPosition = rend.transform.position;

            // Calculate the distance from the object to the moving **horizontal band**
            float horizontalDistance = DistanceToHorizontalBand(objectPosition, currentPosition, horizontalWidth);

            float intensity = Mathf.Clamp01(1 - (horizontalDistance / range));

            // Apply exponential smoothing for a more gradual glow increase
            float smoothIntensity = Mathf.Pow(intensity, 2f); // Adjust exponent for more control
            float emissionStrength = Mathf.Lerp(minEmission, maxEmission, smoothIntensity);

            foreach (Material mat in rend.materials)
            {
                if (!mat.HasProperty("_EmissionColor") || !originalEmissionColors.ContainsKey(mat)) continue;

                Color originalEmission = originalEmissionColors[mat];
                Color finalEmission = originalEmission * emissionStrength;

                mat.SetColor("_EmissionColor", finalEmission);
                mat.EnableKeyword("_EMISSION");
            }
        }

        // Reset emission for objects outside range
        ResetEmissionForObjectsOutOfRange();
    }

    void ResetEmissionForObjectsOutOfRange()
    {
        foreach (Renderer rend in emissiveObjects)
        {
            Vector3 objectPosition = rend.transform.position;
            float horizontalDistance = DistanceToHorizontalBand(objectPosition, currentPosition, horizontalWidth);

            if (horizontalDistance >= range) // If out of range, reset emission to minimum
            {
                foreach (Material mat in rend.materials)
                {
                    if (!mat.HasProperty("_EmissionColor") || !originalEmissionColors.ContainsKey(mat)) continue;

                    Color originalEmission = originalEmissionColors[mat];
                    Color finalEmission = originalEmission * minEmission; // Ensure reset to minEmission

                    mat.SetColor("_EmissionColor", finalEmission);
                    mat.EnableKeyword("_EMISSION");
                }
            }
        }
    }

    float DistanceToHorizontalBand(Vector3 point, Vector3 center, float width)
    {
        // Keep the Y-position the same as the center to ensure a **horizontal** effect
        Vector3 closestPoint = new Vector3(point.x, center.y, center.z);

        // Return the distance from the object to the closest point on the **horizontal band**
        return Vector3.Distance(point, closestPoint);
    }
}