using UnityEngine;
using UnityEngine.UI;

public class EditPanelManager : MonoBehaviour
{
    [SerializeField] Toggle gizmoButton;
    [SerializeField] SelectedBodyManager selectedBodyManager;
    [SerializeField] GameObject gizmo;
    private GizmoManager gizmoManager;

    private bool showGizmo = false;

    void Awake() {
        gizmoManager = gizmo.GetComponent<GizmoManager>();
    }

    void OnEnable()
    {
        Debug.Log("asd");
        if (selectedBodyManager.selectedBody) {
            gizmoButton.gameObject.SetActive(true);
        } else {
            gizmoButton.gameObject.SetActive(false);
        }
    }

    void Update() {

        if (showGizmo) UpdateGizmo();
    }

    private void UpdateGizmo()
    {
        Physics.SyncTransforms();
        gizmo.transform.position = selectedBodyManager.selectedBody.transform.position;
    }

    public void toggleGizmo()
    {
        
        if (showGizmo) {
            showGizmo = false;
            gizmo.SetActive(false);
            return;
        }
        showGizmo = true;
        displayGizmo();
    }

    public void displayGizmo() {
        if (showGizmo) {
            Physics.SyncTransforms();
            gizmo.SetActive(true);
            gizmo.transform.position = selectedBodyManager.selectedBody.transform.position;

            float objRadius = selectedBodyManager.selectedBody.GetComponent<SharedSettings>().getRadius();

            gizmoManager.scale = objRadius;
            gizmoManager.Generate();
        }
    }

    public void dragObjectWithGizmo(ArrowGenerator arrow) {
        gizmoManager.dragObject(arrow, selectedBodyManager.selectedBody);
    }

    public void deselectGizmoButton() {
        if (!gizmoButton.isOn) {
            gizmoButton.isOn = true;
        }
    }



    
}
