using UnityEngine;
using UnityEngine.UI;

public class ControladorHeader : MonoBehaviour
{
    [Header("Paneles de las Pestañas")]
    public GameObject panelFisica;
    public GameObject panelForma;
    public GameObject panelColor;

    [Header("Botones de las Pestañas")]
    public Button botonFisica;
    public Button botonForma;
    public Button botonColor;

    private Color colorActivo = new Color(0.5f, 0.15f, 0.45f);   // Rosa/Morado Oscuro
    private Color colorInactivo = new Color(0.75f, 0.35f, 0.7f); // Rosa/Morado Claro

    private void Start()
    {
        MostrarFisica();
    }

    // Mostrar panel fisica
    public void MostrarFisica()
    {
        panelFisica.SetActive(true);
        panelForma.SetActive(false);
        panelColor.SetActive(false);

        PintarBotones(botonFisica, botonForma, botonColor);
    }

    // Mostrar panel forma
    public void MostrarForma()
    {
        panelFisica.SetActive(false);
        panelForma.SetActive(true);
        panelColor.SetActive(false);

        PintarBotones(botonForma, botonFisica, botonColor);
    }

    // Mostrar panel color
    public void MostrarColor()
    {
        panelFisica.SetActive(false);
        panelForma.SetActive(false);
        panelColor.SetActive(true);

        PintarBotones(botonColor, botonFisica, botonForma);
    }

    private void PintarBotones(Button activo, Button inactivo1, Button inactivo2)
    {
        activo.GetComponent<Image>().color = colorActivo;
        inactivo1.GetComponent<Image>().color = colorInactivo;
        inactivo2.GetComponent<Image>().color = colorInactivo;
    }

    public void MostrarSoloFisica() {
        botonFisica.gameObject.SetActive(true);
        botonForma.gameObject.SetActive(false);
        botonColor.gameObject.SetActive(false);
        MostrarFisica();
    }

    public void MostrarTodosLosBotones() {
        botonFisica.gameObject.SetActive(true);
        botonForma.gameObject.SetActive(true);
        botonColor.gameObject.SetActive(true);
    }
}