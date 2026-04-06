using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Central game state manager for the HideAndSeek mini-game.
    /// Handles win/lose conditions, notifies the UI, and writes the result
    /// into HideAndSeekSessionData before returning to LoopHeroScene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsGameOver { get; private set; }

        [Header("Session Data")]
        [SerializeField] private HideAndSeekSessionData sessionData;

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
        /// Freezes the player, records the victory, then delegates the
        /// timed transition to the UI manager.
        /// </summary>
        public void OnBananaCollected()
        {
            if (IsGameOver) return;
            IsGameOver = true;

            // Freeze player immediately so they can't move during the victory screen
            PlayerController player = FindFirstObjectByType<PlayerController>();
            player?.Die();

            sessionData?.RecordVictory();
            _uiManager?.ShowVictory();
        }

        /// <summary>
        /// Called when the enemy reaches and attacks the player.
        /// </summary>
        public void OnPlayerCaught()
        {
            if (IsGameOver) return;
            IsGameOver = true;

            sessionData?.RecordDefeat();

            PlayerController player = FindFirstObjectByType<PlayerController>();
            player?.Die();

            _uiManager?.ShowGameOver();
        }
    }
}
