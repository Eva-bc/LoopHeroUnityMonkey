using UnityEngine;

/// <summary>
/// ScriptableObject used to persist the HideAndSeek mini-game result
/// across scene loads (LoopHeroScene → HideAndSeekScene → LoopHeroScene).
/// </summary>
[CreateAssetMenu(fileName = "HideAndSeekSessionData", menuName = "Scriptable Objects/HideAndSeekSessionData")]
public class HideAndSeekSessionData : ScriptableObject
{
    public enum Result { None, Victory, Defeat }

    [Tooltip("Set before loading HideAndSeekScene; read on return to LoopHeroScene.")]
    public Result lastResult = Result.None;

    /// <summary>Resets the result before starting a new session.</summary>
    public void StartSession() => lastResult = Result.None;

    /// <summary>Records victory before returning to LoopHeroScene.</summary>
    public void RecordVictory() => lastResult = Result.Victory;

    /// <summary>Records defeat before returning to LoopHeroScene.</summary>
    public void RecordDefeat() => lastResult = Result.Defeat;
}
