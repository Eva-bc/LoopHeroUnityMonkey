using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Board cell that launches the HideAndSeek mini-game.
/// Triggers a scene transition to HideAndSeekScene when the player lands on it.
/// </summary>
public class HideAndSeekCell : Cell
{
    private const string HideAndSeekSceneName = "HideAndSeekScene";

    [Header("Session Data")]
    [SerializeField] private HideAndSeekSessionData sessionData;

    [Header("Optional Notification")]
    [SerializeField] private string activationMessage = "Johnny Kiki te surveille... récupère la banane sans te faire repérer !";

    public override void Activate(Pawn currentPawn)
    {
        if (sessionData == null)
        {
            Debug.LogError("HideAndSeekCell: sessionData is not assigned!");
            return;
        }

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification(activationMessage);

        sessionData.StartSession();
        SceneManager.LoadScene(HideAndSeekSceneName);
    }
}
