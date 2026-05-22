using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    private Toggle toggle;
    private Image image;

    [SerializeField] private Color toggledColor = Color.gray;

    private Color originalColor;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        image = toggle.targetGraphic as Image;

        originalColor = image.color; // guardamos el color original

        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(toggle.isOn); // aplicar estado inicial
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            // aplicar tint multiplicando colores
            image.color = MultiplyColors(originalColor, toggledColor);
        }
        else
        {
            // volver al color original
            image.color = originalColor;
        }
    }

    private Color MultiplyColors(Color a, Color b)
    {
        return new Color(
            a.r * b.r,
            a.g * b.g,
            a.b * b.b,
            a.a * b.a
        );
    }
}