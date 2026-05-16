using UnityEngine;
using TMPro;

public class MostrarSlider : MonoBehaviour
{

    public TextMeshProUGUI textoSlider;
    public void CambiarText(float valor)
    {
        textoSlider.text = valor.ToString("F0");
    }
}
