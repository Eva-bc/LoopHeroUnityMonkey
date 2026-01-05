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

        // Vérifier si la quête est active
        if (GameStateManager.Instance != null && !GameStateManager.Instance.IsBananaQuestActive())
        {
            // Montrer un message si le joueur essaie de prendre la banane sans avoir parlé à Johnny Kiki
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowNotification("🍌 Cette banane appartient à quelqu'un...");
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
