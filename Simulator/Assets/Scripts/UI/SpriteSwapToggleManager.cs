using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapToggleManager : MonoBehaviour
{
    private Toggle toggle;
    private Image image;

    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite stopIcon;



    private void Start()
    {
        toggle = GetComponent<Toggle>();
        image = toggle.targetGraphic as Image;

        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(toggle.isOn); // aplicar estado inicial
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            image.sprite = playIcon;
        }
        else
        {
            image.sprite = stopIcon;
        }
    }
}