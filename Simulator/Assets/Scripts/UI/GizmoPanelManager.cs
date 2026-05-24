using UnityEngine;
using UnityEngine.UI;

public class GizmoPanelManager : MonoBehaviour
{
    [SerializeField] Toggle gizmoButton;
    [SerializeField] SelectedBodyManager selectedBodyManager;
    [SerializeField] CameraController cameraContoller;
    [SerializeField] GameObject gizmo;
    public float gizmoOrbitDistance = 10f;
    private GizmoManager gizmoManager;

    private bool showGizmo = false;

    void Awake() {
        gizmoManager = gizmo.GetComponent<GizmoManager>();
    }

    void OnEnable()
    {
        if (selectedBodyManager.selectedBody) {
            gizmoButton.gameObject.SetActive(true);
        } else {
            gizmoButton.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (selectedBodyManager.selectedBodyExists) {
            if (selectedBodyManager.selectedBody) {
                if (showGizmo) UpdateGizmo();
            } else {
                gizmo.SetActive(false);
            }
        }
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
        cameraContoller.SetOrbitRadius(gizmoOrbitDistance);
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

    public void ActivateGizmoButton() {
        gizmoButton.gameObject.SetActive(true);
    }
    
}
