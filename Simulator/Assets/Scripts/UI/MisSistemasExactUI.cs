using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MisSistemasExactUI : MonoBehaviour
{
    [Header("Referencias (Asignar en Inspector)")]
    public RectTransform panelMisSistemas;

    [Tooltip("Asigna tu botón original aquí. El script lo ocultará y copiará su función.")]
    public Button botonVolverOriginal;

    public TMP_FontAsset fuenteTMP;

    [Header("Sprites Descargados")]
    public Sprite spriteFondoUniverso;
    public Sprite spriteIconoBasura;

    [Header("Nuevos Sprites (Icono y Bordes)")]
    public Sprite spriteIconoSistema;
    public Sprite spriteFondoTarjetaRedondeada;

    [Header("Colores de Interfaz")]
    public Color colorPanelFondo = new Color32(11, 16, 48, 255);
    public Color colorTitulo = new Color32(215, 203, 255, 255);
    public Color colorSubtitulo = new Color32(185, 177, 232, 255);
    public Color colorTarjeta = new Color32(21, 28, 74, 255);
    public Color colorBordeTarjeta = new Color32(46, 57, 116, 255);
    public Color colorBotonCargar = new Color32(101, 54, 255, 255);
    public Color colorBotonBorrar = new Color32(255, 232, 232, 255);

    [Header("Configuración del Grid")]
    public Vector2 tamañoTarjeta = new Vector2(480, 260);
    public Vector2 espaciado = new Vector2(40, 40);

    private readonly string[] nombres =
    {
        "Sistema Alpha",
        "Galaxia Personal",
        "Nebulosa 42",
        "Constelación Nova",
        "Sistema Binario",
        "Experimento Orbital"
    };

    private readonly string[] infos =
    {
        "5 planetas",
        "8 planetas",
        "3 planetas",
        "12 planetas",
        "6 planetas",
        "4 planetas"
    };

    private readonly string[] fechas =
    {
        "10/3/2026",
        "12/3/2026",
        "13/3/2026",
        "14/3/2026",
        "15/3/2026",
        "15/3/2026"
    };

    [ContextMenu("1. Generar Interfaz en Editor")]
    public void GenerarEnEditor()
    {
        if (panelMisSistemas == null)
        {
            Debug.LogError("Asigna panelMisSistemas en el Inspector.");
            return;
        }

        LimpiarEscena();
        GenerarEscena();
    }

    [ContextMenu("2. Limpiar Generados")]
    public void LimpiarEscena()
    {
        if (panelMisSistemas == null) return;

        for (int i = panelMisSistemas.childCount - 1; i >= 0; i--)
        {
            GameObject child = panelMisSistemas.GetChild(i).gameObject;

            if (child.name.Contains("Auto"))
            {
#if UNITY_EDITOR
                DestroyImmediate(child);
#else
                Destroy(child);
#endif
            }
        }
    }

    private void GenerarEscena()
    {
        EstilizarPanelBase();
        CrearImagenFondo();
        CrearBotonVolverNuevo();
        CrearCabeceraTexto();

        GameObject gridObj = CrearObjetoUI("GridContenedorAuto", panelMisSistemas);
        RectTransform gridRT = gridObj.GetComponent<RectTransform>();

        gridRT.anchorMin = new Vector2(0.05f, 0.05f);
        gridRT.anchorMax = new Vector2(0.95f, 0.75f);
        gridRT.offsetMin = Vector2.zero;
        gridRT.offsetMax = Vector2.zero;

        GridLayoutGroup glg = gridObj.AddComponent<GridLayoutGroup>();
        glg.cellSize = tamañoTarjeta;
        glg.spacing = espaciado;
        glg.childAlignment = TextAnchor.UpperCenter;
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = 3;

        for (int i = 0; i < nombres.Length; i++)
        {
            CrearTarjeta(gridObj.transform, nombres[i], infos[i], fechas[i]);
        }
    }

    private void CrearImagenFondo()
    {
        if (spriteFondoUniverso == null) return;

        GameObject fondoObj = CrearObjetoUI("FondoEstrelladoAuto", panelMisSistemas);
        ConfigurarAnclaje(fondoObj, Vector2.zero, Vector2.one, Vector2.zero);

        Image img = fondoObj.AddComponent<Image>();
        img.sprite = spriteFondoUniverso;
        img.type = Image.Type.Simple;
        img.color = Color.white;
        img.raycastTarget = false;

        fondoObj.transform.SetAsFirstSibling();
    }

    private void CrearTarjeta(Transform padre, string nombre, string info, string fecha)
    {
        GameObject tarjeta = CrearObjetoUI("TarjetaAuto_" + nombre, padre);

        Image bg = tarjeta.AddComponent<Image>();
        bg.color = colorTarjeta;

        if (spriteFondoTarjetaRedondeada != null)
        {
            bg.sprite = spriteFondoTarjetaRedondeada;
            bg.type = Image.Type.Sliced;
            bg.pixelsPerUnitMultiplier = 1f;
        }

        UnityEngine.UI.Outline outline = tarjeta.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = colorBordeTarjeta;
        outline.effectDistance = new Vector2(1, -1);

        CrearIconoCentral(tarjeta.transform);

        CrearTextoEnTarjeta(
            tarjeta.transform,
            "Nombre",
            nombre,
            new Vector2(0.06f, 0.40f),
            new Vector2(0.94f, 0.55f),
            24,
            FontStyles.Bold,
            Color.white,
            TextAlignmentOptions.Left
        );

        CrearTextoEnTarjeta(
            tarjeta.transform,
            "Info",
            info,
            new Vector2(0.06f, 0.28f),
            new Vector2(0.5f, 0.38f),
            16,
            FontStyles.Normal,
            colorSubtitulo,
            TextAlignmentOptions.Left
        );

        CrearTextoEnTarjeta(
            tarjeta.transform,
            "Fecha",
            fecha,
            new Vector2(0.5f, 0.28f),
            new Vector2(0.94f, 0.38f),
            15,
            FontStyles.Normal,
            colorSubtitulo,
            TextAlignmentOptions.Right
        );

        CrearBotonCargar(tarjeta.transform, new Vector2(0.06f, 0.08f), new Vector2(0.80f, 0.22f));
        CrearBotonBorrarConIcono(tarjeta.transform, new Vector2(0.84f, 0.08f), new Vector2(0.94f, 0.22f));
    }

    private void CrearIconoCentral(Transform padre)
    {
        GameObject iconoObj = CrearObjetoUI("IconoCentralAuto", padre);
        ConfigurarAnclaje(iconoObj, new Vector2(0.35f, 0.60f), new Vector2(0.65f, 0.90f), Vector2.zero);

        Image img = iconoObj.AddComponent<Image>();

        if (spriteIconoSistema != null)
        {
            img.sprite = spriteIconoSistema;
            img.color = Color.white;
            img.preserveAspect = true;
        }
        else
        {
            img.color = new Color32(255, 184, 52, 100);
        }
    }

    private void CrearBotonCargar(Transform padre, Vector2 min, Vector2 max)
    {
        GameObject btn = CrearObjetoUI("BtnCargarAuto", padre);
        ConfigurarAnclaje(btn, min, max, Vector2.zero);

        Image bg = btn.AddComponent<Image>();
        bg.color = colorBotonCargar;

        if (spriteFondoTarjetaRedondeada != null)
        {
            bg.sprite = spriteFondoTarjetaRedondeada;
            bg.type = Image.Type.Sliced;
        }

        btn.AddComponent<Button>();
        CrearTextoInterno(btn.transform, "CARGAR", Color.white);
    }

    private void CrearBotonBorrarConIcono(Transform padre, Vector2 min, Vector2 max)
    {
        GameObject btnBorrar = CrearObjetoUI("BtnBorrarAuto", padre);
        ConfigurarAnclaje(btnBorrar, min, max, Vector2.zero);

        Image imgBtn = btnBorrar.AddComponent<Image>();
        imgBtn.color = colorBotonBorrar;

        if (spriteFondoTarjetaRedondeada != null)
        {
            imgBtn.sprite = spriteFondoTarjetaRedondeada;
            imgBtn.type = Image.Type.Sliced;
        }

        btnBorrar.AddComponent<Button>();

        if (spriteIconoBasura != null)
        {
            GameObject icoObj = CrearObjetoUI("IconoBasura", btnBorrar.transform);
            ConfigurarAnclaje(icoObj, new Vector2(0.2f, 0.2f), new Vector2(0.8f, 0.8f), Vector2.zero);

            Image imgIco = icoObj.AddComponent<Image>();
            imgIco.sprite = spriteIconoBasura;
            imgIco.color = Color.white;
            imgIco.preserveAspect = true;
        }
        else
        {
            CrearTextoInterno(btnBorrar.transform, "X", Color.red);
        }
    }

    private void EstilizarPanelBase()
    {
        Image img = panelMisSistemas.GetComponent<Image>();

        if (img == null)
        {
            img = panelMisSistemas.gameObject.AddComponent<Image>();
        }

        img.color = colorPanelFondo;
        img.raycastTarget = false;
    }

    private void CrearBotonVolverNuevo()
    {
        if (botonVolverOriginal != null)
        {
            botonVolverOriginal.gameObject.SetActive(false);
        }

        GameObject btnNuevo = CrearObjetoUI("BtnVolverAuto", panelMisSistemas);
        RectTransform rt = btnNuevo.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0.05f, 0.95f);
        rt.anchorMax = new Vector2(0.05f, 0.95f);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(0, -20);
        rt.sizeDelta = new Vector2(170, 45);

        Image img = btnNuevo.AddComponent<Image>();
        img.color = new Color32(20, 25, 45, 255);

        if (spriteFondoTarjetaRedondeada != null)
        {
            img.sprite = spriteFondoTarjetaRedondeada;
            img.type = Image.Type.Sliced;
        }

        UnityEngine.UI.Outline outline = btnNuevo.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = new Color32(60, 70, 110, 255);
        outline.effectDistance = new Vector2(1, -1);

        Button btn = btnNuevo.AddComponent<Button>();

        if (botonVolverOriginal != null)
        {
            btn.onClick = botonVolverOriginal.onClick;
        }

        GameObject txtObj = CrearObjetoUI("Text", btnNuevo.transform);
        ConfigurarAnclaje(txtObj, Vector2.zero, Vector2.one, Vector2.zero);

        TextMeshProUGUI tmp = txtObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "←  Volver al Menú";
        tmp.fontSize = 16;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        if (fuenteTMP != null)
        {
            tmp.font = fuenteTMP;
        }
    }

    private void CrearCabeceraTexto()
    {
        CrearTextoTMP(
            "TituloAuto",
            "Mis Sistemas Solares",
            new Vector2(0.05f, 0.88f),
            46f,
            FontStyles.Bold,
            colorTitulo
        );

        CrearTextoTMP(
            "SubtituloAuto",
            nombres.Length + " sistemas creados",
            new Vector2(0.05f, 0.82f),
            18f,
            FontStyles.Normal,
            colorSubtitulo
        );
    }

    private void CrearTextoInterno(Transform padre, string texto, Color? color = null)
    {
        GameObject t = CrearObjetoUI("Label", padre);
        ConfigurarAnclaje(t, Vector2.zero, Vector2.one, Vector2.zero);

        TextMeshProUGUI tmp = t.AddComponent<TextMeshProUGUI>();
        tmp.text = texto;
        tmp.fontSize = 14;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color ?? Color.white;

        if (fuenteTMP != null)
        {
            tmp.font = fuenteTMP;
        }
    }

    private void CrearTextoEnTarjeta(
        Transform padre,
        string nombre,
        string contenido,
        Vector2 min,
        Vector2 max,
        float size,
        FontStyles estilo,
        Color color,
        TextAlignmentOptions alineacion)
    {
        GameObject go = CrearObjetoUI(nombre + "Auto", padre);
        ConfigurarAnclaje(go, min, max, Vector2.zero);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = contenido;
        tmp.fontSize = size;
        tmp.fontStyle = estilo;
        tmp.color = color;
        tmp.alignment = alineacion;

        if (fuenteTMP != null)
        {
            tmp.font = fuenteTMP;
        }
    }

    private void CrearTextoTMP(
        string nombre,
        string contenido,
        Vector2 anclaje,
        float size,
        FontStyles estilo,
        Color color)
    {
        GameObject go = CrearObjetoUI(nombre, panelMisSistemas);
        RectTransform rt = go.GetComponent<RectTransform>();

        rt.anchorMin = anclaje;
        rt.anchorMax = anclaje;
        rt.pivot = anclaje;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(800, 100);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = contenido;
        tmp.fontSize = size;
        tmp.fontStyle = estilo;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Left;

        if (fuenteTMP != null)
        {
            tmp.font = fuenteTMP;
        }
    }

    private GameObject CrearObjetoUI(string nombre, Transform padre)
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(padre, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    private void ConfigurarAnclaje(GameObject go, Vector2 min, Vector2 max, Vector2 offset)
    {
        RectTransform rt = go.GetComponent<RectTransform>();

        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = offset;
        rt.offsetMax = -offset;
    }
}