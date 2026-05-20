using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class UIHoverManager : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    
    public bool isHovering = false;

    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        var mouse = Mouse.current;
        PointerEventData data = new PointerEventData(eventSystem);
        data.position = mouse.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);

        if (results.Count > 0)
        {
            isHovering = true;
        } else
        {
            isHovering = false;
        }
    }
}