using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Marks the banana object as a collectible trigger item in 3D.
    /// Requires a Collider set as trigger and the "Banana" tag.
    /// Bobs vertically and rotates slowly for visibility.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BananaPickup : MonoBehaviour
    {
        [Header("Bob")]
        [SerializeField] private float bobAmplitude = 0.15f;
        [SerializeField] private float bobFrequency = 2f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 90f;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
            gameObject.tag = "Banana";

            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            float yOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.position = _startPosition + new Vector3(0f, yOffset, 0f);
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
