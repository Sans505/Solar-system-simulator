using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonPausa : MonoBehaviour
{
    [SerializeField] NBodySimulation simulator;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite resumeSprite;
    private bool estaPausado = false;
    private Button miBoton;
    private Image image;

    void Awake()
    {
        miBoton = GetComponent<Button>();
        image = miBoton.targetGraphic as Image;
    }

    public void AlternarPausa()
    {
        estaPausado = !estaPausado;

        if (estaPausado)
        {
            simulator.PauseSimulation();
            if (image != null) image.sprite = resumeSprite;
        }
        else
        {
            simulator.ResumeSimulation();
            if (image != null) image.sprite = pauseSprite;
        }


        // Deselccionar el botón
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Activate() {
        estaPausado = false;
        if (image != null) image.sprite = pauseSprite;
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        estaPausado = false;
        gameObject.SetActive(false);
    }
}