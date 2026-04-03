using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Smoothly follows the target (player) while maintaining a fixed offset.
    /// The offset is computed automatically from the camera's position relative to the target at startup.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Smoothing")]
        [SerializeField] private float smoothSpeed = 8f;

        private Vector3 _offset;

        private void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                    target = player.transform;
            }

            if (target != null)
                _offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
