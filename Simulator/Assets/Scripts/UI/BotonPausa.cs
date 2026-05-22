using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonPausa : MonoBehaviour
{
    [SerializeField] NBodySimulation simulator;
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
            simulator.PauseSimulation();
            if (textoBoton != null) textoBoton.text = "REANUDAR";
        }
        else
        {
            simulator.ResumeSimulation();
            if (textoBoton != null) textoBoton.text = "PAUSA";
        }


        // Deselccionar el botón
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Activate() {
        estaPausado = false;
        if (textoBoton != null) textoBoton.text = "PAUSA";
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        estaPausado = false;
        gameObject.SetActive(false);
    }
}