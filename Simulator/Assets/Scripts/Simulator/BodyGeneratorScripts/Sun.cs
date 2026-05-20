using UnityEngine;

public class Sun : MonoBehaviour
{
    public bool autoUpdate = true;
    public SunSettings sunSettings;
    [SerializeField, HideInInspector]
    GameObject sphere;
    [SerializeField, HideInInspector]
    Light pointLight;

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

        pointLight.intensity = sunSettings.lightIntensity;
        pointLight.range = sunSettings.lightRange;

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
}
