using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BodyEntry : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Color selectedColor;

    public string name;
    private BodyListMenuManager bodyListManager;

    private void Reset() {
        textField = GetComponentInChildren<TMP_Text>();
    }

    public void Setup(Sprite sprite, string text, BodyListMenuManager blm) {
        icon.sprite = sprite;
        icon.color = Color.white;
        textField.SetText(text);
        textField.color = Color.white;
        name = text;
        bodyListManager = blm;
    }

    public void OnPointerClick(PointerEventData eventData) {

        bool doubleClick = eventData.clickCount == 2;
        bodyListManager.SelectEntry(this, doubleClick); 

    }

    public void Select() {
        icon.color = selectedColor;
        textField.color = selectedColor;
    }
    public void DeSelect() {
        icon.color = Color.white;
        textField.color = Color.white;
    }
}
