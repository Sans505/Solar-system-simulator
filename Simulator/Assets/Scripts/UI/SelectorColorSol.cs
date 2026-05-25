using UnityEngine;
using UnityEngine.UI;

public class SelectorColorSol : MonoBehaviour
{
    [SerializeField] private EditMenuManager editMenuManager;
    [Header("Datos Guardados")]
    public Color colorSol = Color.white; // Aquí se guarda el color del sol

    [Header("Conexiones de la Interfaz")]
    public Button botonAbrirColor;
    public Image vistaPreviaColor; // Preview color

    private void Start()
    {
        if (botonAbrirColor != null)
        {
            botonAbrirColor.onClick.AddListener(AbrirVentanaColor);
        }

        // Color por defecto al empezar
        if (vistaPreviaColor != null) 
        {
            vistaPreviaColor.color = colorSol;
        }
    }

    private void AbrirVentanaColor()
    {
        ColorPicker.Create(colorSol, "Color del Sol", AlCambiarColor, AlSeleccionarColor, true);
    }

    private void AlCambiarColor(Color nuevoColor)
    {
        // Guardar nuevo color
        colorSol = nuevoColor;
        
        // Pintar vista previa
        if (vistaPreviaColor != null) 
        {
            vistaPreviaColor.color = colorSol;
        }
    }

    private void AlSeleccionarColor(Color nuevoColor) {
        editMenuManager.UpdateSunColor(colorSol);
    }

    public void EstablecerColor(Color color) {
        colorSol = color;

    }
}