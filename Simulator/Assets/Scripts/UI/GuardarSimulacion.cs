using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class DatosSimulacion
{
    // Añadir aquí las variables que se van a guardar
}

public class GuardarSimulacion : MonoBehaviour
{
    public void GuardarSim()
    {
#if UNITY_EDITOR
        string rutaElegida = EditorUtility.SaveFilePanel("Guardar Simulación", "", "simulacion", "json");

        if (!string.IsNullOrEmpty(rutaElegida))
        {
            DatosSimulacion datosAGuardar = new DatosSimulacion();
            
            // Asignar los valores a datosAGuardar antes de guardar

            string textoJson = JsonUtility.ToJson(datosAGuardar, true);

            using (StreamWriter outputFile = new StreamWriter(rutaElegida, false))
            {
                outputFile.Write(textoJson); 
            }
            
            Debug.Log("Simulación guardada en: " + rutaElegida);
        }
        else
        {
            Debug.Log("Guardado cancelado.");
        }
#else
        Debug.LogWarning("El guardado con explorador requiere el Editor de Unity.");
#endif
    }
}