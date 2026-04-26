using UnityEngine;

namespace Minigame
{
    /// <summary>
    /// Attached to each banana collectible in the mini-game.
    /// Bobs and rotates for visibility, then notifies <see cref="MinigameManager"/> on player contact.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class MinigameBananaPickup : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [Header("Bob")]
        [SerializeField] private float bobAmplitude = 0.2f;
        [SerializeField] private float bobFrequency = 2f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 80f;

        private Vector3 _startPosition;
        private bool    _collected;

        private void Start()
        {
            _startPosition = transform.position;
            GetComponent<Collider>().isTrigger = true;
        }

        private void Update()
        {
            if (_collected) return;

            float yOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = _startPosition + new Vector3(0f, yOffset, 0f);
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collected) return;
            if (!other.CompareTag(PlayerTag)) return;

            _collected = true;
            gameObject.SetActive(false);

            MinigameManager.Instance?.OnBananaCollected();
        }
    }
}
