using UnityEngine;
using UnityEngine.UI;

public class EditMenuManager : MonoBehaviour
{
    [SerializeField] private ControladorHeader headerController;
    [SerializeField] private GestorDatosMenu menuDataManager;
    [SerializeField] private GestorDatosMenuSol sunMenuDataManager;

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
            headerController.CambiarPaneles(true);
            DisplayPlanetData();
        } else if (sun != null) {
            bodySunSettings = sun.sunSettings;
            headerController.CambiarPaneles(false);
            DisplaySunData();
        } else {
            NotificationManager.instance.ShowNotification("Error: Información del astro perdida", NotificationType.Error);
            return;
        }
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

    private void DisplayPlanetData() {

        menuDataManager.RegistrarValoresFijos(celestialBody, body.GetComponent<SharedSettings>());
        menuDataManager.RegistrarValoresRuido(bodyShapeSettings);
        menuDataManager.RegistrarValoresBiomas(bodyColourSettings);
    }

    private void DisplaySunData() {
        sunMenuDataManager.RegistrarValoresFijos(celestialBody, body.GetComponent<SharedSettings>());
        sunMenuDataManager.RegistrarValoresSol(bodySunSettings);
    }

    public void UpdateBodyNumericData(string dataTag, float value, int numCapaOBioma = -1) {

        Vector3 pos;
        bool valueChanged = false;

        switch(dataTag) {

            // ------------------- DATOS DE PLANETAS ----------------

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
            case "InputMasaSol":
            case "Masa":
                valueChanged = false;
                celestialBody.mass = value;
                break;
            case "InputCXSol":
            case "PosX":
                pos = body.transform.position;
                valueChanged = false;
                pos.x = value;
                body.transform.position = pos;
                break;
            case "InputCYSol":
            case "PosY":
                pos = body.transform.position;
                valueChanged = false;
                pos.y = value;
                body.transform.position = pos;
                break;
            case "InputCZSol":
            case "PosZ":
                pos = body.transform.position;
                valueChanged = false;
                pos.z = value;
                body.transform.position = pos;
                break;
            case "InputVXSol":
            case "VelX":
                valueChanged = false;
                celestialBody.velocity.x = value;
                break;
            case "VelY":
            case "InputVYSol":
                valueChanged = false;
                celestialBody.velocity.y = value;
                break;
            case "VelZ":
            case "InputVZSol":
                valueChanged = false;
                celestialBody.velocity.z = value;
                break;
            case "InputVRXSol":
            case "VelRotX":
                valueChanged = false;
                celestialBody.angularVelocity.x = value;
                break;
            case "InputVRYSol":
            case "VelRotY":
                valueChanged = false;
                celestialBody.angularVelocity.y = value;
                break;
            case "InputVRZSol":
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
            
            // ------------------ DATOS DE SOL -----------------------


            case "InputRadioSol":

                valueChanged = bodySunSettings.sunRadius != value;
                bodySunSettings.sunRadius = value;
                break;
            case "SliderNoiseScale":
                valueChanged = bodySunSettings.NoiseScale != value;
                bodySunSettings.NoiseScale = value;
                break;

            case "SliderNoisePower":
                valueChanged = bodySunSettings.NoisePower != value;
                bodySunSettings.NoisePower = value;
                break;

            case "SliderShaderIntensity":
                valueChanged = bodySunSettings.shaderIntensity != value;
                bodySunSettings.shaderIntensity = value;
                break;

            case "SliderTwirlStrength":
                valueChanged = bodySunSettings.TwirlStrength != value;
                bodySunSettings.TwirlStrength = value;
                break;

            case "SliderDistortionScale":
                valueChanged = bodySunSettings.DistorsionScale != value;
                bodySunSettings.DistorsionScale = value;
                break;

            case "SliderPanSpeed":
                Vector2 panSpeed = new Vector2(value, value);
                valueChanged = bodySunSettings.PanSpeed != panSpeed;
                bodySunSettings.PanSpeed = panSpeed;
                break;
        }

        if (valueChanged) regenerateBody();
    }

    public void UpdateBodyColorTint(Color tint, int biomeNum) {
        bodyColourSettings.biomeColourSettings.biomes[biomeNum].tint = tint;

        regenerateBody();
    }

    public void UpdateSunColor(Color color) {
        bodySunSettings.BaseColor = color;

        regenerateBody();
    }

    public void UpdateBodyGradient(Gradient gradient, int biomeNum) {
        bodyColourSettings.biomeColourSettings.biomes[biomeNum].gradient = gradient;

        regenerateBody();
    }

    public void AddRuido() {
        ShapeSettings.NoiseLayer newNoiseLayer = new ShapeSettings.NoiseLayer();
        newNoiseLayer.noiseSettings = new NoiseSettings();
        bodyShapeSettings.noiseLayers = AñadirElementoAlFinal(bodyShapeSettings.noiseLayers, newNoiseLayer);

        regenerateBody();
    }

    public void AddBiome() {
        ColourSettings.BiomeColourSettings.Biome newBiome = new ColourSettings.BiomeColourSettings.Biome();
        newBiome.tint = Color.white;
        newBiome.gradient = new Gradient();
        bodyColourSettings.biomeColourSettings.biomes = AñadirElementoAlFinal(bodyColourSettings.biomeColourSettings.biomes, newBiome);

        menuDataManager.ActualizarLisenersSlidersBiomas();
        ReorganizeBiomesStartHeight();
        regenerateBody();
    }


    public void DestroyRuido(int ruidoId) {
        bodyShapeSettings.noiseLayers = 
            EliminarIndice<ShapeSettings.NoiseLayer>(
                bodyShapeSettings.noiseLayers, ruidoId);

        menuDataManager.VincularListaRuidos();
        
        regenerateBody();
    }

    public void DestroyBiome(int biomeId) {
        bodyColourSettings.biomeColourSettings.biomes = 
            EliminarIndice<ColourSettings.BiomeColourSettings.Biome>(
                bodyColourSettings.biomeColourSettings.biomes, biomeId);
        
        menuDataManager.ActualizarLisenersSlidersBiomas();
        ReorganizeBiomesStartHeight();

        regenerateBody();
    }

    private void ReorganizeBiomesStartHeight() {
        ColourSettings.BiomeColourSettings.Biome[] biomes = bodyColourSettings.biomeColourSettings.biomes;

        float heightInterval = 1f / biomes.Length;

        for (int i = 0; i < biomes.Length; i++) {
            biomes[i].startHeight = heightInterval * i;
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

    private static T[] AñadirElementoAlFinal<T>(T[] array, T elemento)
    {
        if (array == null || array.Length == 0)
            return array;

        T[] nuevo = new T[array.Length + 1];

        for (int i = 0; i < array.Length; i++)
        {
            nuevo[i] = array[i];
        }

        nuevo[array.Length] = elemento;

        return nuevo;
    }

    private void regenerateBody() {

        if (headerController.editandoPlaneta) {
            Planet planet = body.GetComponent<Planet>();

            if (planet != null) {
                planet.GeneratePlanet();
            }  
        } else {
            Sun sun = body.GetComponent<Sun>();
            if (sun != null) {
                sun.GenerateSun();
            }
        } 
    }
}