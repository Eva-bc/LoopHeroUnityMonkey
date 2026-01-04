using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Loop Hero/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)]
        public string speakerName;

        [TextArea(3, 6)]
        public string text;
    }

    public DialogueLine[] dialogueLines;
}
