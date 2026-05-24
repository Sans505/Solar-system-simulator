using UnityEngine;
using TMPro;

public class MostrarSlider : MonoBehaviour
{

    public TextMeshProUGUI textoSlider;
    public void CambiarText(float valor)
    {
        textoSlider.text = valor.ToString("F2");
    }

    public void CambiarTextSinDecimal(float valor) {
        textoSlider.text = valor.ToString("F0");
    }

    public void CambiarTextPercentage(float valor)
    {
        textoSlider.text = (valor * 100).ToString("F0" ) + "%";
    }
}
