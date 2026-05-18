using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AutoAuthForm : MonoBehaviour
{
    private GameObject canvasObj;
    private GameObject formPanel;
    private InputField emailField;
    private InputField passwordField;
    private Text statusText;

    // Asegúrate de que tu servidor Node.js esté corriendo en la terminal
    private string loginUrl = "http://localhost:3000/login";

    void Start()
    {
        CrearInterfaz();
        if (formPanel != null) formPanel.SetActive(false);
    }

    void CrearInterfaz()
    {
        // 1. Configuración del Canvas
        canvasObj = new GameObject("AuthCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // 2. Botón para abrir el formulario (Azul)
        GameObject openBtnObj = new GameObject("Btn_AbrirLogin");
        openBtnObj.transform.SetParent(canvasObj.transform);
        openBtnObj.AddComponent<Image>().color = new Color(0.1f, 0.4f, 0.8f);
        Button openBtn = openBtnObj.AddComponent<Button>();
        RectTransform oRect = openBtnObj.GetComponent<RectTransform>();
        oRect.sizeDelta = new Vector2(180, 50);
        oRect.anchoredPosition = new Vector2(0, 220);
        CrearTextoSimple("Text", "INICIAR SESIÓN", openBtnObj.transform, Color.white, true);
        openBtn.onClick.AddListener(() => formPanel.SetActive(true));

        // 3. Panel del Formulario (Fondo oscuro)
        formPanel = new GameObject("FormPanel");
        formPanel.transform.SetParent(canvasObj.transform);
        formPanel.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.98f);
        RectTransform pRect = formPanel.GetComponent<RectTransform>();
        pRect.sizeDelta = new Vector2(400, 500);
        pRect.anchoredPosition = Vector2.zero;

        // 4. InputFields (Email y Password)
        emailField = CrearInputField("EmailInput", new Vector2(0, 100), "Introduce Email...", formPanel.transform);
        passwordField = CrearInputField("PassInput", new Vector2(0, 20), "Contraseña...", formPanel.transform);
        passwordField.contentType = InputField.ContentType.Password;

        // 5. Botón Entrar (Verde)
        GameObject loginBtnObj = new GameObject("LoginButton");
        loginBtnObj.transform.SetParent(formPanel.transform);
        loginBtnObj.AddComponent<Image>().color = new Color(0.2f, 0.7f, 0.2f);
        Button loginBtn = loginBtnObj.AddComponent<Button>();
        RectTransform lRect = loginBtnObj.GetComponent<RectTransform>();
        lRect.sizeDelta = new Vector2(180, 55);
        lRect.anchoredPosition = new Vector2(0, -90);
        CrearTextoSimple("Txt", "ENTRAR", loginBtnObj.transform, Color.white, true);
        loginBtn.onClick.AddListener(() => StartCoroutine(PostLogin()));

        // 6. Botón Cerrar (Rojo)
        GameObject closeBtnObj = new GameObject("CloseButton");
        closeBtnObj.transform.SetParent(formPanel.transform);
        closeBtnObj.AddComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f);
        Button closeBtn = closeBtnObj.AddComponent<Button>();
        RectTransform cRect = closeBtnObj.GetComponent<RectTransform>();
        cRect.sizeDelta = new Vector2(40, 40);
        cRect.anchoredPosition = new Vector2(175, 225);
        CrearTextoSimple("Txt", "X", closeBtnObj.transform, Color.white, true);
        closeBtn.onClick.AddListener(() => formPanel.SetActive(false));

        // 7. Texto de Estado
        statusText = CrearTextoSimple("StatusText", "Esperando...", formPanel.transform, Color.yellow, true);
        statusText.rectTransform.anchoredPosition = new Vector2(0, -170);
    }

    InputField CrearInputField(string nombre, Vector2 pos, string placeholder, Transform padre)
    {
        // Contenedor principal del Input
        GameObject root = new GameObject(nombre);
        root.transform.SetParent(padre);
        root.AddComponent<Image>().color = Color.white;
        InputField input = root.AddComponent<InputField>();
        RectTransform rt = root.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 55);
        rt.anchoredPosition = pos;

        // Objeto para el texto que escribe el usuario
        GameObject txtObj = new GameObject("TextDisplay");
        txtObj.transform.SetParent(root.transform);
        Text t = txtObj.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.color = Color.black;
        t.fontSize = 24;
        t.alignment = TextAnchor.MiddleCenter;
        
        RectTransform tRect = t.rectTransform;
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.offsetMin = new Vector2(15, 5);
        tRect.offsetMax = new Vector2(-15, -5);

        // Objeto para el texto de fondo (Placeholder)
        GameObject phObj = new GameObject("PlaceholderDisplay");
        phObj.transform.SetParent(root.transform);
        Text p = phObj.AddComponent<Text>();
        p.text = placeholder;
        p.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        p.color = new Color(0.4f, 0.4f, 0.4f, 0.6f);
        p.fontSize = 22;
        p.fontStyle = FontStyle.Italic;
        p.alignment = TextAnchor.MiddleCenter;
        
        RectTransform pRect = p.rectTransform;
        pRect.anchorMin = Vector2.zero;
        pRect.anchorMax = Vector2.one;
        pRect.offsetMin = new Vector2(15, 5);
        pRect.offsetMax = new Vector2(-15, -5);

        input.textComponent = t;
        input.placeholder = p;
        input.targetGraphic = root.GetComponent<Image>();

        return input;
    }

    Text CrearTextoSimple(string nombre, string contenido, Transform padre, Color color, bool center)
    {
        GameObject obj = new GameObject(nombre);
        obj.transform.SetParent(padre);
        Text t = obj.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text = contenido;
        t.color = color;
        t.fontSize = 24;
        t.alignment = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        t.rectTransform.sizeDelta = new Vector2(350, 50);
        return t;
    }

    IEnumerator PostLogin()
    {
        if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            statusText.text = "Error: Rellena los campos";
            yield break;
        }

        statusText.text = "Enviando datos...";
        
        // Crear JSON manualmente para evitar dependencias extra
        string json = "{\"correo\":\"" + emailField.text + "\", \"password\":\"" + passwordField.text + "\"}";
        
        using (UnityWebRequest www = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                statusText.color = Color.green;
                statusText.text = "¡Conectado correctamente!";
                Debug.Log("Login exitoso");
                Invoke("CerrarPanel", 1.5f);
            }
            else
            {
                statusText.color = Color.red;
                statusText.text = "Error de Login";
                Debug.Log("Error: " + www.error);
            }
        }
    }

    void CerrarPanel()
    {
        if (formPanel != null) formPanel.SetActive(false);
    }
}