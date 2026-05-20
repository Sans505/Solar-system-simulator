using UnityEngine;
[CreateAssetMenu()]
public class SunSettings : ScriptableObject {

    public float sunRadius = 1;
    public Material sunMaterial;
    public float NoiseScale = 120;
    public float NoisePower = 5;
    public Color BaseColor;
    public float shaderIntensity = 6.7f;
    public float TwirlStrength = 22;
    public float DistorsionScale = 45;
    public Vector2 PanSpeed = new Vector2(0.01f, 0.01f);
    public float lightIntensity;
    public float lightRange;
}