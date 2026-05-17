using UnityEngine;
using TMPro;
using UnityEngine.UI; 
public class ListaRuidos : MonoBehaviour
{
    // Contenedor del Scroll View
    public Transform contenedorContent;

    // Prefab
    public GameObject bloqueRuidoPrefab;

    public void AñadirNuevoRuido()
    {
        if (bloqueRuidoPrefab != null && contenedorContent != null)
        {
            // Clon
            GameObject nuevoRuido = Instantiate(bloqueRuidoPrefab, contenedorContent);
            
            // Buscamos su botón "EliminarCapa" y le conectamos la función
            Transform botonEliminarTransform = nuevoRuido.transform.Find("EliminarCapa"); 
            if (botonEliminarTransform != null)
            {
                Button botonEliminar = botonEliminarTransform.GetComponent<Button>();
                
                botonEliminar.onClick.RemoveAllListeners(); 
                
                botonEliminar.onClick.AddListener(() => EliminarRuido(nuevoRuido));
            }

            // Reorganizar nombres
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