using UnityEngine;

/// <summary>
/// Placed in LoopHeroScene. On Start, reads the Minigame result
/// from <see cref="MinigameSessionData"/> and applies the corresponding reward.
/// </summary>
public class MinigameResultHandler : MonoBehaviour
{
    [Header("Session Data")]
    [SerializeField] private MinigameSessionData sessionData;

    [Header("Reward")]
    [SerializeField] private int victoryReputationGain = 15;

    private void Start()
    {
        if (sessionData == null || sessionData.lastResult == MinigameSessionData.Result.None)
            return;

        if (sessionData.lastResult == MinigameSessionData.Result.Victory)
            ApplyVictory();

        // Reset so it doesn't fire again on the next Start
        sessionData.lastResult = MinigameSessionData.Result.None;
    }

    private void ApplyVictory()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.AddReputation(victoryReputationGain);

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification($"🍌 Tu as collecté les 5 bananes ! +{victoryReputationGain} réputation");

        Debug.Log("[MinigameResultHandler] Victory applied.");
    }
}
