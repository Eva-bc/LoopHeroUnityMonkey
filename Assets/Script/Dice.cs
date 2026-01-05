using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Pawn _pawn;

    public void RollTheDice()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameOver())
        {
            Debug.Log("Game is over! Cannot roll dice.");
            return;
        }

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            Debug.Log("Cannot roll dice while dialogue is active!");
            return;
        }

        int value = Random.Range(1, 7);
        Debug.Log($"Le dé à fait {value}");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayDiceRoll();
        }

        _pawn.TryMoving(value);
    }
}
