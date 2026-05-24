using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro; // Necesario para cambiar los textos del Prefab

public class ListaSimulaciones : MonoBehaviour
{
    [Header("Configuración de la Interfaz")]
    public Transform contenedorGrid;
    public GameObject prefabTarjetaSimulacion;
    private string carpetaSimulaciones;

    private void Start()
    {
        carpetaSimulaciones = Path.Combine(Application.persistentDataPath, "Simulaciones");

        CargarYMostrarSimulaciones();
    }

    public void CargarYMostrarSimulaciones()
    {
        foreach (Transform hijo in contenedorGrid)
        {
            Destroy(hijo.gameObject);
        }

        if (!Directory.Exists(carpetaSimulaciones))
        {
            Debug.LogWarning("La carpeta de simulaciones aún no existe.");
            return;
        }

        string[] archivosJson = Directory.GetFiles(carpetaSimulaciones, "*.json");

        Debug.Log($"Se han encontrado {archivosJson.Length} simulaciones guardadas.");

        // Por cada archivo JSON encontrado, creamos una tarjeta visual en la grid
        foreach (string rutaArchivo in archivosJson)
        {
            // Extraemos solo el nombre del archivo sin la ruta ni el .json
            string nombreSimulacion = Path.GetFileNameWithoutExtension(rutaArchivo);

            // Clonamos el prefab dentro de la grid
            GameObject nuevaTarjeta = Instantiate(prefabTarjetaSimulacion, contenedorGrid);

            nuevaTarjeta.GetComponent<TarjetaSimulacion>().nombreDeLaSimulacion = nombreSimulacion;
            
            // Rellenamos datos de la tarjeta
            ConfigurarTextosTarjeta(nuevaTarjeta, nombreSimulacion);
        }
    }

    private void ConfigurarTextosTarjeta(GameObject tarjeta, string nombre)
    {
        // Buscamos los componentes de texto dentro del clon del Prefab.        
        Transform tTitulo = BuscarHijoRecursivo(tarjeta.transform, "TextoTitulo");
        Transform tPlanetas = BuscarHijoRecursivo(tarjeta.transform, "TextoPlanetas");
        Transform tFecha = BuscarHijoRecursivo(tarjeta.transform, "TextoFecha");

        // Cambiamos el título por el nombre real del archivo JSON
        if (tTitulo != null)
        {
            tTitulo.GetComponent<TextMeshProUGUI>().text = nombre;
        }

        // Ponemos un número aleatorio de planetas (ahora es aleatorio, tendria que leerlo del json)
        if (tPlanetas != null)
        {
            int planetasAleatorios = Random.Range(1, 10); // Genera entre 1 y 9 planetas
            tPlanetas.GetComponent<TextMeshProUGUI>().text = "Planetas: " + planetasAleatorios;
        }

        // Ponemos la fecha (esto tambien, no se si lo guardamos en el json, si no tampoco creo que pase nada, se podria quitar incluso).
        if (tFecha != null)
        {
            string fechaActual = System.DateTime.Now.ToString("dd/MM/yyyy");
            tFecha.GetComponent<TextMeshProUGUI>().text = fechaActual;
        }
    }

    // Función auxiliar para buscar los textos
    private Transform BuscarHijoRecursivo(Transform padre, string nombreABuscar)
    {
        Transform resultado = padre.Find(nombreABuscar);
        if (resultado != null) return resultado;

        foreach (Transform hijo in padre)
        {
            resultado = BuscarHijoRecursivo(hijo, nombreABuscar);
            if (resultado != null) return resultado;
        }
        return null;
    }
}