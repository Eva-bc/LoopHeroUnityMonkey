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

    private int savedLineIndex = 0;
    private bool hasCompletedInitialDialogue = false;

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
            int startIndex = shouldShowCompletedDialogue ? 0 : (hasCompletedInitialDialogue ? savedLineIndex : 0);
            DialogueManager.Instance.StartDialogue(dialogueToShow, OnDialogueComplete, startIndex);
        }

        if (!shouldShowCompletedDialogue && requiresBanana && GameStateManager.Instance != null && !hasCompletedInitialDialogue)
        {
            GameStateManager.Instance.StartBananaQuest();
        }
    }

    private void OnDialogueComplete()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }

        bool isCompletedDialogue = requiresBanana && GameStateManager.Instance != null && GameStateManager.Instance.HasBanana();

        if (!isCompletedDialogue)
        {
            savedLineIndex = DialogueManager.Instance.GetCurrentLineIndex();

            if (savedLineIndex >= initialDialogue.dialogueLines.Length)
            {
                hasCompletedInitialDialogue = true;
                savedLineIndex = initialDialogue.dialogueLines.Length - 1;
            }
        }

        if (isCompletedDialogue)
        {
            if (celebrationEffect != null)
                celebrationEffect.Play();

            if (characterAnimator != null)
                characterAnimator.SetTrigger("StartDance");

            Debug.Log("Johnny Kiki is pleased! Quest complete!");

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SetGameOver();
            }

            if (VictoryManager.Instance != null)
            {
                VictoryManager.Instance.ShowVictory();
            }
        }
    }
}
