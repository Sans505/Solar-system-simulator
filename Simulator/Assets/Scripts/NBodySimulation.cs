using UnityEngine;
using System.Linq;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] bodies;

    void Awake ()
    {
        bodies = FindObjectsOfType<CelestialBody>()
            .OrderBy(o => o.id)
            .ToArray();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdateVelocity(bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(Universe.physicsTimeStep);
        }
    }

    public Vector3 [][] trajectoryForecast(int steps)
    {
        VirtualBody [] virtualBodies = new VirtualBody[bodies.Length];
        Vector3 [][] bodiesFuturePositions = new Vector3[bodies.Length][];

        //Instanciar los virtual bodies
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i] = new VirtualBody (bodies[i]);
            bodiesFuturePositions[i] = new Vector3[steps];
        }

        for (int i = 0; i < steps; i++)
        {
            // Calcular velocidades de los cuerpos
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                virtualBodies[j].velocity = calculateVirtualVelocity(virtualBodies[j], virtualBodies, Universe.physicsTimeStep);
            }
            //Calcular nuevas posiciones de los cuerpos
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                Vector3 newPosition = calculateVirtualPosition(virtualBodies[j], Universe.physicsTimeStep);
                virtualBodies[j].position = newPosition;
                bodiesFuturePositions[j][i] = newPosition;
            }
        }
        return bodiesFuturePositions;
    }

    Vector3 calculateVirtualVelocity(VirtualBody thisBody, VirtualBody[] allBodies, float timeStep)
    {
        Vector3 vel = thisBody.velocity;
        Vector3 pos = thisBody.position;
        float mass = thisBody.mass;
        Vector3 newVelocity = vel;
        foreach (var otherBody in allBodies)
        {
            if (otherBody != thisBody)
            {
                // Calcula la distancia entre los dos cuerpos
                float sqrDst = (otherBody.position - pos).sqrMagnitude;
                // Calcula el vector normalizado de la fuerza resultante
                Vector3 forceDir = (otherBody.position - pos).normalized;
                // Calcula el vector de fuerza utilizando la fórmula universal de gravedad
                Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
                Vector3 acceleration = force / mass;
                newVelocity += acceleration * timeStep;
            }   
        }
        return newVelocity;
    }

    Vector3 calculateVirtualPosition(VirtualBody body, float timeStep)
    {
        
        return body.position + body.velocity * timeStep;
    }

    class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody (CelestialBody body) {
            position = body.transform.position;
            velocity = body.initialVelocity;
            mass = body.mass;
        }
    }
}
