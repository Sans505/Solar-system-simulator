using UnityEngine;
using UnityEngine.UI;

public class SelectorColorMenu : MonoBehaviour
{
    [SerializeField] private EditMenuManager editMenuManager;
    // Aquí guardaremos el color o gradiente que el usuario diseñe
    [Header("Datos Guardados")]
    public Color colorElegido = Color.white;
    public Gradient gradienteElegido = new Gradient();

    // Aquí arrastraremos los botones de TU interfaz (los que están en tus pestañas)
    [Header("Botones de tus Pestañas")]
    public Button botonParaColorUnico;
    public Button botonParaGradiente;

    private void Start()
    {
        // Le decimos a tus botones que, al hacerles clic, ejecuten las funciones de abajo
        if (botonParaColorUnico != null)
            botonParaColorUnico.onClick.AddListener(AbrirVentanaColor);

        if (botonParaGradiente != null)
            botonParaGradiente.onClick.AddListener(AbrirVentanaGradiente);
    }

    // --- FUNCIÓN PARA EL COLOR ÚNICO ---
    private void AbrirVentanaColor()
    {
        // Esto abre el Pop-up del asset. 
        // Le pasamos el color actual, un título, y la función que se ejecuta al mover el color
        ColorPicker.Create(colorElegido, "Selecciona un Color", AlCambiarColor, null, true);
    }

    private void AlCambiarColor(Color nuevoColor)
    {
        // Cada vez que el usuario mueva el ratón por el selector, el color se guarda aquí
        colorElegido = nuevoColor;
    }

    // --- FUNCIÓN PARA EL GRADIENTE ---
    private void AbrirVentanaGradiente()
    {
        // Esto abre el Pop-up del gradiente
        GradientPicker.Create(gradienteElegido, "Diseña tu Gradiente", AlCambiarGradiente, null);
    }

    private void AlCambiarGradiente(Gradient nuevoGradiente)
    {
        // Cada vez que el usuario toque un punto del degradado, se guarda aquí
        gradienteElegido = nuevoGradiente;
    }
}