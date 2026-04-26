using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Board cell that launches the banana collection mini-game.
/// Triggers a scene transition to Minigame when the player lands on it.
/// </summary>
public class MinigameCell : Cell
{
    private const string MinigameSceneName = "Minigame";

    [Header("Session Data")]
    [SerializeField] private MinigameSessionData sessionData;

    [Header("Notification")]
    [SerializeField] private string activationMessage = "Collecte les 5 bananes pour ouvrir la porte !";

    public override void Activate(Pawn currentPawn)
    {
        if (sessionData == null)
        {
            Debug.LogError("MinigameCell: sessionData is not assigned!");
            return;
        }

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification(activationMessage);

        sessionData.StartSession();
        SceneManager.LoadScene(MinigameSceneName);
    }
}
