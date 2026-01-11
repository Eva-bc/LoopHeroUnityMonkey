using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueWithChoices", menuName = "Loop Hero/Dialogue With Choices Data")]
public class DialogueWithChoicesData : ScriptableObject
{
    [System.Serializable]
    public class Choice
    {
        public string choiceText;
        public bool isAccept;
    }

    [TextArea(2, 5)]
    public string speakerName;

    [TextArea(3, 6)]
    public string questionText;

    public Choice[] choices;
}