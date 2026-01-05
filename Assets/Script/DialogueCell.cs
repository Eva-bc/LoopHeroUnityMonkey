using UnityEngine;

public class DialogueCell : Cell
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData completedDialogue;

    [Header("Completion Requirement")]
    [SerializeField] private bool requiresBanana = false;

    [Header("Visual Feedback")]
    [SerializeField] private Animator characterAnimator;
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

        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StartTalk");
        }

        DialogueData dialogueToShow = shouldShowCompletedDialogue ? completedDialogue : initialDialogue;

        if (dialogueToShow != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueToShow, OnDialogueComplete);
        }

        // Démarrer la quête de la banane si c'est le premier dialogue
        if (!shouldShowCompletedDialogue && requiresBanana && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.StartBananaQuest();
        }

        hasBeenActivatedBefore = true;
    }

    private void OnDialogueComplete()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }

        if (requiresBanana && GameStateManager.Instance != null && GameStateManager.Instance.HasBanana())
        {
            if (celebrationEffect != null)
                celebrationEffect.Play();

            if (characterAnimator != null)
                characterAnimator.SetTrigger("StartDance");

            Debug.Log("Johnny Kiki is pleased! Quest complete!");
        }
    }
}
