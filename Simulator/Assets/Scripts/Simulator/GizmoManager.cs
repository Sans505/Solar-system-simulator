using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    GameObject axisX;
    GameObject axisY;
    GameObject axisZ;
    GameObject directionArrow;

    ArrowGenerator axisXGenerator;
    ArrowGenerator axisYGenerator;
    ArrowGenerator axisZGenerator;
    ArrowGenerator directionArrowGenerator;

    public float scale;

    void Start()
    {
        Generate();
    }

    void OnValidate() {
        Generate();
    }

    private void Generate () {

        Transform axisXT = transform.Find("AxisX");
        Transform axisYT = transform.Find("AxisY");
        Transform axisZT = transform.Find("AxisZ");
        Transform directionArrowT = transform.Find("Direction Arrow");


        if (!axisXT) {
            axisX = new GameObject("AxisX");
            axisX.transform.parent = transform;
            axisX.AddComponent<ArrowGenerator>();
        } else {
            axisX = axisXT.gameObject;
        }
        if (!axisYT) {
            axisY = new GameObject("AxisY");
            axisY.transform.parent = transform;
            axisY.AddComponent<ArrowGenerator>();
        } else {
            axisY = axisYT.gameObject;
        }
        if (!axisZT) {
            axisZ = new GameObject("AxisZ");
            axisZ.transform.parent = transform;
            axisZ.AddComponent<ArrowGenerator>();
        } else {
            axisZ = axisZT.gameObject;
        }
        if (!directionArrowT) {
            directionArrow = new GameObject("Direction Arrow");
            directionArrow.transform.parent = transform;
            directionArrow.AddComponent<ArrowGenerator>();
        } else {
            directionArrow = directionArrowT.gameObject;
        }
        
        axisXGenerator = axisX.GetComponent<ArrowGenerator>();
        axisYGenerator = axisY.GetComponent<ArrowGenerator>();
        axisZGenerator = axisZ.GetComponent<ArrowGenerator>();
        directionArrowGenerator = directionArrow.GetComponent<ArrowGenerator>();

        axisX.transform.localRotation = Quaternion.Euler(90, 90, 0);
        axisY.transform.localRotation = Quaternion.Euler(0, 0, 0);
        axisZ.transform.localRotation = Quaternion.Euler(0, 90, 90);

        axisXGenerator.setSettings(scale, Color.red);
        axisYGenerator.setSettings(scale, Color.green);
        axisZGenerator.setSettings(scale, Color.blue);
        directionArrowGenerator.setSettings(scale, Color.white);
        directionArrowGenerator.stemLength = directionArrowGenerator.stemLength * 1.55f;

        axisXGenerator.GenerateArrow();
        axisYGenerator.GenerateArrow();
        axisZGenerator.GenerateArrow();
        directionArrowGenerator.GenerateArrow();
        directionArrow.SetActive(false);
        showDirectionArrow();

    }

    public void showDirectionArrow() {
        directionArrow.SetActive(true);
        Vector3 dir = new Vector3(1,1,1).normalized;
        Debug.Log(dir);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        directionArrow.transform.rotation = rot;
    }
}
