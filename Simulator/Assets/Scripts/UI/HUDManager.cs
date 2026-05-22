using UnityEngine;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private NBodySimulation simulator;
    [SerializeField] private Camera cam;

    public float renderMinDistance = 10f;
    public GameObject HUDPrefab;
    private List<GameObject> HUDList;
    private bool isEnabled = true;

    public void CreateHUDs()
    {
        if (HUDList != null) {
            foreach (GameObject h in HUDList) {
                Destroy(h);
            }
        }

        HUDList = new List<GameObject>();

        foreach (GameObject body in simulator.bodyList) {
            GameObject h = Instantiate (HUDPrefab, transform);
            h.GetComponent<UI_HUD>().Init(body);
            HUDList.Add(h);
        }
    }

    void Update() {
        if (!isEnabled) return;
        if (HUDList == null) return;

        foreach (GameObject h in HUDList) {

            GameObject target = h.GetComponent<UI_HUD>().target;
            Vector3 center = target.transform.position;
            Vector3 screenCenter = cam.WorldToScreenPoint(center);
            float distance = Vector3.Distance(center, cam.transform.position);

            if (distance > renderMinDistance && screenCenter.z > 0) {
                h.SetActive(true);
            } else {
                h.SetActive(false);
            }
        }
    }

    public void Disable() {
        foreach (GameObject h in HUDList) {
            h.SetActive(false);
        }
        isEnabled = false;
    }

    public void Enable() {
        foreach (GameObject h in HUDList) {
            h.SetActive(true);
        }
        isEnabled = true;
    }
}
