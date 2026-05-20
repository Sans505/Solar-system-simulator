using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    public int id;
    public float mass { get; private set; }
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public Vector3 initialVelocity;
    public Vector3 initialAngularVelocity;
    public Vector3 velocity { get; private set; }
    public Vector3 acceleration { get; private set; }
    public bool destroyAfterSimulation = false;
    Rigidbody rb;
    
    void Awake()
    {
        gameObject.tag = "Selectable";
        velocity = initialVelocity;
        rb = GetComponent<Rigidbody> ();
        mass = rb.mass;
        rb.useGravity = false;
        rb.angularVelocity = initialAngularVelocity;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb.angularDamping = 0f;
    }

    public void Reset() {
        velocity = initialVelocity;
        rb.angularVelocity = initialAngularVelocity;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public void RegisterInitialTransform() {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    //public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    //{
    //    foreach (var otherBody in allBodies)
    //    {
    //        if (otherBody != this)
    //        {
    //            // Calcula la distancia entre los dos cuerpos
    //            float sqrDst = (otherBody.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).sqrMagnitude;
    //            // Calcula el vector normalizado de la fuerza resultante
    //            Vector3 forceDir = (otherBody.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).normalized;
    //            // Calcula el vector de fuerza utilizando la fórmula universal de gravedad
    //            Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
    //            Vector3 acceleration = force / mass;
    //            velocity += acceleration * timeStep;
    //        }   
    //    }
    //}

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        velocity = calculateVelocity(velocity, rb.position, allBodies, timeStep);
    }

    public Vector3 calculateVelocity(Vector3 vel, Vector3 pos, CelestialBody[] allBodies, float timeStep)
    {
        Vector3 newVelocity = vel;
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                // Calcula la distancia entre los dos cuerpos
                float sqrDst = (otherBody.GetComponent<Rigidbody>().position - pos).sqrMagnitude;
                // Calcula el vector normalizado de la fuerza resultante
                Vector3 forceDir = (otherBody.GetComponent<Rigidbody>().position - pos).normalized;
                // Calcula el vector de fuerza utilizando la fórmula universal de gravedad
                Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
                acceleration = force / mass;
                newVelocity += acceleration * timeStep;
            }   
        }
        return newVelocity;
    }

    //public void UpdatePosition (float timeStep) {
    //    //Debug.Log(velocity);
    //    rb.MovePosition(rb.position + velocity * timeStep);
    //}

    public void UpdatePosition (float timeStep)
    {
        rb.MovePosition(calculatePosition(rb.position, velocity, timeStep));
    }

    public Vector3 calculatePosition(Vector3 pos, Vector3 vel, float timeStep)
    {
        return pos + vel * timeStep;
    }
}
