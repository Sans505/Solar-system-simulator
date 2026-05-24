using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestorDatosMenu : MonoBehaviour
{
    [SerializeField] EditMenuManager editMenuManager;
    [Header("Inputs del Panel Física")]
    public TMP_InputField inputMasa;

    public TMP_InputField inputPosX;
    public TMP_InputField inputPosY;
    public TMP_InputField inputPosZ;

    public TMP_InputField inputVelocidadX;
    public TMP_InputField inputVelocidadY;
    public TMP_InputField inputVelocidadZ;

    public TMP_InputField inputVelRotX;
    public TMP_InputField inputVelRotY;
    public TMP_InputField inputVelRotZ;

    [Header("Inputs del Panel Forma")]
    public TMP_InputField inputRadio;

    [Header("Contenedores Dinámicos (Ruido)")]
    public Transform contentRuido;

    private ListaRuidos listaRuidos;
    private ListaBiomas listaBiomas;
    private readonly HashSet<int> controlesRegistrados = new HashSet<int>();

    private void Start()
    {
        VincularListaRuidos();
        RegistrarListenersFijos();
        RegistrarListenersRuido();
    }

    private void OnEnable()
    {
        VincularListaRuidos();
    }

    private void OnDisable()
    {
        DesvincularListaRuidos();
    }

    private void OnDestroy()
    {
        DesvincularListaRuidos();
    }

    private void VincularListaRuidos()
    {
        DesvincularListaRuidos();

        listaRuidos = FindObjectOfType<ListaRuidos>(true);
        listaBiomas = FindObjectOfType<ListaBiomas>(true);

        if (listaRuidos != null)
        {
            listaRuidos.OnRuidosCambiados += ManejarCambioEnCapas;
        }
    }

    private void DesvincularListaRuidos()
    {
        if (listaRuidos != null)
        {
            listaRuidos.OnRuidosCambiados -= ManejarCambioEnCapas;
        }
    }

    private void ManejarCambioEnCapas()
    {
        RegistrarListenersRuido();
    }

    private void RegistrarListenersFijos()
    {
        if (inputMasa != null)
            inputMasa.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("Masa", float.Parse(v)));
        if (inputPosX != null)
            inputPosX.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("PosX", float.Parse(v)));
        if (inputPosY != null)
            inputPosY.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("PosY", float.Parse(v)));
        if (inputPosZ != null)
            inputPosZ.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("PosZ", float.Parse(v)));
        if (inputVelocidadX != null)
            inputVelocidadX.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelX", float.Parse(v)));
        if (inputVelocidadY != null)
            inputVelocidadY.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelY", float.Parse(v)));
        if (inputVelocidadZ != null)
            inputVelocidadZ.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelZ", float.Parse(v)));
        if (inputVelRotX != null)
            inputVelRotX.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelRotX", float.Parse(v)));
        if (inputVelRotY != null)
            inputVelRotY.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelRotY", float.Parse(v)));
        if (inputVelRotZ != null)
            inputVelRotZ.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("VelRotZ", float.Parse(v)));
        if (inputRadio != null)
            inputRadio.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData("Radio", float.Parse(v)));
    }

    private void RegistrarListenersRuido()
    {
        if (contentRuido == null)
        {
            return;
        }

        for (int i = 0; i < contentRuido.childCount; i++)
        {
            Transform capa = contentRuido.GetChild(i);


            RegistrarSliderCapas(capa, "SliderFuerza", i, "Fuerza");
            RegistrarSliderCapas(capa, "SliderCapas", i, "Capas");
            RegistrarSliderCapas(capa, "SliderRugosidadBase", i, "Rug.Base");
            RegistrarSliderCapas(capa, "SliderRugosidad", i, "Rugosidad");
            RegistrarSliderCapas(capa, "SliderPersistencia", i, "Persistencia");

            RegistrarInputCapas(capa, "InputCX", i, "CentroX");
            RegistrarInputCapas(capa, "InputCY", i, "CentroY");
            RegistrarInputCapas(capa, "InputCZ", i, "CentroZ");

            RegistrarSliderCapas(capa, "SliderVMin", i, "Valor.Min");
        }
    }

    private void RegistrarSliderCapas(Transform padre, string nombre, int capaNum, string etiqueta)
    {
        Transform sliderObj = BuscarHijoRecursivo(padre, nombre);
        if (sliderObj == null) return;

        Slider slider = sliderObj.GetComponent<Slider>();
        if (slider == null) return;

        slider.onValueChanged.RemoveAllListeners();
        string valorFormat = etiqueta == "Capas" ? "F0" : "F2";
        slider.onValueChanged.AddListener(v => editMenuManager.UpdateBodyNumericData(etiqueta, v, capaNum));
    }

    private void RegistrarSliderBioma(Transform padre, string nombre, int biomaNum, string etiqueta)
    {
        Transform sliderObj = BuscarHijoRecursivo(padre, nombre);
        if (sliderObj == null) return;

        Slider slider = sliderObj.GetComponent<Slider>();
        if (slider == null) return;

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(v => 
            editMenuManager.UpdateBodyNumericData(etiqueta, v, biomaNum)
        );
    }

    private void RegistrarInputCapas(Transform padre, string nombre, int capaNum, string etiqueta)
    {
        Transform inputObj = BuscarHijoRecursivo(padre, nombre);
        if (inputObj == null) return;

        TMP_InputField input = inputObj.GetComponent<TMP_InputField>();
        if (input == null) return;

        input.onEndEdit.RemoveAllListeners();
        input.onEndEdit.AddListener(v => editMenuManager.UpdateBodyNumericData(etiqueta, float.Parse(v), capaNum));
    }

    private string ObtenerTexto(TMP_InputField input)
    {
        if (input == null || string.IsNullOrEmpty(input.text))
        {
            return "Vacío";
        }

        return input.text;
    }

    private Transform BuscarHijoRecursivo(Transform padre, string nombreABuscar)
    {
        if (padre == null)
        {
            return null;
        }

        Transform resultado = padre.Find(nombreABuscar);
        if (resultado != null)
        {
            return resultado;
        }

        foreach (Transform hijo in padre)
        {
            resultado = BuscarHijoRecursivo(hijo, nombreABuscar);
            if (resultado != null)
            {
                return resultado;
            }
        }

        return null;
    }

    public void RegistrarValoresFijos(CelestialBody celestialBody, SharedSettings sharedSettings) {
        if (inputMasa != null)
            inputMasa.text = celestialBody.mass.ToString();
        if (inputPosX != null)
            inputPosX.text = celestialBody.transform.position.x.ToString();
        if (inputPosY != null)
            inputPosY.text = celestialBody.transform.position.y.ToString();
        if (inputPosZ != null)
            inputPosZ.text = celestialBody.transform.position.z.ToString();
        if (inputVelocidadX != null)
            inputVelocidadX.text = celestialBody.velocity.x.ToString();
        if (inputVelocidadY != null)
            inputVelocidadY.text = celestialBody.velocity.y.ToString();
        if (inputVelocidadZ != null)
            inputVelocidadZ.text = celestialBody.velocity.z.ToString();
        if (inputVelRotX != null)
            inputVelRotX.text = celestialBody.angularVelocity.x.ToString();
        if (inputVelRotY != null)
            inputVelRotY.text = celestialBody.angularVelocity.y.ToString();
        if (inputVelRotZ != null)
            inputVelRotZ.text = celestialBody.angularVelocity.z.ToString();
        if (inputRadio != null)
            inputRadio.text = sharedSettings.getRadius().ToString();
    }

    public void RegistrarValoresRuido(ShapeSettings settings) 
    {
        if (contentRuido == null)
        {
            return;
        }

        listaRuidos.ResetRuidos();

        ShapeSettings.NoiseLayer[] noiseLayers = settings.noiseLayers;

        for (int i = 1; i < noiseLayers.Length; i++) {
            listaRuidos.AñadirNuevoRuido();
        }


        for (int i = 0; i < noiseLayers.Length; i++)
        {
            Transform capa = contentRuido.GetChild(i);

            RegistrarValorCapa(capa, noiseLayers[i].noiseSettings);
        }
    }

    private void RegistrarValorCapa(Transform capa, NoiseSettings settings) {

        RegistrarValorSlider(capa, "SliderFuerza", settings.strength);
        RegistrarValorSlider(capa, "SliderCapas", settings.numLayers);
        RegistrarValorSlider(capa, "SliderRugosidadBase", settings.baseRoughness);
        RegistrarValorSlider(capa, "SliderRugosidad", settings.roughness);
        RegistrarValorSlider(capa, "SliderPersistencia", settings.persistence);

        RegistrarValorInput(capa, "InputCX", settings.centre.x);
        RegistrarValorInput(capa, "InputCY", settings.centre.y);
        RegistrarValorInput(capa, "InputCZ", settings.centre.z);

        RegistrarValorSlider(capa, "SliderVMin", settings.minValue);
    }

    private void RegistrarValorSlider(Transform padre, string nombre, float valor) {
        Transform sliderObj = BuscarHijoRecursivo(padre, nombre);
        if (sliderObj == null) return;

        Slider slider = sliderObj.GetComponent<Slider>();
        if (slider == null) return;

        slider.value = valor;
    }

    private void RegistrarValorInput(Transform padre, string nombre, float valor) {
        Transform inputObj = BuscarHijoRecursivo(padre, nombre);
        if (inputObj == null) return;

        TMP_InputField input = inputObj.GetComponent<TMP_InputField>();
        if (input == null) return;

        input.text = valor.ToString();
    }

    public void RegistrarValoresBiomas(ColourSettings settings) 
    {
        Transform contentBiomas = listaBiomas.contenedorContent;
        if (contentBiomas == null)
        {
            return;
        }

        listaBiomas.ResetBiomas();

        ColourSettings.BiomeColourSettings.Biome[] biomes = settings.biomeColourSettings.biomes;

        for (int i = 1; i < biomes.Length; i++) {
            listaBiomas.AñadirNuevoBioma();
        }


        for (int i = 0; i < biomes.Length; i++)
        {
            Transform bioma = contentBiomas.GetChild(i);
            ConectorColorVisual colorVisualConector = bioma.GetComponent<ConectorColorVisual>();

            colorVisualConector.EstablecerColorYGradiente(biomes[i].tint, biomes[i].gradient);

            RegistrarValorSlider(bioma, "SliderTinte", biomes[i].tintPercent);
            RegistrarSliderBioma(bioma, "SliderTinte", i, "Tinte");
        }
    }
}