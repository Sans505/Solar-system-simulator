using UnityEngine;

public class ShaderLoader : MonoBehaviour
{
    [SerializeField] private NBodySimulation simulator;
    void Start()
    {
        foreach (GameObject body in simulator.bodyList) 
        {
            Planet planet = body.GetComponent<Planet>();
            Sun sun = body.GetComponent<Sun>();
            if (planet != null)
            {
                planet.GeneratePlanet();
            } else if (sun != null)
            {
                sun.GenerateSun();
            }
        }
    }

}