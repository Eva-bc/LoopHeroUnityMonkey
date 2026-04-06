using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Collectible banana trigger. Notifies GameManager when the Player enters
    /// the trigger zone, then disables itself so it can only be collected once.
    /// Also bobs vertically and rotates slowly for visibility.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BananaPickup : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [Header("Bob")]
        [SerializeField] private float bobAmplitude = 0.15f;
        [SerializeField] private float bobFrequency = 2f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 90f;

        private Vector3 _startPosition;
        private bool    _collected;

        private void Start()
        {
            _startPosition = transform.position;
            gameObject.tag = "Banana";

            GetComponent<Collider>().isTrigger = true;
        }

        private void Update()
        {
            if (_collected) return;

            float yOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = _startPosition + new Vector3(0f, yOffset, 0f);
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        /// <summary>Detects Player contact and triggers the victory sequence.</summary>
        private void OnTriggerEnter(Collider other)
        {
            if (_collected) return;
            if (!other.CompareTag(PlayerTag)) return;

            _collected = true;
            gameObject.SetActive(false);

            GameManager.Instance?.OnBananaCollected();
        }
    }
}
