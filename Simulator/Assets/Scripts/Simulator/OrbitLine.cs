using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    public int steps = 10000;
    public int pointInterval = 100;

    public int bodyId;
    [SerializeField]
    private NBodySimulation simulator;
    private NBodySimulation.VirtualBody[] lastStepBodies;
    public int lastBodyPositionsStepCount = 100;
    private Vector3[] lastBodyPositions;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        //Material mat = new Material(Shader.Find("Sprites/Default"));
        //lr.material = mat;

        // Ajustes de la línea
        lr.startWidth = 0.25f;
        lr.endWidth = 0.25f;
    }

    private int framesElapsed = 0;

    void FixedUpdate() {
        
        if (lr.positionCount <= 1) {
            return;
        }
        if (framesElapsed < pointInterval - 1) {
            framesElapsed++;
            return;
        }
        framesElapsed = 0;
        if (lastBodyPositions != null)
        {
            UpdateOrbit();
        }
    }

    public void DrawOrbit()
    {
        var simulation1 = simulator.trajectoryForecast(steps);

        Vector3 [][] bodiesPositions = simulation1.Item1;
        NBodySimulation.VirtualBody[] vt = simulation1.Item2;

        Vector3 [] thisBodyPositions = bodiesPositions[bodyId];

        var simulation2 = simulator.trajectoryForecast(lastBodyPositionsStepCount, vt);

        Vector3 [][] lastBodiesPositions = simulation2.Item1;
        lastStepBodies = simulation2.Item2;

        lastBodyPositions = lastBodiesPositions[bodyId];

        Clear();
        int pointNumber = (int) Mathf.Floor(steps / pointInterval);
        lr.positionCount = pointNumber;

        for (int i = 0; i < pointNumber; i++)
        {
            lr.SetPosition(i, thisBodyPositions[i * pointInterval]);
        }
    }

    private int lastBodyIndex = 0;

    public void UpdateOrbit() {

        Vector3[] positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        Vector3[] newPositions = new Vector3[lr.positionCount];

        for (int i = 1; i < lr.positionCount; i++)
        {
            newPositions[i - 1] = positions[i];
        }

        if (lastBodyIndex * pointInterval >= lastBodyPositionsStepCount) {
            var simulation = simulator.trajectoryForecast(lastBodyPositionsStepCount, lastStepBodies);

            Vector3 [][] lastBodiesPositions = simulation.Item1;
            lastStepBodies = simulation.Item2;

            lastBodyPositions = lastBodiesPositions[bodyId];
            lastBodyIndex = 0;
        }

        newPositions[newPositions.Length - 1] = lastBodyPositions[lastBodyIndex * pointInterval];
        lastBodyIndex++;

        lr.positionCount = newPositions.Length;
        lr.SetPositions(newPositions);
    }

    public void Clear() {
        framesElapsed = 0;
        lr.positionCount = 0;
        lastBodyIndex = 0;
    }
}