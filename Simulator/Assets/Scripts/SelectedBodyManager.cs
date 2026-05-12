using UnityEngine;
using System.Linq;

public class SelectedBodyManager : MonoBehaviour
{
    public GameObject selectedBody;
    public CelestialBody selectedCelestialBody;

    [SerializeField]
    private NBodySimulation simulator;
    [SerializeField]
    private OrbitLine orbitLine;
    [SerializeField]
    private GameObject velocityArrow;
    [SerializeField]
    private CameraController cameraController;

    private GameObject[] forceArrows;
    private bool isDoubleSelected;

    void Update() {
        if (selectedBody) {
            velocityArrow.transform.position = selectedBody.transform.position;
            velocityArrow.transform.up = selectedCelestialBody.velocity.normalized;
            
            
            Vector3[] forces = simulator.getForceVectors(selectedCelestialBody);

            float[] forcesMagnitud = new float[forces.Length];

            for (int i = 0; i < forces.Length; i++) {
                forcesMagnitud[i] = forces[i].sqrMagnitude;
            }

            float[] normalizedForcesMagnitud = Normalizar(forcesMagnitud);

            float objRadius = 0f;
            Planet planet = selectedBody.GetComponent<Planet>();
            Sun sun = selectedBody.GetComponent<Sun>();
            if (planet) {
                objRadius = planet.shapeSettings.planetRadius;
            } else if (sun) {
                objRadius = sun.sunSettings.sunRadius;
            }

            for (int i = 0; i < forces.Length; i++) {

                forceArrows[i].transform.localScale = (normalizedForcesMagnitud[i] * Vector3.one + Vector3.one) * objRadius;
                forceArrows[i].transform.position = selectedBody.transform.position;
                forceArrows[i].transform.up = forces[i].normalized;
            }
        }
    }

    public void SelectBody(GameObject newBody, bool isDoubleSelected) {
        Deselect();

        selectedBody = newBody;
        selectedCelestialBody = selectedBody.GetComponent<CelestialBody>();
        orbitLine.bodyId = selectedCelestialBody.id;

        orbitLine.DrawOrbit();
        Debug.Log("Entro111");
        showVelocityArrow();
        showForceVectors();

        this.isDoubleSelected = isDoubleSelected;
        if (isDoubleSelected) {
            cameraController.orbitObject(selectedBody);
        }
    }

    private void showVelocityArrow() {

        float objRadius = 0f;
        Planet planet = selectedBody.GetComponent<Planet>();
        Sun sun = selectedBody.GetComponent<Sun>();
        if (planet) {
            objRadius = planet.shapeSettings.planetRadius;
        } else if (sun) {
            objRadius = sun.sunSettings.sunRadius;
        }

        velocityArrow.transform.localScale = Vector3.one * objRadius * 0.6f;
        velocityArrow.SetActive(true);
    }

    private void showForceVectors() {

        Vector3[] forces = simulator.getForceVectors(selectedCelestialBody);
        Color[] colors = GenerateRandomColors(forces.Length);
        forceArrows = new GameObject[forces.Length];
        Debug.Log("Entro");

        for (int i = 0; i < forces.Length; i++) {

            GameObject arrow = new GameObject("Force Arrow " + i);
            ArrowGenerator arrowGen = arrow.AddComponent<ArrowGenerator>();
            arrowGen.setSettings(1, colors[i]);

            arrow.transform.localScale = forces[i].sqrMagnitude * Vector3.one;
            arrow.transform.position = selectedBody.transform.position;
            arrow.transform.up = forces[i].normalized;

            Debug.Log("aaaa " + i);
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
    }
    
}
