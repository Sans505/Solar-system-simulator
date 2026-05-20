using UnityEngine;
using UnityEngine.EventSystems;

public class HoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public System.Action<GameObject> onEnter;
    public System.Action<GameObject> onExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(gameObject);
    }
}