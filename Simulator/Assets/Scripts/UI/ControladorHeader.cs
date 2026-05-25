using UnityEngine;
using UnityEngine.UI;

public class ControladorHeader : MonoBehaviour
{
    [Header("Modo de Edición")]
    public bool editandoPlaneta = false; // true edicion planeta, false edición sol
    [Header("Contenedores Principales (Padres)")]
    public GameObject contenedorPlaneta;
    public GameObject contenedorSol;

    [Header("Paneles de las Pestañas (Planetas)")]
    public GameObject panelFisica;
    public GameObject panelForma;
    public GameObject panelColor;

    [Header("Paneles de las Pestañas (Sol)")]
    public GameObject panelFisicaSol;
    public GameObject panelFormaSol;
    public GameObject panelColorSol;

    [Header("Botones de las Pestañas")]
    public Button botonFisica;
    public Button botonForma;
    public Button botonColor;

    private Color colorActivo = new Color(0.5f, 0.15f, 0.45f);
    private Color colorInactivo = new Color(0.75f, 0.35f, 0.7f);

    private void Start()
    {
        if (botonFisica != null) botonFisica.onClick.AddListener(MostrarFisica);
        if (botonForma != null) botonForma.onClick.AddListener(MostrarForma);
        if (botonColor != null) botonColor.onClick.AddListener(MostrarColor);

        MostrarFisica();
    }

    // --- FUNCIONES DE LAS PESTAÑAS ---

    public void MostrarFisica()
    {
        ApagarTodosLosPaneles();
        GestionarPadres();

        if (editandoPlaneta) panelFisica.SetActive(true);
        else panelFisicaSol.SetActive(true);

        PintarBotones(botonFisica, botonForma, botonColor);
    }

    public void MostrarForma()
    {
        ApagarTodosLosPaneles();
        GestionarPadres(); 

        if (editandoPlaneta) panelForma.SetActive(true);
        else panelFormaSol.SetActive(true);

        PintarBotones(botonForma, botonFisica, botonColor);
    }

    public void MostrarColor()
    {
        ApagarTodosLosPaneles();
        GestionarPadres(); 

        if (editandoPlaneta) panelColor.SetActive(true);
        else panelColorSol.SetActive(true);

        PintarBotones(botonColor, botonFisica, botonForma);
    }

    // --- FUNCIONES AUXILIARES ---

    private void GestionarPadres()
    {
        // Si estamos editando el planeta, encendemos el padre Planeta y apagamos el panelSol (y viceversa)
        if (contenedorPlaneta != null) contenedorPlaneta.SetActive(editandoPlaneta);
        if (contenedorSol != null) contenedorSol.SetActive(!editandoPlaneta);
    }

    private void ApagarTodosLosPaneles()
    {
        if (panelFisica != null) panelFisica.SetActive(false);
        if (panelForma != null) panelForma.SetActive(false);
        if (panelColor != null) panelColor.SetActive(false);
        
        if (panelFisicaSol != null) panelFisicaSol.SetActive(false);
        if (panelFormaSol != null) panelFormaSol.SetActive(false);
        if (panelColorSol != null) panelColorSol.SetActive(false);
    }

    private void PintarBotones(Button activo, Button inactivo1, Button inactivo2)
    {
        if (activo != null) activo.GetComponent<Image>().color = colorActivo;
        if (inactivo1 != null) inactivo1.GetComponent<Image>().color = colorInactivo;
        if (inactivo2 != null) inactivo2.GetComponent<Image>().color = colorInactivo;
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

    public void CambiarPaneles(bool planetEdit) {
        editandoPlaneta = planetEdit;
        MostrarFisica();
    }
}