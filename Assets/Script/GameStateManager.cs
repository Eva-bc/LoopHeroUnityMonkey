using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public bool hasBanana = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
}
