using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HideAndSeek
{
    /// <summary>
    /// Manages the in-game UI panels: Game Over and Victory screens.
    /// </summary>
    public class HideAndSeekUIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        [Header("Buttons")]
        [SerializeField] private Button restartButton;
        [SerializeField] private Button victoryRestartButton;

        [Header("Fade")]
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private float fadeDuration = 0.6f;

        private void Awake()
        {
            gameOverPanel?.SetActive(false);
            victoryPanel?.SetActive(false);

            restartButton?.onClick.AddListener(RestartScene);
            victoryRestartButton?.onClick.AddListener(RestartScene);
        }

        /// <summary>
        /// Displays the Game Over panel with a fade-in transition.
        /// </summary>
        public void ShowGameOver()
        {
            StartCoroutine(FadeInPanel(gameOverPanel));
        }

        /// <summary>
        /// Displays the Victory panel with a fade-in transition.
        /// </summary>
        public void ShowVictory()
        {
            StartCoroutine(FadeInPanel(victoryPanel));
        }

        private IEnumerator FadeInPanel(GameObject panel)
        {
            panel?.SetActive(true);

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                float elapsed = 0f;
                while (elapsed < fadeDuration)
                {
                    elapsed += Time.deltaTime;
                    fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                    yield return null;
                }
                fadeCanvasGroup.alpha = 1f;
            }
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
