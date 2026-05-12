using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    [SerializeField]
    private SelectedBodyManager selectedBodyManager;
    public float doubleClickTime = 0.3f;
    private float lastClickTime;
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Highlight
        if (highlight != null)
        {
            highlight.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Vector2 mousePos = mouse.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                Outline outline = highlight.GetComponent<Outline>();

                if (outline != null)
                {
                    outline.enabled = true;
                }
                else
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    //outline.OutlineColor = Color.white;
                    //outline.OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (mouse.leftButton.wasPressedThisFrame)
        {
            bool doubleSelected = false;
            if (Time.unscaledTime - lastClickTime <= doubleClickTime) {
                doubleSelected = true;
            }
            lastClickTime = Time.unscaledTime;
            //Debug.Log(lastClickTime);

            if (highlight)
            {
                if (selection != null)
                {
                    selection.GetComponent<Outline>().enabled = false;
                }

                selection = raycastHit.transform;
                selection.GetComponent<Outline>().enabled = true;
                highlight = null;

                selectedBodyManager.SelectBody(selection.gameObject, doubleSelected);
            }
            else
            {
                if (selection)
                {
                    selection.GetComponent<Outline>().enabled = false;
                    selection = null;

                    //selectedBodyManager.Deselect();
                }
            }
        }
    }
}