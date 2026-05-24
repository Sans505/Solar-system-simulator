using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SliderReleaseListener : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;

    public UnityEvent<float> onRelease = new UnityEvent<float>();

    public void OnPointerUp(PointerEventData eventData)
    {
        onRelease.Invoke(slider.value);
    }
}