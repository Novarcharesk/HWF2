using UnityEngine;
using System.Collections.Generic;

public class EmissionProximity : MonoBehaviour
{
    public Transform movingLightSource;  // The object that moves around, affecting emission
    public Renderer[] emissiveObjects;   // Objects with emissive materials
    public float maxEmission = 2f;       // Maximum emission intensity multiplier
    public float minEmission = 0.2f;     // Minimum emission intensity multiplier
    public float range = 5f;             // The distance within which emission is affected

    // Circular movement variables
    public Transform pathCenter;         // The center point of the circle
    public float radius = 5f;            // The radius of the circular path
    public float rotationSpeed = 20f;    // The speed of the rotation (degrees per second)
    public float startAngle = 0f;        // The starting angle for the light source (in degrees)

    private float currentAngle;          // Current angle of the moving light source

    private Dictionary<Material, Color> originalEmissionColors = new Dictionary<Material, Color>();

    void Start()
    {
        // Store original emission colors on start
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

        // Initialize the light source's starting position based on startAngle
        currentAngle = startAngle;
        SetInitialPosition();
    }

    void Update()
    {
        if (movingLightSource == null || pathCenter == null) return; // Avoid errors if no source or center is assigned

        // Update circular movement
        MoveInCircle();

        // Update emission based on proximity
        UpdateEmission();
    }

    void SetInitialPosition()
    {
        // Set the initial position based on the startAngle
        float radians = startAngle * Mathf.Deg2Rad; // Convert angle to radians
        float x = pathCenter.position.x + Mathf.Cos(radians) * radius; // X position
        float z = pathCenter.position.z + Mathf.Sin(radians) * radius; // Z position
        movingLightSource.position = new Vector3(x, movingLightSource.position.y, z);
    }

    void MoveInCircle()
    {
        // Calculate the new position using circular movement formulas
        currentAngle += rotationSpeed * Time.deltaTime;  // Increment angle over time
        float radians = currentAngle * Mathf.Deg2Rad;    // Convert angle to radians

        float x = pathCenter.position.x + Mathf.Cos(radians) * radius; // X position based on angle
        float z = pathCenter.position.z + Mathf.Sin(radians) * radius; // Z position based on angle

        // Update the moving light source's position
        movingLightSource.position = new Vector3(x, movingLightSource.position.y, z);
    }

    void UpdateEmission()
    {
        foreach (Renderer rend in emissiveObjects)
        {
            float distance = Vector3.Distance(rend.transform.position, movingLightSource.position);
            float intensity = Mathf.Clamp01(1 - (distance / range)); // Closer = stronger glow
            float emissionStrength = Mathf.Lerp(minEmission, maxEmission, intensity);

            foreach (Material mat in rend.materials)
            {
                if (!mat.HasProperty("_EmissionColor") || !originalEmissionColors.ContainsKey(mat)) continue;

                Color originalEmission = originalEmissionColors[mat]; // Retrieve stored color
                Color finalEmission = originalEmission * emissionStrength;

                mat.SetColor("_EmissionColor", finalEmission);
                mat.EnableKeyword("_EMISSION"); // Ensure Unity applies the emission update
            }
        }
    }
}