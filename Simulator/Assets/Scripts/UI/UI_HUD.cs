using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour {

//		[SerializeField]TextMeshProUGUI panel;
	[SerializeField]Text panel;
	[Range(0f, 1f)]
	public float marginPercent = 0.2f; // 20%
    [SerializeField] private float minSize = 100f;

	public Renderer rend;
	public GameObject target;
	Camera cam;
	RectTransform rect;
	private SharedSettings targetSharedSettings;

	public void Init(GameObject t)
    {
        target = t;
        cam = Camera.main;
        rect = GetComponent<RectTransform>();

        if (panel != null)
            panel.text = target.name;
		
		targetSharedSettings = t.GetComponent<SharedSettings>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

		float radius = targetSharedSettings.getRadius();

        Vector3 center = target.transform.position;

        // Convertir a pantalla
        Vector3 screenCenter = cam.WorldToScreenPoint(center);

        // Calcular radio en pantalla
        Vector3 screenRight = cam.WorldToScreenPoint(center + cam.transform.right * radius);
        Vector3 screenUp = cam.WorldToScreenPoint(center + cam.transform.up * radius);

        float radiusX = Mathf.Abs(screenRight.x - screenCenter.x);
        float radiusY = Mathf.Abs(screenUp.y - screenCenter.y);

        float marginMultiplier = 1f + marginPercent;

		float width = radiusX * 2f * marginMultiplier;
		float height = radiusY * 2f * marginMultiplier;

        width = Mathf.Max(width, minSize);
        height = Mathf.Max(height, minSize);

        // POSICIÓN (centro del rect)
        rect.position = new Vector2(screenCenter.x, screenCenter.y);

        // TAMAÑO
        rect.sizeDelta = new Vector2(width, height);
    }
}
