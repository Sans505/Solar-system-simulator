using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestorDatosMenu : MonoBehaviour
{
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
            inputMasa.onEndEdit.AddListener(v => Debug.Log($"Cambio -> Masa: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputPosX != null)
            inputPosX.onEndEdit.AddListener(v => Debug.Log($"Cambio -> PosX: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputPosY != null)
            inputPosY.onEndEdit.AddListener(v => Debug.Log($"Cambio -> PosY: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputPosZ != null)
            inputPosZ.onEndEdit.AddListener(v => Debug.Log($"Cambio -> PosZ: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelocidadX != null)
            inputVelocidadX.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelX: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelocidadY != null)
            inputVelocidadY.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelY: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelocidadZ != null)
            inputVelocidadZ.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelZ: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelRotX != null)
            inputVelRotX.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelRotX: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelRotY != null)
            inputVelRotY.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelRotY: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputVelRotZ != null)
            inputVelRotZ.onEndEdit.AddListener(v => Debug.Log($"Cambio -> VelRotZ: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
        if (inputRadio != null)
            inputRadio.onEndEdit.AddListener(v => Debug.Log($"Cambio -> Radio: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
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
            int capaNum = i + 1;

            RegistrarSliderCapas(capa, "SliderFuerza", capaNum, "Fuerza");
            RegistrarSliderCapas(capa, "SliderCapas", capaNum, "Capas");
            RegistrarSliderCapas(capa, "SliderRugosidadBase", capaNum, "Rug.Base");
            RegistrarSliderCapas(capa, "SliderRugosidad", capaNum, "Rugosidad");
            RegistrarSliderCapas(capa, "SliderPersistencia", capaNum, "Persistencia");

            RegistrarInputCapas(capa, "InputCX", capaNum, "CentroX");
            RegistrarInputCapas(capa, "InputCY", capaNum, "CentroY");
            RegistrarInputCapas(capa, "InputCZ", capaNum, "CentroZ");
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
        slider.onValueChanged.AddListener(v => Debug.Log($"Cambio -> {etiqueta} capa {capaNum}: {v.ToString(valorFormat)}"));
    }

    private void RegistrarInputCapas(Transform padre, string nombre, int capaNum, string etiqueta)
    {
        Transform inputObj = BuscarHijoRecursivo(padre, nombre);
        if (inputObj == null) return;

        TMP_InputField input = inputObj.GetComponent<TMP_InputField>();
        if (input == null) return;

        input.onEndEdit.RemoveAllListeners();
        input.onEndEdit.AddListener(v => Debug.Log($"Cambio -> {etiqueta} capa {capaNum}: {(string.IsNullOrEmpty(v) ? "Vacío" : v)}"));
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
}