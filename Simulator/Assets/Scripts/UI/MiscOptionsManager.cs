using UnityEngine;

public class MiscOptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPlane;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private HUDManager hudManager;

    private bool showGrid = true;
    private bool showHUD = true;

    public void toggleGrid() {
        showGrid = !showGrid;
        gridPlane.SetActive(showGrid);
    }

    public void toggleHUD() {
        showHUD = !showHUD;
        if (showHUD) {
            hudManager.Enable();
        } else {
            hudManager.Disable();
        }
    }

    public void moveCameraToTopView() {
        cameraController.moveToTopView();
    }
}
