using UnityEngine;

public class BananaCell : Cell
{
    [Header("Banana Settings")]
    [SerializeField] private GameObject bananaVisual;
    [SerializeField] private Animator bananaAnimator;
    [SerializeField] private ParticleSystem collectEffect;

    private bool isCollected = false;

    public override void Activate(Pawn CurrentPawn)
    {
        if (isCollected)
            return;

        if (GameStateManager.Instance != null && !GameStateManager.Instance.IsBananaQuestActive())
        {
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowNotification("🍌 Cette banane appartient à quelqu'un...\nPeut-être que Johnny Kiki sait quelque chose ?");
            }
            return;
        }

        CollectBanana();
    }

    private void CollectBanana()
    {
        isCollected = true;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CollectBanana();
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification("🍌 Banane collectée ! 🍌\nRetournez voir Johnny Kiki !");
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCollectSound();
        }

        if (bananaAnimator != null)
        {
            bananaAnimator.SetTrigger("Collect");
        }

        if (collectEffect != null)
        {
            collectEffect.Play();
        }

        if (bananaVisual != null)
        {
            Invoke(nameof(HideBanana), 1f);
        }

        Debug.Log("Banana collected! Return to Johnny Kiki!");
    }

    private void HideBanana()
    {
        if (bananaVisual != null)
            bananaVisual.SetActive(false);
    }
}
