using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI notificationText;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (notificationText != null)
            notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (notificationText == null)
        {
            Debug.LogWarning("Notification text is not assigned!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        notificationText.gameObject.SetActive(false);
    }
}
