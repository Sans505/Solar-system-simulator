using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class BodyCreationManager : MonoBehaviour
{
    //[SerializeField] private GameObject planet;

    [SerializeField] private SelectedBodyManager selectedBodyManager;
    [SerializeField] private NBodySimulation simulator;
    [SerializeField] private GameObject creationSubPanel;
    [SerializeField] private BodyListMenuManager bodyListMenuManager;

    public TMP_InputField nameInput;

    public TMP_Dropdown dropdown;

    public TMP_InputField XposInput;
    public TMP_InputField YposInput;
    public TMP_InputField ZposInput;

    public void Activate() {
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void CreateBody() {
        if (simulator.isSimulating) {
            NotificationManager.instance.ShowNotification("Error: No se pueden crear astros durante la simulación", NotificationType.Error);
            return;
        }
        creationSubPanel.SetActive(true);
    }

    public void Create() {

        string name = nameInput.text;

        if (name == "") {
            NotificationManager.instance.ShowNotification("Nombre inválido", NotificationType.Warning);
            return;
        }

        if (bodyListMenuManager.containsEntry(name)) {
            NotificationManager.instance.ShowNotification("Ya existe un astro llamado " + name, NotificationType.Warning);
            return;
        }

        Vector3 pos = new Vector3
            (float.Parse(XposInput.text),
            float.Parse(YposInput.text),
            float.Parse(ZposInput.text));

        GameObject newBody = new GameObject();

        CelestialBody celestialBody = newBody.AddComponent<CelestialBody>();
        newBody.transform.position = pos;

        if (dropdown.value == 0) {
            CreatePlanet(newBody);
        } else {
            CreateSun(newBody);
        }

        GameObject newBodyInstance = Instantiate(newBody, simulator.transform);
        newBodyInstance.name = name;
        simulator.AddNewBody(newBodyInstance);
        
        Planet planet = newBodyInstance.GetComponent<Planet>();
        Sun sun = newBodyInstance.GetComponent<Sun>();
        if (planet != null) {
            planet.GeneratePlanet();
        } else if (sun != null){
            sun.simulator = simulator;
            sun.GenerateSun();
        }

        selectedBodyManager.SelectBody(newBodyInstance, true);

        creationSubPanel.SetActive(false);
        NotificationManager.instance.ShowNotification("Astro creado con éxito", NotificationType.Info);
    }

    public void Cancel() {
        NotificationManager.instance.ShowNotification("Creación cancelada", NotificationType.Info);
        creationSubPanel.SetActive(false);
    }

    private void CreatePlanet(GameObject newBody) {
        Planet planet = newBody.AddComponent<Planet>();

        ShapeSettings shapeSettings = ScriptableObject.CreateInstance<ShapeSettings>();
        ColourSettings colourSettings = ScriptableObject.CreateInstance<ColourSettings>();

        ShapeSettings.NoiseLayer noiseLayer = new ShapeSettings.NoiseLayer();
        noiseLayer.noiseSettings = new NoiseSettings();
        shapeSettings.noiseLayers = new ShapeSettings.NoiseLayer[1];
        shapeSettings.noiseLayers[0] = noiseLayer;

        ColourSettings.BiomeColourSettings.Biome biome = new ColourSettings.BiomeColourSettings.Biome();
        biome.gradient = new Gradient();
        biome.tint = Color.white;
        biome.startHeight = 0f;
        colourSettings.biomeColourSettings = new ColourSettings.BiomeColourSettings();
        colourSettings.biomeColourSettings.biomes = new ColourSettings.BiomeColourSettings.Biome[1];
        colourSettings.biomeColourSettings.biomes[0] = biome;

        NoiseSettings colourNoiseSettings = new NoiseSettings();
        colourNoiseSettings.strength = 0.75f;
        colourNoiseSettings.numLayers = 3;
        colourNoiseSettings.baseRoughness = 1.65f;
        colourNoiseSettings.roughness = 3f;
        colourNoiseSettings.persistence = 0.3f;
        colourNoiseSettings.minValue = 0.05f;
        
        colourSettings.biomeColourSettings.noise = colourNoiseSettings;
        colourSettings.biomeColourSettings.noiseOffset = 0.4f;
        colourSettings.biomeColourSettings.noiseStrength = 0.1f;
        colourSettings.biomeColourSettings.blendAmount = 0.4f;

        Shader shader = Shader.Find("Shader Graphs/Planet");
        Material material = new Material(shader);
        colourSettings.planetMaterial = material;

        planet.shapeSettings = shapeSettings;
        planet.colourSettings = colourSettings;
    }

    private void CreateSun(GameObject newBody) {
        Sun sun = newBody.AddComponent<Sun>();

        SunSettings sunSettings = ScriptableObject.CreateInstance<SunSettings>();
        sunSettings.BaseColor = Color.white;

        Shader shader = Shader.Find("Shader Graphs/Sun");
        Material material = new Material(shader);
        sunSettings.sunMaterial = material;

        sun.sunSettings = sunSettings;
    }

}
