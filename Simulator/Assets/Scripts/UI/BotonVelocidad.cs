using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonVelocidad : MonoBehaviour
{
    private Slider sliderVelocidad;
    public float velocidad;

    void Awake()
    {
        sliderVelocidad = GetComponent<Slider>();
    }

    public void velocidadSlider()
    {
            velocidad = sliderVelocidad.value;
            Time.timeScale = (float)velocidad;
    }
}