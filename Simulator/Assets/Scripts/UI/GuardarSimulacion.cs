using UnityEngine;
using System.IO;
using TMPro; 

[System.Serializable]
public class DatosSimulacion
{
    // Aqui irian las variables de los datos a guardar
}

public class GuardarSimulacion : MonoBehaviour
{
    private string carpetaSimulaciones;

    [Header("Conexión con el Pop-up")]
    public TMP_InputField cajaNombreArchivo; 
    public GameObject popUpGuardar;

    private void Start()
    {
        carpetaSimulaciones = Path.Combine(Application.persistentDataPath, "Simulaciones");

        if (!Directory.Exists(carpetaSimulaciones))
        {
            Directory.CreateDirectory(carpetaSimulaciones);
        }
    }

    public void GuardarSim()
    {
        string nombreArchivo = cajaNombreArchivo.text;

        if (string.IsNullOrEmpty(nombreArchivo) || string.IsNullOrWhiteSpace(nombreArchivo))
        {
            Debug.LogWarning("No se puede guardar: El nombre está vacío.");
            return; 
        }

        if (!nombreArchivo.EndsWith(".json"))
        {
            nombreArchivo += ".json";
        }

        string rutaCompleta = Path.Combine(carpetaSimulaciones, nombreArchivo);

        DatosSimulacion datosAGuardar = new DatosSimulacion();
        
        // Aqui se tienen que asignar los datos a las variables de antes

        string textoJson = JsonUtility.ToJson(datosAGuardar, true);
        File.WriteAllText(rutaCompleta, textoJson);
        
        Debug.Log("Simulación guardada con éxito en: " + rutaCompleta);

        cajaNombreArchivo.text = "";
        if (popUpGuardar != null) popUpGuardar.SetActive(false);
    }
}