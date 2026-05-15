using UnityEngine;
using System.Linq;

public class SelectedBodyManager : MonoBehaviour
{
    public GameObject selectedBody;
    public CelestialBody selectedCelestialBody;
    private SelectionMode mode;

    [SerializeField]
    private NBodySimulation simulator;
    [SerializeField]
    private OrbitLine orbitLine;
    [SerializeField]
    private GameObject velocityArrow;
    [SerializeField]
    private GameObject gizmos;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private GameObject selectionModePanel;
    [SerializeField]
    private GameObject editPanel;
    [SerializeField]
    private GameObject startEditPanel;

    private bool showOrbit = false;
    private bool showVelocity = false;
    private bool showForces = false;
    private bool showGizmo = false;

    private GizmoManager gizmoManager;
    private GameObject[] forceArrows;
    private bool isDoubleSelected;

    void Awake() {
        gizmoManager = gizmos.GetComponent<GizmoManager>();
    }

    void Update() {
        if (selectedBody) {

            if (showVelocity) UpdateVelocityVector();
            if (showForces) UpdateForcesVector();
            if (showGizmo) UpdateGizmo();
            
        }
    }

    private void UpdateVelocityVector()
    {
        velocityArrow.transform.position = selectedBody.transform.position;
        velocityArrow.transform.up = selectedCelestialBody.velocity.normalized;
    }

    private void UpdateForcesVector()
    {
        Vector3[] forces = simulator.getForceVectors(selectedCelestialBody);

        float[] forcesMagnitud = new float[forces.Length];

        for (int i = 0; i < forces.Length; i++) {
            forcesMagnitud[i] = forces[i].sqrMagnitude;
        }

        float[] normalizedForcesMagnitud = Normalizar(forcesMagnitud);

        float objRadius = selectedBody.GetComponent<SharedSettings>().getRadius();

        for (int i = 0; i < forces.Length; i++) {

            forceArrows[i].transform.localScale = (normalizedForcesMagnitud[i] * Vector3.one + Vector3.one * 0.2f);
            forceArrows[i].transform.position = selectedBody.transform.position + forces[i].normalized * objRadius;
            forceArrows[i].transform.up = forces[i].normalized;
        }
    }

    private void UpdateGizmo()
    {
        Physics.SyncTransforms();
        gizmos.transform.position = selectedBody.transform.position;
    }

    public void SelectBody(GameObject newBody, bool isDoubleSelected) {
        Deselect();

        selectedBody = newBody;
        selectedCelestialBody = selectedBody.GetComponent<CelestialBody>();

        if (showVelocity) displayVelocityVector();
        if (showForces) displayForceVectors();
        if (showGizmo) displayGizmo();
        if (showOrbit) displayOrbit();


        this.isDoubleSelected = isDoubleSelected;
        if (isDoubleSelected) {
            cameraController.orbitObject(selectedBody);
        }

        selectionModePanel.SetActive(true);
    }
    public void toggleOrbit() {
        if (showOrbit) {
            showOrbit = false;
            orbitLine.Clear();
            return;
        }
        displayOrbit();
        showOrbit = true;
    }

    public void displayOrbit() {
        orbitLine.bodyId = selectedCelestialBody.id;
        orbitLine.DrawOrbit();
    }

    public void toggleVelocityArrow() {

        if (showVelocity) {
            showVelocity = false;
            velocityArrow.SetActive(false);
            return;
        }

        displayVelocityVector();
        showVelocity = true;
    }
    public void displayVelocityVector() {
        float objRadius = selectedBody.GetComponent<SharedSettings>().getRadius();

        velocityArrow.transform.localScale = Vector3.one * objRadius * 0.6f;
        velocityArrow.SetActive(true);
    }

    public void toggleForceVectors() {

        if (showForces) {
            showForces = false;
            if (forceArrows != null) {
                foreach (var arrow in forceArrows)
                {
                    Destroy(arrow);
                }
            }
            return;
        }
        displayForceVectors();
        showForces = true;
    }

    private void displayForceVectors() {
        Vector3[] forces = simulator.getForceVectors(selectedCelestialBody);
        Color[] colors = GenerateRandomColors(forces.Length);
        forceArrows = new GameObject[forces.Length];

        for (int i = 0; i < forces.Length; i++) {

            GameObject arrow = new GameObject("Force Arrow " + i);
            ArrowGenerator arrowGen = arrow.AddComponent<ArrowGenerator>();
            arrowGen.setSettings(1, colors[i]);

            arrow.transform.localScale = forces[i].sqrMagnitude * Vector3.one;
            arrow.transform.position = selectedBody.transform.position;
            arrow.transform.up = forces[i].normalized;

            forceArrows[i] = arrow;
        }
    }

    private Color[] GenerateRandomColors(int number)
    {
        Color[] colors = new Color[number];

        for (int i = 0; i < number; i++)
        {
            float hue = (float)i / number; // distribución uniforme
            colors[i] = Color.HSVToRGB(hue, 1f, 1f);
        }

        return colors;
    }

    public void toggleGizmo()
    {
        if (showGizmo) {
            showGizmo = false;
            gizmos.SetActive(false);
            return;
        }
        displayGizmo();
        showGizmo = true;
    }

    private void displayGizmo() {
        Physics.SyncTransforms();
        gizmos.SetActive(true);
        gizmos.transform.position = selectedBody.transform.position;

        float objRadius = selectedBody.GetComponent<SharedSettings>().getRadius();

        gizmoManager.scale = objRadius;
        gizmoManager.Generate();
    }

    public void dragObjectWithGizmo(ArrowGenerator arrow) {
        gizmoManager.dragObject(arrow, selectedBody);
    }

    float[] Normalizar(float[] valores)
    {
        float min = valores.Min();
        float max = valores.Max();

        float[] resultado = new float[valores.Length];

        // Evitar división por 0
        if (max - min == 0)
        {
            for (int i = 0; i < valores.Length; i++)
                resultado[i] = 0f;

            return resultado;
        }

        for (int i = 0; i < valores.Length; i++)
        {
            resultado[i] = (valores[i] - min) / (max - min);
        }

        return resultado;
    }

    public void changeToEditMode() {
        editPanel.SetActive(true);
        startEditPanel.SetActive(false);
        simulator.PauseSimulation();
    }

    public void changeToSimulationMode() {
        editPanel.SetActive(false);
        startEditPanel.SetActive(true);
        simulator.ResumeSimulation();
    }

    public void Deselect() {
        selectedBody = null;
        isDoubleSelected = false;
        orbitLine.Clear();
        velocityArrow.SetActive(false);
        if (forceArrows != null) {
            foreach (var arrow in forceArrows)
            {
                Destroy(arrow);
            }
        }
        selectionModePanel.SetActive(false);
        gizmos.SetActive(false);
    }

    enum SelectionMode
    {
        Orbit,
        VelocityVector,
        ForcesVector,
        Gizmo
    }
    
}
