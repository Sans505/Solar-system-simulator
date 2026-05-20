using UnityEngine;
using UnityEngine.UI;

public class UIHoverManager : MonoBehaviour
{
    void Start()
    {
        var graphics = GetComponentsInChildren<Graphic>();

        foreach (var g in graphics)
        {
            GameObject obj = g.gameObject;

            var hover = obj.GetComponent<HoverItem>();
            if (hover == null)
                hover = obj.AddComponent<HoverItem>();

            hover.onEnter += OnEnter;
            hover.onExit += OnExit;
        }
    }

    void OnEnter(GameObject go)
    {
        Debug.Log("Enter: " + go.name);
    }

    void OnExit(GameObject go)
    {
        Debug.Log("Exit: " + go.name);
    }
}