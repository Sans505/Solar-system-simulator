using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectedBodyManager : MonoBehaviour
{

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
    [SerializeField]
    private GameObject gizmoPanel;
    [SerializeField]
    private BodyListMenuManager bodyListManager;
    [SerializeField]
    private GizmoPanelManager gizmoPanelManager;

    public GameObject selectedBody;
    public CelestialBody selectedCelestialBody;
    public bool selectedBodyExists = false;

    private bool showOrbit = false;
    private bool showVelocity = false;
    private bool showForces = false;

    private GizmoManager gizmoManager;
    private GameObject[] forceArrows;
    private bool isDoubleSelected;

    void Update() {
        if (selectedBodyExists) {
            if (selectedBody) {

                if (showVelocity) UpdateVelocityVector();
                if (showForces) UpdateForcesVector();

                Outline outline = selectedBody.GetComponent<Outline>();
                if (outline != null) outline.enabled = true;
            
            } else {
                selectedBodyExists = false;
                NotificationManager.instance.ShowNotification("Error: Se ha perdido la referencia al cuerpo seleccionado", NotificationType.Error);
                Deselect();
            }
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

    public void SelectBody(GameObject newBody, bool isDoubleSelected) {
        if (selectedBody) Deselect();

        selectedBody = newBody;
        selectedCelestialBody = selectedBody.GetComponent<CelestialBody>();

        gizmoPanelManager.displayGizmo();
        if (showVelocity) displayVelocityVector();
        if (showForces) displayForceVectors();
        if (showOrbit) displayOrbit();


        this.isDoubleSelected = isDoubleSelected;
        if (isDoubleSelected) {
            cameraController.orbitObject(selectedBody);
        }

        Outline outline = selectedBody.GetComponent<Outline>();

        if (outline != null)
        {
            outline.enabled = true;
        }
        else
        {
            outline = selectedBody.AddComponent<Outline>();
            outline.enabled = true;
        }

        bodyListManager.UpdateSelectedEntry(selectedBody.name);
        selectionModePanel.SetActive(true);
        selectedBodyExists = true;
    }

    public void SelectBody(string bodyName, bool isDoubleSelected) {
        GameObject selectedBody = simulator.bodyList.Find(x => x.name == bodyName);
        if (selectedBody) {
            SelectBody(selectedBody, isDoubleSelected);
        } else {
            NotificationManager.instance.ShowNotification("Error: Cuerpo no encontrado", NotificationType.Error);
        }
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
        gizmoPanel.SetActive(true);
        startEditPanel.SetActive(false);

        if (simulator.isSimulating) {
            simulator.PauseSimulation();
            NotificationManager.instance.ShowNotification("Simulación pausada", NotificationType.Info);
        }
    }

    public void changeToSimulationMode() {
        gizmoPanelManager.deselectGizmoButton();    
        gizmoPanel.SetActive(false);
        startEditPanel.SetActive(true);
        editPanel.SetActive(false);

        if (simulator.isSimulating) {
            simulator.ResumeSimulation();
            NotificationManager.instance.ShowNotification("Simulación reanudada", NotificationType.Info);
        }
    }
//
    public void refreshOrbit() {
        if (showOrbit) {
            displayOrbit();
        }
    }
    public void ClearOrbit() {
        orbitLine.Clear();
    }

    public void Deselect() {
        Outline outline = selectedBody.GetComponent<Outline>();
        if (outline != null) outline.enabled = false;
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
    
}
