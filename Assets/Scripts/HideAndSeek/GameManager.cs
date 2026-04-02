using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Central game state manager. Handles win/lose conditions and notifies the UI.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsGameOver { get; private set; }

        private HideAndSeekUIManager _uiManager;

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
            _uiManager = FindFirstObjectByType<HideAndSeekUIManager>();
        }

        /// <summary>
        /// Called when the player successfully collects the banana.
        /// </summary>
        public void OnBananaCollected()
        {
            if (IsGameOver) return;
            IsGameOver = true;
            _uiManager?.ShowVictory();
        }

        /// <summary>
        /// Called when the enemy reaches and attacks the player.
        /// </summary>
        public void OnPlayerCaught()
        {
            if (IsGameOver) return;
            IsGameOver = true;

            PlayerController player = FindFirstObjectByType<PlayerController>();
            player?.Die();

            _uiManager?.ShowGameOver();
        }
    }
}
