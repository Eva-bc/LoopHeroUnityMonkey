using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("Quest State")]
    public bool hasBanana = false;
    public bool bananaQuestActive = false;
    public bool isGameOver = false;

    [Header("Reputation System")]
    [SerializeField] private int startingReputation = 50;
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int minReputation = 0;
    [SerializeField] private int reputationThreshold = 20;

    private int currentReputation;

    public int CurrentReputation => currentReputation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentReputation = startingReputation;
    }

    public void StartBananaQuest()
    {
        bananaQuestActive = true;
        Debug.Log("Banana quest started! Go find the banana!");
    }

    public bool IsBananaQuestActive()
    {
        return bananaQuestActive;
    }

    public void CollectBanana()
    {
        hasBanana = true;
        Debug.Log("Banana collected!");
    }

    public bool HasBanana()
    {
        return hasBanana;
    }

    public void SetGameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over - Victory!");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void AddReputation(int amount)
    {
        currentReputation = Mathf.Clamp(currentReputation + amount, minReputation, maxReputation);
        Debug.Log($"Réputation +{amount} ? {currentReputation}/{maxReputation}");

        if (ReputationManager.Instance != null)
        {
            ReputationManager.Instance.UpdateReputationUI();
        }

        CheckReputationGameOver();
    }

    public void RemoveReputation(int amount)
    {
        currentReputation = Mathf.Clamp(currentReputation - amount, minReputation, maxReputation);
        Debug.Log($"Réputation -{amount} ? {currentReputation}/{maxReputation}");

        if (ReputationManager.Instance != null)
        {
            ReputationManager.Instance.UpdateReputationUI();
        }

        CheckReputationGameOver();
    }

    private void CheckReputationGameOver()
    {
        if (currentReputation <= reputationThreshold)
        {
            Debug.LogWarning("?? Réputation critique ! Tu risques d'être expulsé de l'île !");
        }

        if (currentReputation <= minReputation)
        {
            TriggerReputationGameOver();
        }
    }

    private void TriggerReputationGameOver()
    {
        Debug.Log("Game Over - Réputation trop basse !");
        isGameOver = true;

        if (VictoryManager.Instance != null)
        {
            VictoryManager.Instance.ShowGameOver("?? GAME OVER\n\nTa réputation est trop basse...\nTu as été expulsé de l'île !");
        }
    }
}
