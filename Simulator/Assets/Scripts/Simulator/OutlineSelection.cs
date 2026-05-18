using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private Transform lastClickedObject = null;
    private RaycastHit raycastHit;
    private bool isEnabled = true;

    [SerializeField]
    private SelectedBodyManager selectedBodyManager;
    [SerializeField]
    private EditPanelManager editPanelManager;

    public float doubleClickTime = 0.3f;
    private float lastClickTime = -1f;
    void Update()
    {
        if (!isEnabled) return;
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Highlight
        if (highlight != null)
        {
            switch(highlight.tag) {
                case "Selectable":
                    highlight.GetComponent<Outline>().enabled = false;
                    break;
                case "Gizmo":
                    highlight.GetComponent<ArrowGenerator>().ResetColor();
                    //Debug.Log("Finn");
                    break;
            }
            highlight = null;

        }

        Vector2 mousePos = mouse.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

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
            } else if (highlight.CompareTag("Gizmo")) {

                ArrowGenerator arrow = highlight.GetComponent<ArrowGenerator>();
                arrow.Glow();
                //Debug.Log("Gloww");
            }
            else
            {
                highlight = null;
            }
        }
        

        if (mouse.leftButton.wasPressedThisFrame) {

            if (raycastHit.transform)
            {              
                // Gizmo
                if (raycastHit.transform.CompareTag("Gizmo")) {
                    ArrowGenerator arrow = highlight.GetComponent<ArrowGenerator>();
                    //Debug.Log("Comenzar movimiento");
                    editPanelManager.dragObjectWithGizmo(arrow);
                }
                // Selection
                else
                {
                    bool doubleSelected = false;

                    if (selection == lastClickedObject &&
                        Time.unscaledTime - lastClickTime <= doubleClickTime)
                    {
                        doubleSelected = true;
                        lastClickTime = -1f;
                        lastClickedObject = null;
                    }
                    else
                    {
                        lastClickTime = Time.unscaledTime;
                        lastClickedObject = selection;
                    }

                    if (selection != null)
                    {
                        selection.GetComponent<Outline>().enabled = false;
                    }

                    selection = raycastHit.transform;
                    //Ray clickRay = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                    //RaycastHit clickHit;
//
                    //if (Physics.Raycast(clickRay, out clickHit))
                    //{
                    //    selection = clickHit.transform;
                    //}

                    selection.GetComponent<Outline>().enabled = true;
                    highlight = null;

                    selectedBodyManager.SelectBody(selection.gameObject, doubleSelected);
                }
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

    public void Enable() {
        isEnabled = true;
    }
    public void Disable() {
        isEnabled = false;
    }
}