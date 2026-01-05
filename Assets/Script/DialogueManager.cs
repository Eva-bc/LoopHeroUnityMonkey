using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Visual Effect")]
    [SerializeField] private ParticleSystem dialogueCompleteEffect;
    [SerializeField] private Button diceButton;

    private DialogueData currentDialogueData;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    private System.Action onDialogueCompleteCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void StartDialogue(DialogueData dialogueData, System.Action onComplete = null)
    {
        if (dialogueData == null || dialogueData.dialogueLines.Length == 0)
        {
            Debug.LogWarning("No dialogue data to display!");
            return;
        }

        currentDialogueData = dialogueData;
        currentLineIndex = 0;
        onDialogueCompleteCallback = onComplete;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);

        if (diceButton != null)
            diceButton.interactable = false;

        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        if (currentLineIndex >= currentDialogueData.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueData.DialogueLine line = currentDialogueData.dialogueLines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        dialogueText.text = line.text;
    }

    private void OnContinueClicked()
    {
        if (!isDialogueActive)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonClick();
        }

        currentLineIndex++;
        DisplayCurrentLine();
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (dialogueCompleteEffect != null)
            dialogueCompleteEffect.Play();

        if (diceButton != null)
            diceButton.interactable = true;

        onDialogueCompleteCallback?.Invoke();

        currentDialogueData = null;
        onDialogueCompleteCallback = null;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
