using UnityEngine;
using UnityEngine.UI;

public class ConectorColorVisual : MonoBehaviour
{
    private EditMenuManager editMenuManager;

    [Header("Tus dos Imágenes (Arrastra aquí)")]
    public Image imagenColorUnico;      // La Image normal para el color único
    public RawImage imagenGradiente;   // La RawImage rectangular para el gradiente

    [Header("Tus dos Botones (Arrastra aquí)")]
    public Button botonAbrirColor;
    public Button botonAbrirGradiente;

    // Aquí guardaremos los datos que elija el usuario
    [HideInInspector] public Color colorGuardado = Color.white;
    [HideInInspector] public Gradient gradienteGuardado = new Gradient();

    private Texture2D texturaTexturaGradiente;

    private void Start()
    {
        editMenuManager = GetComponentInParent<EditMenuManager>();
        // Decimos a tus botones que abran las ventanas al hacer clic
        if (botonAbrirColor != null)
            botonAbrirColor.onClick.AddListener(AbrirPopUpColor);

        if (botonAbrirGradiente != null)
            botonAbrirGradiente.onClick.AddListener(AbrirPopUpGradiente);

        // Pintamos los colores que vienen por defecto al empezar
        PintarColorEnUI();
        PintarGradienteEnUI();
    }

    // --- LÓGICA DEL COLOR ÚNICO ---
    private void AbrirPopUpColor()
    {
        ColorPicker.Create(colorGuardado, "Elige un Color", AlCambiarColor, AlSeleccionarColor, true);
    }

    private void AlCambiarColor(Color nuevoColor)
    {
        colorGuardado = nuevoColor;
        PintarColorEnUI();
    }

    private void AlSeleccionarColor(Color nuevoColor) {
        editMenuManager.UpdateBodyColorTint(colorGuardado, transform.GetSiblingIndex());
    }

    private void PintarColorEnUI()
    {
        if (imagenColorUnico != null)
        {
            imagenColorUnico.color = colorGuardado; // Cambia el color del cuadradito
        }
    }

    // --- LÓGICA DEL GRADIENTE ---
    private void AbrirPopUpGradiente()
    {
        GradientPicker.Create(gradienteGuardado, "Diseña tu Gradiente", AlCambiarGradiente, AlSeleccionarGradiente);
    }

    private void AlCambiarGradiente(Gradient nuevoGradiente)
    {
        // Clonamos el gradiente para que Unity no haga cosas raras con la memoria
        gradienteGuardado = new Gradient();
        gradienteGuardado.SetKeys(nuevoGradiente.colorKeys, nuevoGradiente.alphaKeys);
        
        PintarGradienteEnUI();
    }

    private void AlSeleccionarGradiente(Gradient nuevoGradiente) {
        editMenuManager.UpdateBodyGradient(gradienteGuardado, transform.GetSiblingIndex());
    }

    private void PintarGradienteEnUI()
    {
        if (imagenGradiente == null || gradienteGuardado == null) return;

        // Generamos una textura horizontal para pintar el degradado en la RawImage
        if (texturaTexturaGradiente == null)
        {
            texturaTexturaGradiente = new Texture2D(100, 1, TextureFormat.RGBA32, false);
            texturaTexturaGradiente.wrapMode = TextureWrapMode.Clamp;
            imagenGradiente.texture = texturaTexturaGradiente;
        }

        for (int x = 0; x < 100; x++)
        {
            float t = x / 99f;
            Color colorPixel = gradienteGuardado.Evaluate(t);
            texturaTexturaGradiente.SetPixel(x, 0, colorPixel);
        }

        texturaTexturaGradiente.Apply(); // Aplica los colores a la pantalla
    }

    public void EstablecerColorYGradiente(Color color, Gradient gradient) {
        colorGuardado = color;
        gradienteGuardado = new Gradient();
        gradienteGuardado.SetKeys(gradient.colorKeys, gradient.alphaKeys);

        PintarColorEnUI();
        PintarGradienteEnUI();
    }
}