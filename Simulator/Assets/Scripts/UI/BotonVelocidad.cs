using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonVelocidad : MonoBehaviour
{
    [SerializeField] NBodySimulation simulator;
    private Slider sliderVelocidad;
    public float velocidad;

    void Awake()
    {
        sliderVelocidad = GetComponent<Slider>();
    }

    public void velocidadSlider()
    {
            velocidad = sliderVelocidad.value;
            simulator.ChangeTimeScale((float)velocidad);
    }
}