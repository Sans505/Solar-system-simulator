using UnityEngine;
using UnityEngine.UI;

public class EditMenuManager : MonoBehaviour
{
    [SerializeField] private ControladorHeader headerController;
    [SerializeField] private GestorDatosMenu menuDataManager;

    private GameObject body;
    private CelestialBody celestialBody;
    private ShapeSettings bodyShapeSettings;
    private ColourSettings bodyColourSettings;
    private SunSettings bodySunSettings;

    public void Activate(GameObject body) {

        if (body == null) {
            NotificationManager.instance.ShowNotification("Error: No hay astro seleccionado", NotificationType.Error);
            return;
        }

        gameObject.SetActive(true);
        
        this.body = body;
        celestialBody = body.GetComponent<CelestialBody>();
        Sun sun = body.GetComponent<Sun>();
        Planet planet = body.GetComponent<Planet>();

        if (planet != null) {
            bodyShapeSettings = planet.shapeSettings;
            bodyColourSettings = planet.colourSettings;
        } else if (sun != null) {
            bodySunSettings = sun.sunSettings;
        } else {
            NotificationManager.instance.ShowNotification("Error: Información del astro perdida", NotificationType.Error);
            return;
        }

        DisplayBodyData();
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void SetEditMode(bool isSimulating) {
        if (isSimulating) {
            headerController.MostrarSoloFisica();
        } else {
            headerController.MostrarTodosLosBotones();
        }
    }

    private void DisplayBodyData() {

        menuDataManager.RegistrarValoresFijos(celestialBody, body.GetComponent<SharedSettings>());
        menuDataManager.RegistrarValoresRuido(bodyShapeSettings);
        menuDataManager.RegistrarValoresBiomas(bodyColourSettings);
    }

    public void UpdateBodyNumericData(string dataTag, float value, int numCapaOBioma = -1) {

        Vector3 pos;
        bool valueChanged = false;

        switch(dataTag) {
            case "Fuerza":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.strength != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.strength = value;
                break;
            case "Capas":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.numLayers != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.numLayers = (int) value;
                break;
            case "Rug.Base":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.baseRoughness != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.baseRoughness = value;
                break;
            case "Rugosidad":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.roughness != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.roughness = value;
                break;
            case "Persistencia":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.persistence != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.persistence = value;
                break;
            case "CentroX":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.x != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.x = value;
                break;
            case "CentroY":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.y != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.y = value;
                break;
            case "CentroZ":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.z != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.centre.z = value;
                break;
            case "Valor.Min":
                valueChanged = bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.minValue != value;
                bodyShapeSettings.noiseLayers[numCapaOBioma].noiseSettings.minValue = value;
                break;
            case "Masa":
                valueChanged = false;
                celestialBody.mass = value;
                break;
            case "PosX":
                pos = body.transform.position;
                valueChanged = false;
                pos.x = value;
                body.transform.position = pos;
                break;
            case "PosY":
                pos = body.transform.position;
                valueChanged = false;
                pos.y = value;
                body.transform.position = pos;
                break;
            case "PosZ":
                pos = body.transform.position;
                valueChanged = false;
                pos.z = value;
                body.transform.position = pos;
                break;
            case "VelX":
                valueChanged = false;
                celestialBody.velocity.x = value;
                break;
            case "VelY":
                valueChanged = false;
                celestialBody.velocity.y = value;
                break;
            case "VelZ":
                valueChanged = false;
                celestialBody.velocity.z = value;
                break;
            case "VelRotX":
                valueChanged = false;
                celestialBody.angularVelocity.x = value;
                break;
            case "VelRotY":
                valueChanged = false;
                celestialBody.angularVelocity.y = value;
                break;
            case "VelRotZ":
                valueChanged = false;
                celestialBody.angularVelocity.z = value;
                break;
            case "Radio":
                valueChanged = bodyShapeSettings.planetRadius != value;
                bodyShapeSettings.planetRadius = value;
                break;
            case "Tinte":
                valueChanged = bodyColourSettings.biomeColourSettings.biomes[numCapaOBioma].tintPercent != value;
                bodyColourSettings.biomeColourSettings.biomes[numCapaOBioma].tintPercent = value;
                break;
        }

        Planet planet = body.GetComponent<Planet>();

        if (planet != null && valueChanged) {
            planet.GeneratePlanet();
        }
    }

    public void UpdateBodyColorTint(Color tint, int biomeNum) {
        bodyColourSettings.biomeColourSettings.biomes[biomeNum].tint = tint;

        Planet planet = body.GetComponent<Planet>();

        if (planet != null) {
            planet.GeneratePlanet();
        }
    }

    public void UpdateBodyGradient(Gradient gradient, int biomeNum) {
        bodyColourSettings.biomeColourSettings.biomes[biomeNum].gradient = gradient;

        Planet planet = body.GetComponent<Planet>();

        if (planet != null) {
            planet.GeneratePlanet();
        }
    }

    public void DestroyRuido(int ruidoId) {
        bodyShapeSettings.noiseLayers = 
            EliminarIndice<ShapeSettings.NoiseLayer>(
                bodyShapeSettings.noiseLayers, ruidoId);
        
        Planet planet = body.GetComponent<Planet>();

        if (planet != null) {
            planet.GeneratePlanet();
        }
    }

    public void DestroyBiome(int biomeId) {
        bodyColourSettings.biomeColourSettings.biomes = 
            EliminarIndice<ColourSettings.BiomeColourSettings.Biome>(
                bodyColourSettings.biomeColourSettings.biomes, biomeId);

        Planet planet = body.GetComponent<Planet>();

        if (planet != null) {
            planet.GeneratePlanet();
        }    
    }

    private static T[] EliminarIndice<T>(T[] array, int indice)
    {
        if (array == null || array.Length == 0)
            return array;

        if (indice < 0 || indice >= array.Length)
            return array;

        T[] nuevo = new T[array.Length - 1];

        int j = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (i == indice) continue;

            nuevo[j] = array[i];
            j++;
        }

        return nuevo;
    }
}