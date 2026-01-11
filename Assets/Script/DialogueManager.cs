using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    [Header("Choice UI")]
    [SerializeField] private GameObject choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Visual Effect")]
    [SerializeField] private ParticleSystem dialogueCompleteEffect;
    [SerializeField] private Button diceButton;

    private DialogueData currentDialogueData;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    private System.Action onDialogueCompleteCallback;
    private System.Action<bool> onChoiceMadeCallback;

    private List<GameObject> currentChoiceButtons = new List<GameObject>();

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

        if (choicesContainer != null)
            choicesContainer.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogueData, System.Action onComplete = null, int startLineIndex = 0)
    {
        if (dialogueData == null || dialogueData.dialogueLines.Length == 0)
        {
            Debug.LogWarning("No dialogue data to display!");
            return;
        }

        currentDialogueData = dialogueData;
        currentLineIndex = Mathf.Clamp(startLineIndex, 0, dialogueData.dialogueLines.Length - 1);
        onDialogueCompleteCallback = onComplete;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);

        if (diceButton != null)
            diceButton.interactable = false;

        if (choicesContainer != null)
            choicesContainer.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(true);

        DisplayCurrentLine();
    }

    public void ShowDialogueWithChoices(DialogueWithChoicesData choicesData, System.Action<bool> onChoiceMade)
    {
        if (choicesData == null)
        {
            Debug.LogWarning("No choices data to display!");
            return;
        }

        isDialogueActive = true;
        onChoiceMadeCallback = onChoiceMade;

        dialoguePanel.SetActive(true);

        if (diceButton != null)
            diceButton.interactable = false;

        speakerNameText.text = choicesData.speakerName;
        dialogueText.text = choicesData.questionText;

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        ShowChoices(choicesData.choices);
    }

    private void ShowChoices(DialogueWithChoicesData.Choice[] choices)
    {
        ClearChoiceButtons();

        if (choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogError("Choices container or button prefab not assigned!");
            return;
        }

        choicesContainer.SetActive(true);

        foreach (DialogueWithChoicesData.Choice choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            currentChoiceButtons.Add(buttonObj);

            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = choice.choiceText;
            }

            bool isAccept = choice.isAccept;
            if (button != null)
            {
                button.onClick.AddListener(() => OnChoiceSelected(isAccept));
            }
        }
    }

    private void OnChoiceSelected(bool accepted)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonClick();
        }

        ClearChoiceButtons();

        if (choicesContainer != null)
            choicesContainer.SetActive(false);

        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (diceButton != null)
            diceButton.interactable = true;

        onChoiceMadeCallback?.Invoke(accepted);
        onChoiceMadeCallback = null;
    }

    private void ClearChoiceButtons()
    {
        foreach (GameObject button in currentChoiceButtons)
        {
            Destroy(button);
        }
        currentChoiceButtons.Clear();
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

    public int GetCurrentLineIndex()
    {
        return currentLineIndex;
    }
}