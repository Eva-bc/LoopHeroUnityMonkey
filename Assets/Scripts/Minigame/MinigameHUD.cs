using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame
{
    /// <summary>
    /// Drives the in-game HUD for the banana collection mini-game.
    /// Displays the objective label, a banana icon, and the live collect counter.
    /// </summary>
    public class MinigameHUD : MonoBehaviour
    {
        [Header("Objective")]
        [SerializeField] private TextMeshProUGUI objectiveLabel;

        [Header("Counter")]
        [SerializeField] private Image    bananaIcon;
        [SerializeField] private TextMeshProUGUI counterLabel;

        [Header("Completion")]
        [SerializeField] private TextMeshProUGUI completionLabel;

        private const string ObjectiveText  = "Collecte les 5 bananes !";
        private const string CompletionText = "Bravo ! La porte est ouverte !";

        private void OnEnable()
        {
            MinigameManager.OnBananaCollectedEvent += HandleBananaCollected;
            MinigameManager.OnAllBananasCollected  += HandleAllCollected;
        }

        private void OnDisable()
        {
            MinigameManager.OnBananaCollectedEvent -= HandleBananaCollected;
            MinigameManager.OnAllBananasCollected  -= HandleAllCollected;
        }

        private void Start()
        {
            if (objectiveLabel  != null) objectiveLabel.text  = ObjectiveText;
            if (completionLabel != null) completionLabel.gameObject.SetActive(false);
            RefreshCounter(0);
        }

        private void HandleBananaCollected(int count)
        {
            RefreshCounter(count);
        }

        private void HandleAllCollected()
        {
            if (completionLabel != null)
                completionLabel.gameObject.SetActive(true);
        }

        /// <summary>Updates the banana counter label (e.g. "3 / 5").</summary>
        private void RefreshCounter(int count)
        {
            if (counterLabel != null)
                counterLabel.text = $"{count} / {MinigameManager.Instance?.Total ?? 5}";
        }
    }
}
