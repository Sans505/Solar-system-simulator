using UnityEngine;

public class GridFollow : MonoBehaviour
{
    [Tooltip("Asigna aquí tu cámara principal")]
    public Transform cameraTransform;
    
    // Guardamos la altura inicial del plano para que no se mueva en el eje Y
    private float initialY;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        initialY = transform.position.y;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Copiamos la posición X y Z de la cámara, manteniendo el plano en su altura Y original
        Vector3 newPosition = new Vector3(cameraTransform.position.x, initialY, cameraTransform.position.z);
        transform.position = newPosition;
    }
}