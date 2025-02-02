using UnityEngine;
using System.Collections.Generic;

public class EmissionProximity : MonoBehaviour
{
    public Transform movingLightSource;  // The object that moves around, affecting emission
    public Renderer[] emissiveObjects;   // Objects with emissive materials
    public float maxEmission = 2f;       // Maximum emission intensity multiplier
    public float minEmission = 0.2f;     // Minimum emission intensity multiplier
    public float range = 5f;             // The distance within which emission is affected

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
    }

    void Update()
    {
        if (movingLightSource == null) return; // Avoid errors if no source is assigned

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