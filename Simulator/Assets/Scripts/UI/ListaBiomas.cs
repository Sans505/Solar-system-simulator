using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ListaBiomas : MonoBehaviour
{
    [Header("Configuracion del Scroll View")]
    public Transform contenedorContent;  // Content del scroll view
    public GameObject biomaPrefab;  // Prefab
    public void AñadirNuevoBioma()
    {
        if (biomaPrefab != null && contenedorContent != null)
        {
            GameObject nuevoBioma = Instantiate(biomaPrefab, contenedorContent);

            Transform botonMenosTransform = nuevoBioma.transform.Find("EliminarBioma");
            if (botonMenosTransform != null)
            {
                Button btnMenos = botonMenosTransform.GetComponent<Button>();
                btnMenos.onClick.RemoveAllListeners();
                btnMenos.onClick.AddListener(() => EliminarBioma(nuevoBioma));
            }

            ReorganizarNombres();
        }
    }

    public void EliminarBioma(GameObject biomaAEliminar)
    {
        if (biomaAEliminar != null)
        {
            Destroy(biomaAEliminar);

            //esperamos un frame para que se destruya el objeto antes de reorganizar los nombres
            Invoke(nameof(ReorganizarNombres), 0.1f);
        }
    }

    private void ReorganizarNombres()
    {
        int contador = 1;
        for (int i = 0; i < contenedorContent.childCount; i++)
        {
            Transform hijo = contenedorContent.GetChild(i);
            hijo.gameObject.name = "Bioma " + contador;

            Transform textoTransform = hijo.Find("TextBioma");
            if (textoTransform != null)
            {
                textoTransform.GetComponent<TextMeshProUGUI>().text = "Bioma " + contador;
            }
            contador++;
        }
    }
}