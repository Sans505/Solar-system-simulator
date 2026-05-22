using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    [Header("Notification UI")]
    [SerializeField] private GameObject notificationParent;
    [SerializeField] private TMP_Text notificationTextUI;
    [SerializeField] private Image notificationImageUI;

    [SerializeField] private Sprite infoIcon;
    [SerializeField] private Sprite warningIcon;
    [SerializeField] private Sprite errorIcon;

    private CanvasGroup notificationUICanvasGroup;
    private Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
    private bool isDisplayingNotification = false;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        notificationUICanvasGroup = notificationParent.GetComponent<CanvasGroup>();
        notificationUICanvasGroup.alpha = 0;
    }

    public void ShowNotification(string msg, NotificationType notificationType) {
        NotificationData data = new NotificationData();

        data.message = msg;
        data.displayDuration = 2f;
        data.fadeDuration = 1f;

        switch (notificationType)
        {
            case NotificationType.Info:
                data.icon = infoIcon;
                data.color = Color.white;
                break;

            case NotificationType.Warning:
                data.icon = warningIcon;
                data.color = Color.yellow;
                break;

            case NotificationType.Error:
                data.icon = errorIcon;
                data.color = Color.red;
                break;
        }

        notificationQueue.Enqueue(data);
        if (!isDisplayingNotification) {
            StartCoroutine(DisplayNotification());
        }
    }

    private IEnumerator DisplayNotification() {
        
        isDisplayingNotification = true;
        while (notificationQueue.Count > 0) {
            NotificationData data = notificationQueue.Dequeue();

            // Apply SO data
            notificationTextUI.text = data.message;
            notificationTextUI.color = data.color;
            notificationImageUI.sprite = data.icon;
            notificationImageUI.color = data.color;

            // Fade in
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, true, data.fadeDuration));

            // Display
            yield return new WaitForSecondsRealtime(data.displayDuration);

            // Fade out
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, false, data.fadeDuration));
        }
        isDisplayingNotification = false;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn, float duration) {
        float targetAlpha = fadeIn ? 1f : 0f;
        float initialAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    public class NotificationData {
        public string message;
        public Sprite icon;
        public Color color;
        public float displayDuration;
        public float fadeDuration;
    }   
}

public enum NotificationType
{
    Info,
    Warning,
    Error
}
