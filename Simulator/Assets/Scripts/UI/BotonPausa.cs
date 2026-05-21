using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonPausa : MonoBehaviour
{
    private bool estaPausado = false;
    private Button miBoton;
    private TextMeshProUGUI textoBoton;

    void Awake()
    {
        miBoton = GetComponent<Button>();
        textoBoton = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AlternarPausa()
    {
        estaPausado = !estaPausado;

        if (estaPausado)
        {
            Time.timeScale = 0f;
            if (textoBoton != null) textoBoton.text = "REANUDAR";
        }
        else
        {
            Time.timeScale = 1f;
            if (textoBoton != null) textoBoton.text = "PAUSA";
        }


        // Deselccionar el botón
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}