using TMPro;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    public static MessageHandler Instance { get; protected set; }

    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject errorNotificationPrefab;
    [SerializeField] private Transform notificationParent;
    private GameObject notification;
    private bool fade;

    protected virtual void Awake()
    {
        Instance = this;
    }

    /**
	 * Use this to show a confirmation that the case was saved successfully
	 */
    public void ShowMessage(string message, bool error)
    {
        if (notification != null)
            Destroy(notification);
        if (error)
            notification = Instantiate(errorNotificationPrefab, notificationParent);
        else
            notification = Instantiate(notificationPrefab, notificationParent);

        CancelInvoke("Fade");
        fade = false;
        notification.GetComponent<CanvasGroup>().alpha = 1;
        notification.SetActive(true);
        TextMeshProUGUI messageText = notification.transform.Find("BG/Message").GetComponent<TextMeshProUGUI>();
        messageText.text = message;
        Invoke("Fade", 5f);
        NextFrame.Function(delegate { fade = true; });

    }

    /**
	 * Used temporarily as a delay for the save confirmation message
	 */
    private void Fade()
    {
        if (notification.GetComponent<CanvasGroup>().alpha > 0) {
            if (!fade) {
                return;
            }
            //t.color = new Color (t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime / 6f);
            notification.GetComponent<CanvasGroup>().alpha = (notification.GetComponent<CanvasGroup>().alpha - Time.deltaTime / 3f);
            NextFrame.Function(Fade);
            return;
        }
        Destroy(notification);
    }
}
