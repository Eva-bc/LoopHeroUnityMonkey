using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Pawn _pawn;

    public void RollTheDice()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            Debug.Log("Cannot roll dice while dialogue is active!");
            return;
        }

        int value = Random.Range(1, 7);
        Debug.Log($"Le dé à fait {value}");
        _pawn.TryMoving(value);
    }
}
