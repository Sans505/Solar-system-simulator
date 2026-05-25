using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;

public class GestorDatosMenuSol : MonoBehaviour
{   
    [SerializeField] EditMenuManager editMenuManager;
    [SerializeField] SelectorColorSol selectorColorSol;
    [Header("Contenedor Principal (Arrastra aquí PanelSol)")]
    public Transform panelSol;

    private Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>();
    private Dictionary<string, Slider> sliders = new Dictionary<string, Slider>();

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

            inputs[nombreObjeto] = input; // 👈 GUARDAMOS

            input.onEndEdit.AddListener(v => 
                editMenuManager.UpdateBodyNumericData(nombreObjeto, float.Parse(v)));
        }
    }

    private void ConectarSlider(string nombreObjeto)
    {
        Transform obj = BuscarHijoRecursivo(panelSol, nombreObjeto);
        if (obj != null)
        {
            Slider slider = obj.GetComponent<Slider>();

            sliders[nombreObjeto] = slider; // 👈 GUARDAMOS

            SliderReleaseListener listener = obj.gameObject.AddComponent<SliderReleaseListener>();
            listener.slider = slider;

            listener.onRelease.AddListener(v =>
            {
                editMenuManager.UpdateBodyNumericData(nombreObjeto, v);
            });
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

    public void RegistrarValoresFijos(CelestialBody celestialBody, SharedSettings sharedSettings) {
        if (inputs.ContainsKey("InputMasaSol"))
        inputs["InputMasaSol"].SetTextWithoutNotify(celestialBody.mass.ToString());

        if (inputs.ContainsKey("InputCXSol"))
            inputs["InputCXSol"].SetTextWithoutNotify(celestialBody.transform.position.x.ToString());

        if (inputs.ContainsKey("InputCYSol"))
            inputs["InputCYSol"].SetTextWithoutNotify(celestialBody.transform.position.y.ToString());

        if (inputs.ContainsKey("InputCZSol"))
            inputs["InputCZSol"].SetTextWithoutNotify(celestialBody.transform.position.z.ToString());

        if (inputs.ContainsKey("InputVXSol"))
            inputs["InputVXSol"].SetTextWithoutNotify(celestialBody.velocity.x.ToString());

        if (inputs.ContainsKey("InputVYSol"))
            inputs["InputVYSol"].SetTextWithoutNotify(celestialBody.velocity.y.ToString());

        if (inputs.ContainsKey("InputVZSol"))
            inputs["InputVZSol"].SetTextWithoutNotify(celestialBody.velocity.z.ToString());

        if (inputs.ContainsKey("InputVRXSol"))
            inputs["InputVRXSol"].SetTextWithoutNotify(celestialBody.angularVelocity.x.ToString());

        if (inputs.ContainsKey("InputVRYSol"))
            inputs["InputVRYSol"].SetTextWithoutNotify(celestialBody.angularVelocity.y.ToString());

        if (inputs.ContainsKey("InputVRZSol"))
            inputs["InputVRZSol"].SetTextWithoutNotify(celestialBody.angularVelocity.z.ToString());
        
        if (inputs.ContainsKey("InputRadioSol"))
            inputs["InputRadioSol"].SetTextWithoutNotify(sharedSettings.getRadius().ToString());

    }

    public void RegistrarValoresSol(SunSettings settings) {
        if (sliders.ContainsKey("SliderNoiseScale"))
        sliders["SliderNoiseScale"].SetValueWithoutNotify(settings.NoiseScale);

        if (sliders.ContainsKey("SliderNoisePower"))
            sliders["SliderNoisePower"].SetValueWithoutNotify(settings.NoisePower);

        if (sliders.ContainsKey("SliderShaderIntensity"))
            sliders["SliderShaderIntensity"].SetValueWithoutNotify(settings.shaderIntensity);

        if (sliders.ContainsKey("SliderTwirlStrength"))
            sliders["SliderTwirlStrength"].SetValueWithoutNotify(settings.TwirlStrength);

        if (sliders.ContainsKey("SliderDistortionScale"))
            sliders["SliderDistortionScale"].SetValueWithoutNotify(settings.DistorsionScale);

        if (sliders.ContainsKey("SliderPanSpeed"))
            sliders["SliderPanSpeed"].SetValueWithoutNotify(settings.PanSpeed.x);
        selectorColorSol.EstablecerColor(settings.BaseColor);
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