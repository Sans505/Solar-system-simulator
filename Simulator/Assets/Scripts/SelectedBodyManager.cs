using UnityEngine;

public class SelectedBodyManager : MonoBehaviour
{
    public GameObject selectedBody;
    [SerializeField]
    private OrbitLine orbitLine;
    [SerializeField]
    private CameraController cameraController;
    private bool isDoubleSelected;

    public void SelectBody(GameObject newBody, bool isDoubleSelected) {
        selectedBody = newBody;
        orbitLine.bodyId = selectedBody.GetComponent<CelestialBody>().id;
        orbitLine.DrawOrbit();

        this.isDoubleSelected = isDoubleSelected;
        if (isDoubleSelected) {
            cameraController.orbitObject(selectedBody);
        }
    }
    public void Deselect() {
        selectedBody = null;
        isDoubleSelected = false;
        orbitLine.Clear();
    }
    
}
