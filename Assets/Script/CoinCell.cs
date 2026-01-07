using UnityEngine;

public class CoinCell : Cell
{
    [Header("Coin Settings")]
    [SerializeField] private int reputationGain = 15;
    [SerializeField] private GameObject coinVisual;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private Animator coinAnimator;

    private bool hasBeenCollected = false;

    public override void Activate(Pawn CurrentPawn)
    {
        if (hasBeenCollected)
        {
            Debug.Log("Cette pièce a déjà été ramassée.");
            return;
        }

        CollectCoin();
    }

    private void CollectCoin()
    {
        hasBeenCollected = true;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.AddReputation(reputationGain);
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification($"💰 Pièce ramassée !\nRéputation +{reputationGain}");
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCollectSound();
        }

        if (collectEffect != null)
        {
            collectEffect.Play();
        }

        if (coinAnimator != null)
        {
            coinAnimator.SetTrigger("Collect");
        }

        if (coinVisual != null)
        {
            Invoke(nameof(HideCoin), 1f);
        }
    }

    private void HideCoin()
    {
        if (coinVisual != null)
        {
            coinVisual.SetActive(false);
        }
    }
}
