using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private OutlineSelection outlineSelection;

    private bool draggingObject;
    private GameObject moveAxisGO;
    private Vector3 moveAxis;
    private GameObject selectedObject;

    public float scale;
    public float translateSensitivity = 1.0f; 

    void Start()
    {
        draggingObject = false;
        Generate();
    }

    void Update() {
        if (!draggingObject) return;
        
        var mouse = Mouse.current;
        if (mouse.leftButton.isPressed) {
            // Dirección en mundo del eje (X, Y o Z) //selectedObject.transform.TransformDirection(moveAxis)
            //Vector3 worldDirection = moveAxis.normalized;
//
            //// Posición del objeto en pantalla
            //Vector3 objectScreenPos = camera.WorldToScreenPoint(selectedObject.transform.position);
//
            //// Dirección del eje en pantalla
            //Vector3 axisScreenPos = camera.WorldToScreenPoint(selectedObject.transform.position + worldDirection);
//
            //Vector2 screenDirection = (axisScreenPos - objectScreenPos);
            //screenDirection.Normalize();
//
            //// Movimiento del ratón
            //Vector2 mouseMovement = Mouse.current.delta.ReadValue();
//
            //// Proyección del movimiento del mouse sobre el eje
            //float movement = Vector2.Dot(mouseMovement, screenDirection);
//
            //// Mover en ESPACIO MUNDO
            //selectedObject.transform.position += worldDirection * movement * translateSensitivity;

            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Punto base (donde empieza el eje)
            Vector3 origin = selectedObject.transform.position;

            // Dirección del eje (GLOBAL)
            Vector3 axis = moveAxis.normalized;

            // Crear un plano perpendicular al eje
            Plane plane = new Plane(camera.transform.forward, origin);

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                // Proyectar el punto sobre el eje
                Vector3 projected = origin + Vector3.Project(hitPoint - origin, axis);

                selectedObject.transform.position = projected - moveAxis * scale * 2f;
            }


        } else {
            Physics.SyncTransforms();
            dropObject();
            draggingObject = false;
        }

    }

    void OnValidate() {
        Generate();
    }

    public void Generate () {

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
        axisX.tag = "Gizmo";
        axisY.tag = "Gizmo";
        axisZ.tag = "Gizmo";
        directionArrow.tag = "Gizmo";
        
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
        //showDirectionArrow();

    }

    public void showDirectionArrow() {
        directionArrow.SetActive(true);
        Vector3 dir = new Vector3(1,1,1).normalized;
        Debug.Log(dir);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        directionArrow.transform.rotation = rot;
    }

    public void dragObject(ArrowGenerator arrowGen, GameObject selection) {

        moveAxisGO = arrowGen.transform.gameObject;

        camera.GetComponent<CameraController>().lookAtTransform = null; // Hacemos que deje de seguirle la camara al objeto
        outlineSelection.Disable();

        selectedObject = selection;
        
        arrowGen.ChangeColorTemporaly(Color.yellow * 1.5f);

        if (arrowGen == axisXGenerator) {
            moveAxis = new Vector3(1, 0, 0);
            axisYGenerator.ChangeColorTemporaly(Color.gray);
            axisZGenerator.ChangeColorTemporaly(Color.gray);
        } else if (arrowGen == axisYGenerator) {
            moveAxis = new Vector3(0, 1, 0);
            axisXGenerator.ChangeColorTemporaly(Color.gray);
            axisZGenerator.ChangeColorTemporaly(Color.gray);
        } else {
            moveAxis = new Vector3(0, 0, 1);
            axisXGenerator.ChangeColorTemporaly(Color.gray);
            axisYGenerator.ChangeColorTemporaly(Color.gray);
        }

        draggingObject = true;
    }

    public void dropObject() {
        axisXGenerator.ResetColor();
        axisYGenerator.ResetColor();
        axisZGenerator.ResetColor();
        outlineSelection.Enable();
    }
}
