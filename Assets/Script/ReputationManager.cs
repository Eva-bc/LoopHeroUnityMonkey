using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private Slider reputationBar;

    [Header("Visual Settings")]
    [SerializeField] private Color highReputationColor = Color.green;
    [SerializeField] private Color mediumReputationColor = Color.yellow;
    [SerializeField] private Color lowReputationColor = Color.red;
    [SerializeField] private Image reputationBarFill;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateReputationUI();
    }

    public void UpdateReputationUI()
    {
        if (GameStateManager.Instance == null)
            return;

        int currentRep = GameStateManager.Instance.CurrentReputation;

        if (reputationText != null)
        {
            reputationText.text = $"Réputation: {currentRep}";
        }

        if (reputationBar != null)
        {
            reputationBar.value = currentRep;
        }

        UpdateBarColor(currentRep);
    }

    private void UpdateBarColor(int reputation)
    {
        if (reputationBarFill == null)
            return;

        if (reputation >= 70)
        {
            reputationBarFill.color = highReputationColor;
        }
        else if (reputation >= 40)
        {
            reputationBarFill.color = mediumReputationColor;
        }
        else
        {
            reputationBarFill.color = lowReputationColor;
        }
    }
}
