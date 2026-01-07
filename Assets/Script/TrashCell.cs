using UnityEngine;

public class TrashCell : Cell
{
    [Header("Trash Settings")]
    [SerializeField] private int reputationLoss = 10;
    [SerializeField] private GameObject trashVisual;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private Animator trashAnimator;

    private bool hasBeenCollected = false;

    public override void Activate(Pawn CurrentPawn)
    {
        if (hasBeenCollected)
        {
            Debug.Log("Ce sac poubelle a déjà été ramassé.");
            return;
        }

        CollectTrash();
    }

    private void CollectTrash()
    {
        hasBeenCollected = true;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.RemoveReputation(reputationLoss);
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification($"🗑️ Sac poubelle ramassé !\nRéputation -{reputationLoss}");
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCollectSound();
        }

        if (collectEffect != null)
        {
            collectEffect.Play();
        }

        if (trashAnimator != null)
        {
            trashAnimator.SetTrigger("Collect");
        }

        if (trashVisual != null)
        {
            Invoke(nameof(HideTrash), 1f);
        }
    }

    private void HideTrash()
    {
        if (trashVisual != null)
        {
            trashVisual.SetActive(false);
        }
    }
}
