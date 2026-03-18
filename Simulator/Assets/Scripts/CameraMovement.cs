using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float lookSpeedH = 2f;
    [SerializeField] private float lookSpeedV = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float dragSpeed = 3f;

    private float yaw = 0f;
    private float pitch = 0f;

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void Update()
    {
        // Solo si Left Alt está presionado
        if (Keyboard.current.leftAltKey.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            Vector2 mouseScroll = Mouse.current.scroll.ReadValue();

            // Click izquierdo - Rotar cámara
            if (Mouse.current.leftButton.isPressed)
            {
                yaw += lookSpeedH * mouseDelta.x * Time.deltaTime;
                pitch -= lookSpeedV * mouseDelta.y * Time.deltaTime;

                transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            }

            // Click medio - Arrastrar cámara
            if (Mouse.current.middleButton.isPressed)
            {
                transform.Translate(
                    -mouseDelta.x * dragSpeed * Time.deltaTime,
                    -mouseDelta.y * dragSpeed * Time.deltaTime,
                    0
                );
            }

            // Click derecho - Zoom horizontal
            if (Mouse.current.rightButton.isPressed)
            {
                transform.Translate(
                    0,
                    0,
                    mouseDelta.x * zoomSpeed * 0.07f * Time.deltaTime,
                    Space.Self
                );
            }

            // Rueda del mouse - Zoom
            transform.Translate(
                0,
                0,
                mouseScroll.y * zoomSpeed * 0.01f,
                Space.Self
            );
        }
    }
}