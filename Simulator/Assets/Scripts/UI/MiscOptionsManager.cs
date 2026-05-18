using UnityEngine;

public class MiscOptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPlane;
    [SerializeField] private CameraController cameraController;

    private bool showGrid = true;

    public void toggleGrid() {
        showGrid = !showGrid;
        gridPlane.SetActive(showGrid);
    }

    public void moveCameraToTopView() {
        cameraController.moveToTopView();
    }
}
