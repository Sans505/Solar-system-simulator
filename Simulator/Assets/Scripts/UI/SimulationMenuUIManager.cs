using UnityEngine;
using UnityEngine.UI;

public class SimulationMenuUIManager : MonoBehaviour
{
    [SerializeField] private NBodySimulation simulator;

    public void StartOrStopSimulation() {
        if (simulator.isSimulating) {
            simulator.StopSimulation();
        } else {
            simulator.StartSimulation();
        }
    }
}
