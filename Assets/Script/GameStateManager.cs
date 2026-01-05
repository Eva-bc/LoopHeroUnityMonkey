using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public bool hasBanana = false;
    public bool bananaQuestActive = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
}
