using UnityEngine;
using TMPro;
using UnityEngine.UI; // 👈 ¡IMPORTANTE! Añade esto arriba para poder usar el componente Button

public class ListaRuidos : MonoBehaviour
{
    // Contenedor del Scroll View
    public Transform contenedorContent;

    // Prefab (El archivo azul del Project)
    public GameObject bloqueRuidoPrefab;

    public void AñadirNuevoRuido()
    {
        if (bloqueRuidoPrefab != null && contenedorContent != null)
        {
            // 1. Creamos el clon
            GameObject nuevoRuido = Instantiate(bloqueRuidoPrefab, contenedorContent);
            
            // 2. 🌟 EL TRUCO POR CÓDIGO: Buscamos su botón "EliminarCapa" y le conectamos la función
            // Cambia "EliminarCapa" por el nombre exacto que tenga el objeto de tu botón en el Prefab
            Transform botonEliminarTransform = nuevoRuido.transform.Find("EliminarCapa"); 
            if (botonEliminarTransform != null)
            {
                Button botonEliminar = botonEliminarTransform.GetComponent<Button>();
                
                // Limpiamos cualquier cable viejo que tenga el prefab para que no falle
                botonEliminar.onClick.RemoveAllListeners(); 
                
                // Le decimos: "Al hacer click, llama a EliminarRuido de este script y pásate a ti mismo"
                botonEliminar.onClick.AddListener(() => EliminarRuido(nuevoRuido));
            }

            // 3. Reorganizamos los nombres y textos
            ReorganizarCapas();
        }
    }

    public void EliminarRuido(GameObject ruidoEliminar)
    {
        if (ruidoEliminar != null)
        {
            // Reorganizamos ignorando esta capa antes de destruirla
            ReorganizarCapas(ruidoEliminar);
            Destroy(ruidoEliminar);
        }
    }

    private void ReorganizarCapas(GameObject objetoAIgnorar = null)
    {
        if (contenedorContent == null) return;

        int contadorReal = 1;

        for (int i = 0; i < contenedorContent.childCount; i++)
        {
            Transform hijoCapa = contenedorContent.GetChild(i);

            if (hijoCapa.gameObject == objetoAIgnorar)
            {
                continue; 
            }

            hijoCapa.name = "Ruido" + contadorReal;

            Transform textoTransform = hijoCapa.Find("Capa");
            if (textoTransform != null)
            {
                textoTransform.GetComponent<TextMeshProUGUI>().text = "Capa " + contadorReal;
            }

            contadorReal++;
        }
    }
}