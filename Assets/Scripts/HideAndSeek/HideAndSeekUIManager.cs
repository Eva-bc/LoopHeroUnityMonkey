using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HideAndSeek
{
    /// <summary>
    /// Manages the in-game UI panels for the HideAndSeek mini-game.
    ///
    /// Victory  : fade-in → hold <victoryHoldDuration>s → load LoopHeroScene (automatic).
    /// Game Over: fade-in → player clicks Restart → reload HideAndSeekScene.
    /// </summary>
    public class HideAndSeekUIManager : MonoBehaviour
    {
        private const string MainSceneName    = "LoopHeroScene";
        private const string MiniGameSceneName = "HideAndSeekScene";

        [Header("Panels")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        [Header("Game Over button")]
        [SerializeField] private Button restartButton;

        [Header("Timing")]
        [Tooltip("Seconds the victory panel stays visible before returning to LoopHeroScene.")]
        [SerializeField] private float victoryHoldDuration = 2.5f;
        [SerializeField] private float fadeDuration        = 0.6f;

        private void Awake()
        {
            // Hide panels and reset their CanvasGroups
            InitPanel(gameOverPanel);
            InitPanel(victoryPanel);

            // Game Over → restart the mini-game
            restartButton?.onClick.AddListener(ReloadMiniGame);
        }

        /// <summary>Fades in the Game Over panel. Restart button reloads the mini-game.</summary>
        public void ShowGameOver()
        {
            StartCoroutine(FadeInPanel(gameOverPanel));
        }

        /// <summary>Fades in the Victory panel then automatically returns to LoopHeroScene.</summary>
        public void ShowVictory()
        {
            StartCoroutine(VictorySequence());
        }

        // ── Sequences ────────────────────────────────────────────────────────────

        private IEnumerator VictorySequence()
        {
            yield return FadeInPanel(victoryPanel);
            yield return new WaitForSeconds(victoryHoldDuration);
            SceneManager.LoadScene(MainSceneName);
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        /// <summary>Resets a panel to its hidden state (inactive, alpha 0, no raycasts).</summary>
        private static void InitPanel(GameObject panel)
        {
            if (panel == null) return;
            panel.SetActive(false);

            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null) return;
            cg.alpha          = 0f;
            cg.blocksRaycasts = false;
            cg.interactable   = false;
        }

        private IEnumerator FadeInPanel(GameObject panel)
        {
            if (panel == null) yield break;

            panel.SetActive(true);

            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null) yield break;

            cg.alpha          = 0f;
            cg.blocksRaycasts = false;
            cg.interactable   = false;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed  += Time.deltaTime;
                cg.alpha  = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }

            cg.alpha          = 1f;
            cg.blocksRaycasts = true;
            cg.interactable   = true;
        }

        private static void ReloadMiniGame()
        {
            SceneManager.LoadScene(MiniGameSceneName);
        }
    }
}
