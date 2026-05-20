using UnityEngine;

public class Sun : MonoBehaviour, SharedSettings
{
    private NBodySimulation simulator;
    public bool autoUpdate = true;
    public SunSettings sunSettings;
    [SerializeField, HideInInspector]
    GameObject sphere;
    [SerializeField, HideInInspector]
    Light pointLight;

    void Start()
    {
        simulator = transform.parent.GetComponent<NBodySimulation>();
    }

    private void OnValidate() {
        GenerateSun();
    }
    public void OnSettingsUpdated()
    {
        if (autoUpdate)
        {
            GenerateSun();
        }
    }
    public void GenerateSun()
    {

        if (sphere == null)
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = transform;
        }
        if (pointLight == null)
        {
            pointLight = gameObject.AddComponent<Light>();
            pointLight.type = LightType.Point;
        }

        sphere.transform.localScale = Vector3.one * sunSettings.sunRadius * 2f;

        pointLight.intensity = 1f;
        pointLight.range = 10f;

        Material sunMaterial = sunSettings.sunMaterial;
        sunMaterial.SetFloat("_NoiseScale", sunSettings.NoiseScale);
        sunMaterial.SetFloat("_NoisePower", sunSettings.NoisePower);
        float factor = Mathf.Pow(2, sunSettings.shaderIntensity);
        sunMaterial.SetColor("_BaseColor", sunSettings.BaseColor * factor);
        sunMaterial.SetFloat("_TwirlStrength", sunSettings.TwirlStrength);
        sunMaterial.SetFloat("_DistorsionScale", sunSettings.DistorsionScale);
        sunMaterial.SetVector("_PanSpeed", sunSettings.PanSpeed);

        Renderer renderer = sphere.GetComponent<Renderer>();
        renderer.material = sunMaterial;
    }

    void Update()
    {
        float maximumDistance = 0f;
        foreach (GameObject body in simulator.bodyList)
        {
            if (body != this)
            {
                float distance = Vector3.Distance(transform.position, body.transform.position);
                if (distance > maximumDistance)
                {
                    maximumDistance = distance;
                }
            }
        }
        if (Mathf.Abs(pointLight.range - maximumDistance) > 50f)
        {
            pointLight.range = maximumDistance + 100f;
        }
    }
    public float getRadius()
    {
        return sunSettings.sunRadius;
    }
}
