using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineaDePuntos : MonoBehaviour
{
    public int steps = 10000;
    public int pointInterval = 100;
    public float distanciaEntrePuntos = 1f;

    public int bodyId;
    private NBodySimulation simulator;

    private LineRenderer lr;

    void Awake()
    {
        simulator = GetComponentInParent<NBodySimulation>();
    }
    void Start()
    {
        Vector3 [][] bodiesPositions = simulator.trajectoryForecast(steps);
        Vector3 [] thisBodyPositions = bodiesPositions[bodyId];

        lr = GetComponent<LineRenderer>();

        // Crear material automáticamente
        Material mat = new Material(Shader.Find("Sprites/Default"));
        lr.material = mat;

        // Ajustes de la línea
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        int pointNumber = steps / pointInterval;
        lr.positionCount = pointNumber;

        for (int i = 0; i < pointNumber; i++)
        {
            lr.SetPosition(i, thisBodyPositions[i * pointInterval]);
        }
    }
}