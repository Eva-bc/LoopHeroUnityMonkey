using UnityEngine;

public class DialogueCell : Cell
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData completedDialogue;

    [Header("Completion Requirement")]
    [SerializeField] private bool requiresBanana = false;

    [Header("Visual Feedback")]
    [SerializeField] private Animator cellAnimator;
    [SerializeField] private ParticleSystem celebrationEffect;

    private bool hasBeenActivatedBefore = false;

    public override void Activate(Pawn CurrentPawn)
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
            return;
        }

        bool shouldShowCompletedDialogue = false;

        if (requiresBanana && GameStateManager.Instance != null)
        {
            shouldShowCompletedDialogue = GameStateManager.Instance.HasBanana();
        }

        DialogueData dialogueToShow = shouldShowCompletedDialogue ? completedDialogue : initialDialogue;

        if (dialogueToShow != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueToShow, OnDialogueComplete);
        }

        hasBeenActivatedBefore = true;
    }

    private void OnDialogueComplete()
    {
        if (requiresBanana && GameStateManager.Instance != null && GameStateManager.Instance.HasBanana())
        {
            if (celebrationEffect != null)
                celebrationEffect.Play();

            if (cellAnimator != null)
                cellAnimator.SetTrigger("Celebrate");

            Debug.Log("Johnny Kiki is pleased! Quest complete!");
        }
    }
}
