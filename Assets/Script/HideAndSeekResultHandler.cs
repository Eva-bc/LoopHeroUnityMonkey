using UnityEngine;

/// <summary>
/// Placed in LoopHeroScene. On Start, reads the HideAndSeek mini-game result
/// from HideAndSeekSessionData and applies the corresponding reward or penalty.
/// </summary>
public class HideAndSeekResultHandler : MonoBehaviour
{
    [Header("Session Data")]
    [SerializeField] private HideAndSeekSessionData sessionData;

    [Header("Consequences")]
    [SerializeField] private int victoryReputationGain = 10;
    [SerializeField] private int defeatReputationLoss  = 10;

    private void Start()
    {
        if (sessionData == null || sessionData.lastResult == HideAndSeekSessionData.Result.None)
            return;

        switch (sessionData.lastResult)
        {
            case HideAndSeekSessionData.Result.Victory:
                ApplyVictory();
                break;

            case HideAndSeekSessionData.Result.Defeat:
                ApplyDefeat();
                break;
        }

        // Reset so it doesn't fire again on the next Start
        sessionData.lastResult = HideAndSeekSessionData.Result.None;
    }

    private void ApplyVictory()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CollectBanana();
            GameStateManager.Instance.AddReputation(victoryReputationGain);
        }

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification($"🍌 Tu as récupéré la banane ! +{victoryReputationGain} réputation");

        Debug.Log("[HideAndSeekResultHandler] Victory applied.");
    }

    private void ApplyDefeat()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.RemoveReputation(defeatReputationLoss);

        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification($"😤 Johnny Kiki t'a attrapé ! -{defeatReputationLoss} réputation");

        Debug.Log("[HideAndSeekResultHandler] Defeat applied.");
    }
}
