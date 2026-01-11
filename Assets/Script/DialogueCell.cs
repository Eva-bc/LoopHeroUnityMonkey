using UnityEngine;

public class DialogueCell : Cell
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueWithChoicesData questChoiceDialogue;
    [SerializeField] private DialogueData completedDialogue;
    [SerializeField] private DialogueData refusalDialogue;

    [Header("Completion Requirement")]
    [SerializeField] private bool requiresBanana = false;

    [Header("Visual Feedback")]
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private ParticleSystem celebrationEffect;

    private int savedLineIndex = 0;
    private bool hasCompletedInitialDialogue = false;
    private bool hasAskedQuest = false;

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

        if (shouldShowCompletedDialogue)
        {
            if (completedDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(completedDialogue, OnDialogueComplete, 0);
            }
        }
        else if (!hasCompletedInitialDialogue && initialDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(initialDialogue, OnInitialDialogueComplete, savedLineIndex);
        }
        else if (!hasAskedQuest && questChoiceDialogue != null)
        {
            ShowQuestChoice();
        }
    }

    private void OnInitialDialogueComplete()
    {
        savedLineIndex = DialogueManager.Instance.GetCurrentLineIndex();

        if (savedLineIndex >= initialDialogue.dialogueLines.Length)
        {
            hasCompletedInitialDialogue = true;
            savedLineIndex = initialDialogue.dialogueLines.Length - 1;

            if (questChoiceDialogue != null)
            {
                ShowQuestChoice();
            }
        }

        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }
    }

    private void ShowQuestChoice()
    {
        if (DialogueManager.Instance != null && questChoiceDialogue != null)
        {
            DialogueManager.Instance.ShowDialogueWithChoices(questChoiceDialogue, OnQuestChoiceMade);
        }
    }

    private void OnQuestChoiceMade(bool accepted)
    {
        hasAskedQuest = true;

        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }

        if (accepted)
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.StartBananaQuest();
            }
            Debug.Log("Quête acceptée ! Va chercher la banane !");
        }
        else
        {
            Debug.Log("Quête refusée ! Game Over !");

            if (refusalDialogue != null && DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(refusalDialogue, OnRefusalDialogueComplete, 0);
            }
            else
            {
                TriggerGameOverForRefusal();
            }
        }
    }

    private void OnRefusalDialogueComplete()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }

        TriggerGameOverForRefusal();
    }

    private void TriggerGameOverForRefusal()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetGameOver();
        }

        if (VictoryManager.Instance != null)
        {
            VictoryManager.Instance.ShowGameOver(" GAME OVER !");
        }
    }

    private void OnDialogueComplete()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StopTalk");
        }

        bool isCompletedDialogue = requiresBanana && GameStateManager.Instance != null && GameStateManager.Instance.HasBanana();

        if (isCompletedDialogue)
        {
            if (celebrationEffect != null)
                celebrationEffect.Play();

            if (characterAnimator != null)
                characterAnimator.SetTrigger("StartDancing");

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