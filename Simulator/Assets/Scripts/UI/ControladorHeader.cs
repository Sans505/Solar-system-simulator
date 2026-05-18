using UnityEngine;
using UnityEngine.UI;

public class ControladorHeader : MonoBehaviour
{
    [Header("Paneles de las Pestañas")]
    public GameObject panelFisica;
    public GameObject panelForma;
    public GameObject panelColor;

    [Header("Boton seleccionado default")]
    public Button botonFisica;

    private void Start()
    {
        botonFisica.Select();
    }

    // Mostrar panel fisica
    public void MostrarFisica()
    {
        panelFisica.SetActive(true);
        panelForma.SetActive(false);
        panelColor.SetActive(false);
    }

    // Mostrar panel forma
    public void MostrarForma()
    {
        panelFisica.SetActive(false);
        panelForma.SetActive(true);
        panelColor.SetActive(false);
    }

    // Mostrar panel color
    public void MostrarColor()
    {
        panelFisica.SetActive(false);
        panelForma.SetActive(false);
        panelColor.SetActive(true);
    }
}