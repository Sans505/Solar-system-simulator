using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MostrarVelocidad : MonoBehaviour
{
    // objeto textMeshPro
    private TextMeshProUGUI campoVelocidad;

    
    void Awake()
    {
        campoVelocidad = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Actualizar el texto con la velocidad actual
        campoVelocidad.text = "Velocidad: " + Time.timeScale.ToString("0.00") + "x";
    }
}