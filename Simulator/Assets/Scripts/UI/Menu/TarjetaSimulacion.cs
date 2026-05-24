using UnityEngine;
using System.IO;
using TMPro;

public class TarjetaSimulacion : MonoBehaviour
{
    [Header("Datos de esta tarjeta")]
    public string nombreDeLaSimulacion; 

    // Función interna para que la tarjeta calcule dónde está su propio archivo
    private string ObtenerRutaDelArchivo()
    {
        string carpeta = Path.Combine(Application.persistentDataPath, "Simulaciones");
        return Path.Combine(carpeta, nombreDeLaSimulacion + ".json");
    }

    public void BotonCargarPulsado()
    {
        string rutaExacta = ObtenerRutaDelArchivo();

        if (File.Exists(rutaExacta))
        {
            Debug.Log("Cargando simulación desde: " + rutaExacta);
            
            // Aqui se tendria que cargar la simulación con los datos del JSON, que estan en la ruta "rutaExacta".

        }
        else
        {
            Debug.LogError("No se ha encontrado el archivo: " + rutaExacta);
        }
    }

    public void BotonEliminarPulsado()
    {
        string rutaExacta = ObtenerRutaDelArchivo();

        // Borrar el arcivo
        if (File.Exists(rutaExacta))
        {
            File.Delete(rutaExacta);
            Debug.Log("Archivo eliminado: " + nombreDeLaSimulacion);
        }

        // Borrar la tarjeta
        Destroy(gameObject);
    }
}