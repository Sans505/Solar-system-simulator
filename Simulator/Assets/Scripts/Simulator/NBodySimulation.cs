using UnityEngine;
using System.Linq;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] bodies;

    public float timeScale = 1;

    void Awake ()
    {
        bodies = FindObjectsOfType<CelestialBody>()
            .OrderBy(o => o.id)
            .ToArray();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        Time.timeScale = timeScale;
    }

    [ContextMenu("Cambiar tiempo")]
    private void changrTimeButton() {
        changeTimeScale(timeScale);
    }
    public void changeTimeScale(float newTimeScale) {
        Time.timeScale = newTimeScale;
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
    // Funcion que calcula las posiciones futuras que seguiran todos los cuerpos
    // Return
    // - Las posiciones de cada cuerpo
    // - Los cuerpos virtuales con las propiedades del ultimo step
    public (Vector3[][], VirtualBody[]) trajectoryForecast(int steps)
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
        return (bodiesFuturePositions, virtualBodies);
    }

    public (Vector3[][], VirtualBody[]) trajectoryForecast(int steps, VirtualBody[] startBodies)
    {
        VirtualBody [] virtualBodies = startBodies;
        Vector3 [][] bodiesFuturePositions = new Vector3[startBodies.Length][];

        //Instanciar los virtual bodies
        for (int i = 0; i < virtualBodies.Length; i++) {
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
        return (bodiesFuturePositions, virtualBodies);
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

    public Vector3[] getForceVectors(CelestialBody thisBody) {
        Vector3[] forces = new Vector3[bodies.Length - 1];
        int i = 0;
        foreach (var otherBody in bodies)
        {
            if (otherBody != thisBody)
            {
                // Calcula la distancia entre los dos cuerpos
                float sqrDst = (otherBody.transform.position - thisBody.transform.position).sqrMagnitude;
                // Calcula el vector normalizado de la fuerza resultante
                Vector3 forceDir = (otherBody.transform.position - thisBody.transform.position).normalized;
                // Calcula el vector de fuerza utilizando la fórmula universal de gravedad
                forces[i] = forceDir * Universe.gravitationalConstant * thisBody.mass * otherBody.mass / sqrDst;
                i++;
            }   
        }
        return forces;
    }

    public class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody (CelestialBody body) {
            position = body.transform.position;
            velocity = body.velocity;
            mass = body.mass;
        }
    }
}
