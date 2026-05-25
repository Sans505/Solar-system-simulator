using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class BodyDeletionManager : MonoBehaviour
{
    //[SerializeField] private GameObject planet;

    [SerializeField] private SelectedBodyManager selectedBodyManager;
    [SerializeField] private NBodySimulation simulator;
    [SerializeField] private BodyListMenuManager bodyListMenuManager;


    public void Activate() {
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void Delete() {
        simulator.DeleteBody(selectedBodyManager.selectedBody);
        selectedBodyManager.Deselect();
    }

}
