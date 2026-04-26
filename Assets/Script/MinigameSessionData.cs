using UnityEngine;

/// <summary>
/// ScriptableObject used to persist the Minigame result
/// across scene loads (LoopHeroScene → Minigame → LoopHeroScene).
/// </summary>
[CreateAssetMenu(fileName = "MinigameSessionData", menuName = "Scriptable Objects/MinigameSessionData")]
public class MinigameSessionData : ScriptableObject
{
    public enum Result { None, Victory }

    [Tooltip("Set before loading Minigame; read on return to LoopHeroScene.")]
    public Result lastResult = Result.None;

    /// <summary>Resets the result before starting a new session.</summary>
    public void StartSession() => lastResult = Result.None;

    /// <summary>Records victory before returning to LoopHeroScene.</summary>
    public void RecordVictory() => lastResult = Result.Victory;
}
