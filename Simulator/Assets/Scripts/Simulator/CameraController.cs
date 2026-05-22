using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    [SerializeField] private NBodySimulation simulator;
    [SerializeField] private UIHoverManager uiHoverManager;
    public float navigationSpeed = 2.4f;
    public float shiftMultiplier = 2f;
    public float sensitivity = 1.0f;
    public float panSensitivity = 0.5f;
    public float mouseWheelZoomSpeed = 1.0f;
    public float orbitDamping = 10f;
    public float orbitRadius = 5f;
    public float lerpDuration = 6f;
    public float lerpIntensity = 3f;
    public float topViewPadding = 1.2f;

    public Transform lookAtTransform;

    private Camera cam;
    private Vector3 anchorPoint;
    private Quaternion anchorRot;

    private bool isPanning;

    private float pan_x;
    private float pan_y;
    private Vector3 panComplete;

    private float yaw;
    private float pitch;
    private float mouseX;
    private float mouseY;
    private float minOrbitRadius;

    private float time;
    private bool isMovingToOrbit;
    private bool isMovingToPosition;

    private Vector3 originPoint;
    private Vector3 destinationPoint;
    private Quaternion originRotation;
    private Quaternion destinationRotation;

    private void Awake () {
        cam = GetComponent<Camera>();
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update () {

        if (isMovingToOrbit) {
            moveToOrbitObject();
            return;
        }

        if (isMovingToPosition) {
            moveToPosition();
            return;
        }

        var mouse = Mouse.current;
        var keyboard = Keyboard.current;

        if (mouse == null || keyboard == null) return;

        if (lookAtTransform != null) {

            if (keyboard.spaceKey.wasPressedThisFrame) {
                lookAtTransform = null;
                //transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                //transform.rotation = Quaternion.identity;
                //transform.LookAt(lookAtTransform);
                return;
            }
            if (mouse.leftButton.isPressed && !uiHoverManager.isHovering) {
                
                Vector2 lookInput = mouse.delta.ReadValue();

                mouseX = lookInput.x;
                mouseY = lookInput.y;

                yaw += mouseX * sensitivity;
                pitch -= mouseY * sensitivity;

                transform.rotation = Quaternion.Euler(pitch, yaw, 0);
            }

            if (!uiHoverManager.isHovering) orbitRadius -= mouse.scroll.ReadValue().y * sensitivity;
            orbitRadius = Mathf.Max(orbitRadius, minOrbitRadius);
            transform.position = lookAtTransform.position - transform.forward * orbitRadius;
            return;
        }

        if (uiHoverManager.isHovering) return;  //Cursor sobre interfaz

        MousePanning(mouse);

        if (isPanning)
            return;

        //Movimiento
        if (mouse.rightButton.isPressed && !uiHoverManager.isHovering) {
            Vector3 move = Vector3.zero;

            float speed = navigationSpeed *
                          (keyboard.leftShiftKey.isPressed ? shiftMultiplier : 1f) *
                          Time.unscaledDeltaTime * 9.1f;

            if (keyboard.wKey.isPressed)
                move += Vector3.forward * speed;
            if (keyboard.sKey.isPressed)
                move -= Vector3.forward * speed;
            if (keyboard.dKey.isPressed)
                move += Vector3.right * speed;
            if (keyboard.aKey.isPressed)
                move -= Vector3.right * speed;
            if (keyboard.eKey.isPressed)
                move += Vector3.up * speed;
            if (keyboard.qKey.isPressed)
                move -= Vector3.up * speed;

            transform.Translate(move);
        }

        //Inicio rotación
        if (mouse.rightButton.wasPressedThisFrame) {
            Vector2 mousePos = mouse.position.ReadValue();
            anchorPoint = new Vector3(mousePos.y, -mousePos.x);
            anchorRot = transform.rotation;
        }
        //Rotación
        if (mouse.rightButton.isPressed) {
            Quaternion rot = anchorRot;
            Vector2 mousePos = mouse.position.ReadValue();
            Vector3 dif = anchorPoint - new Vector3(mousePos.y, -mousePos.x);
            rot.eulerAngles += dif * sensitivity;
            transform.rotation = rot;
        }

        MouseWheeling(mouse, keyboard);
    }

    //Zoom con rueda
    void MouseWheeling(Mouse mouse, Keyboard keyboard)
    {
        float scroll = mouse.scroll.ReadValue().y;

        float speed = 10 * (mouseWheelZoomSpeed *
                      (keyboard.leftShiftKey.isPressed ? shiftMultiplier : 1f) *
                      Time.unscaledDeltaTime * 9.1f);

        if (scroll < 0)
        {
            transform.position -= transform.forward * speed;
        }
        if (scroll > 0)
        {
            transform.position += transform.forward * speed;
        }
    }

    //Panning con botón central
    void MousePanning(Mouse mouse)
    {
        Vector2 delta = mouse.delta.ReadValue();

        pan_x = -delta.x * panSensitivity * Time.unscaledDeltaTime;
        pan_y = -delta.y * panSensitivity * Time.unscaledDeltaTime;

        panComplete = new Vector3(pan_x, pan_y, 0);

        if (mouse.middleButton.wasPressedThisFrame)
        {
            isPanning = true;
        }

        if (mouse.middleButton.wasReleasedThisFrame)
        {
            isPanning = false;
        }

        if (isPanning)
        {
            transform.Translate(panComplete);
        }
    }

    private void moveToOrbitObject() {
        if (time < lerpDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / lerpDuration;

            t = Mathf.SmoothStep(0f, 1f, Mathf.Pow(t, lerpIntensity));

            destinationPoint = lookAtTransform.position - transform.forward * orbitRadius;
            transform.position = Vector3.Lerp(originPoint, destinationPoint, t);
        } else {
            isMovingToOrbit = false;
        }
    }

    public void SetOrbitRadius(float multiplier)
    {
        if (!lookAtTransform) return;
        float objRadius = lookAtTransform.GetComponent<SharedSettings>().getRadius();
        orbitRadius = objRadius * multiplier;
    }

    public void orbitObject(GameObject obj) {
        lookAtTransform = obj.transform;

        float objRadius = obj.GetComponent<SharedSettings>().getRadius();

        minOrbitRadius = objRadius * 1.3f;
        orbitRadius = objRadius * 4f;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        isMovingToPosition = false;
        isMovingToOrbit = true;
        time = 0f;
        originPoint = transform.position;
    }

    private void moveToPosition() {

        if (time < lerpDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / lerpDuration;

            t = Mathf.SmoothStep(0f, 1f, Mathf.Pow(t, lerpIntensity));

            transform.position = Vector3.Lerp(originPoint, destinationPoint, t);
            transform.rotation = Quaternion.Slerp(originRotation, destinationRotation, t);
        } else {
            isMovingToPosition = false;
        }
    }



    public void moveToTopView() {

        lookAtTransform = null;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        List<GameObject> bodyList = simulator.bodyList;

        if (bodyList.Count == 0) return;

        Bounds bounds = new Bounds(bodyList[0].transform.position, Vector3.zero);

        // Expandir bounds para incluir todos los objetos
        foreach (var b in bodyList)
        {
            bounds.Encapsulate(b.transform.position);
        }

        Vector3 center = bounds.center;
        float size = Mathf.Max(bounds.size.x, bounds.size.z);

        // Asegurar que mira hacia abajo
        originRotation = transform.rotation;
        destinationRotation = Quaternion.Euler(90f, 0f, 0f);

        // Calcular altura necesaria según FOV
        float fov = cam.fieldOfView;
        float distance = size / (2f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));

        distance *= topViewPadding;

        destinationPoint = center + Vector3.up * distance;

        isMovingToOrbit = false;
        isMovingToPosition = true;
        time = 0f;
        originPoint = transform.position;
    }
}