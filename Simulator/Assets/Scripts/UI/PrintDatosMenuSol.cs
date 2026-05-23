using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class PrintDatosMenuSol : MonoBehaviour
{
    [Header("Contenedor Principal (Arrastra aquí PanelSol)")]
    public Transform panelSol;

    private void Start()
    {
        if (panelSol == null)
        {
            Debug.LogError("No has asignado el PanelSol en el PrintDatosMenuSol.");
            return;
        }

        // --- CONECTAMOS LOS INPUTS (Textos/Números escritos) ---
        ConectarInput("InputMasaSol");
        ConectarInput("InputCXSol");
        ConectarInput("InputCYSol");
        ConectarInput("InputCZSol");
        ConectarInput("InputVXSol");
        ConectarInput("InputVYSol");
        ConectarInput("InputVZSol");
        ConectarInput("InputVRXSol");
        ConectarInput("InputVRYSol");
        ConectarInput("InputVRZSol");
        ConectarInput("InputRadioSol");

        // --- CONECTAMOS LOS SLIDERS (Barras deslizantes) ---
        ConectarSlider("SliderNoiseScale");
        ConectarSlider("SliderNoisePower");
        ConectarSlider("SliderShaderIntensity");
        ConectarSlider("SliderTwirlStrength");
        ConectarSlider("SliderDistortionScale");
        ConectarSlider("SliderPanSpeed");
    }

    // --- FUNCIONES QUE BUSCAN Y ENCHUFAN EL CABLE A LA VEZ ---
    
    private void ConectarInput(string nombreObjeto)
    {
        Transform obj = BuscarHijoRecursivo(panelSol, nombreObjeto);
        if (obj != null)
        {
            TMP_InputField input = obj.GetComponent<TMP_InputField>();
            
            // Cuando cambie, llama a la función pasándole SU NOMBRE y SU NUEVO VALOR
            input.onValueChanged.AddListener((nuevoValor) => AplicarCambioStr(nombreObjeto, nuevoValor));
        }
        else
        {
            Debug.LogWarning($"[PrintDatosSol] No se encontró el Input: '{nombreObjeto}'");
        }
    }

    private void ConectarSlider(string nombreObjeto)
    {
        Transform obj = BuscarHijoRecursivo(panelSol, nombreObjeto);
        if (obj != null)
        {
            Slider slider = obj.GetComponent<Slider>();
            
            // Cuando cambie, llama a la función pasándole SU NOMBRE y SU NUEVO VALOR DECIMAL
            slider.onValueChanged.AddListener((nuevoValor) => AplicarCambioFloat(nombreObjeto, nuevoValor));
        }
        else
        {
            Debug.LogWarning($"[PrintDatosSol] No se encontró el Slider: '{nombreObjeto}'");
        }
    }


    // Esta función recibe los datos de los Inputs (Masa, Coordenadas, etc.)
    private void AplicarCambioStr(string nombrePropiedad, string valorTexto)
    {
        // Si el usuario borra todo y lo deja en blanco, lo tratamos como "0" o "Vacío"
        if (string.IsNullOrEmpty(valorTexto)) valorTexto = "Vacío";

        Debug.Log($"<b>[ACTUALIZACIÓN INPUT]</b> {nombrePropiedad} ahora vale: <b>{valorTexto}</b>");
        
        //Aqui entiendo que es donde metes lo de cambiar el valor

    }

    // Esta función recibe los datos de los Sliders (Ruido, Shaders, etc.)
    private void AplicarCambioFloat(string nombrePropiedad, float valorNumerico)
    {
        // Imprimimos en consola (usamos F2 para que la consola no se sature de decimales largos)
        Debug.Log($"<color=#ffaa00><b>[ACTUALIZACIÓN SLIDER]</b></color> {nombrePropiedad} ahora vale: <b>{valorNumerico.ToString("F2")}</b>");

        //Aqui entiendo que es donde metes lo de cambiar el valor

    }

    // =========================================================================

    // Función auxiliar para buscar en toda la jerarquía
    private Transform BuscarHijoRecursivo(Transform padre, string nombreABuscar)
    {
        Transform resultado = padre.Find(nombreABuscar);
        if (resultado != null) return resultado;

        foreach (Transform hijo in padre)
        {
            resultado = BuscarHijoRecursivo(hijo, nombreABuscar);
            if (resultado != null) return resultado; 
        }
        return null;
    }
}