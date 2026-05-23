using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CargarSimulacion : MonoBehaviour
{
    public void CargarSim()
    {
#if UNITY_EDITOR
        string rutaElegida = EditorUtility.OpenFilePanel("Cargar Simulación", "", "json");

        if (!string.IsNullOrEmpty(rutaElegida))
        {
            Debug.Log("Archivo seleccionado para cargar: " + rutaElegida);
            
        }
        else
        {
            Debug.Log("Carga cancelada.");
        }
#else
        Debug.LogWarning("La carga con explorador requiere el Editor de Unity.");
#endif
    }
}