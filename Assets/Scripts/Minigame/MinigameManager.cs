using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame
{
    /// <summary>
    /// Central manager for the banana collection mini-game.
    /// Tracks how many bananas have been collected, plays audio feedback,
    /// fires UI events, and opens the door when all 5 are gathered.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager Instance { get; private set; }

        private const int    TotalBananas    = 5;
        private const string LoopHeroScene   = "LoopHeroScene";

        [Header("Door")]
        [SerializeField] private Transform door;
        [SerializeField] private Vector3   openPositionOffset = new Vector3(0f, 4f, 0f);
        [SerializeField] private float     doorOpenSpeed      = 2.5f;

        [Header("Audio")]
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private AudioClip allCollectedSound;

        [Header("Session")]
        [SerializeField] private MinigameSessionData sessionData;
        [SerializeField] private float returnDelay = 3f;

        private AudioSource _audioSource;
        private int         _collectedCount;
        private bool        _doorOpen;
        private Vector3     _doorOpenPosition;

        /// <summary>Fired each time a banana is collected. Arg = current collected count.</summary>
        public static event Action<int> OnBananaCollectedEvent;

        /// <summary>Fired once all bananas are collected.</summary>
        public static event Action OnAllBananasCollected;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance     = this;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (door != null)
                _doorOpenPosition = door.position + openPositionOffset;
        }

        private void Update()
        {
            if (!_doorOpen || door == null) return;
            door.position = Vector3.MoveTowards(door.position, _doorOpenPosition, doorOpenSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Called by <see cref="MinigameBananaPickup"/> each time a banana is collected.
        /// </summary>
        public void OnBananaCollected()
        {
            _collectedCount++;
            PlaySound(collectSound);
            OnBananaCollectedEvent?.Invoke(_collectedCount);

            if (_collectedCount >= TotalBananas)
                OpenDoor();
        }

        /// <summary>Returns the number of bananas collected so far.</summary>
        public int CollectedCount => _collectedCount;

        /// <summary>Returns the total number of bananas to collect.</summary>
        public int Total => TotalBananas;

        private void OpenDoor()
        {
            _doorOpen = true;
            PlaySound(allCollectedSound);
            OnAllBananasCollected?.Invoke();

            sessionData?.RecordVictory();
            StartCoroutine(ReturnToLoopHero());
        }

        private IEnumerator ReturnToLoopHero()
        {
            yield return new WaitForSeconds(returnDelay);
            SceneManager.LoadScene(LoopHeroScene);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
                _audioSource.PlayOneShot(clip);
        }
    }
}
