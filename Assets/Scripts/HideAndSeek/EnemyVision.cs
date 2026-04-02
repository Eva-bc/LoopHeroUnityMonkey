using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Detects the player using a 3D cone of vision with Physics.Raycast to account for obstacles.
    /// </summary>
    public class EnemyVision : MonoBehaviour
    {
        [Header("Vision Cone")]
        [SerializeField] private float visionRange = 10f;
        [SerializeField] [Range(10f, 180f)] private float visionAngle = 60f;
        [SerializeField] private LayerMask obstructionMask;

        private Transform _playerTransform;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _playerTransform = player.transform;
        }

        /// <summary>
        /// Returns true if the player is in range, within the vision angle, and not blocked by an obstacle.
        /// </summary>
        public bool CanSeePlayer()
        {
            if (_playerTransform == null) return false;

            // Eye position offset — roughly at Johnny Kiki's head height
            Vector3 eyePosition = transform.position + Vector3.up * 1.6f;
            Vector3 playerChestPosition = _playerTransform.position + Vector3.up * 0.5f;

            Vector3 directionToPlayer = playerChestPosition - eyePosition;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > visionRange) return false;

            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle > visionAngle * 0.5f) return false;

            // Linecast to check for obstacles between eye and player
            if (Physics.Linecast(eyePosition, playerChestPosition, obstructionMask))
                return false;

            return true;
        }

        /// <summary>
        /// Draws the vision cone in the Scene view for debugging.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Vector3 eyePos = transform.position + Vector3.up * 1.6f;
            Gizmos.color = Color.yellow;

            Vector3 leftBound  = Quaternion.Euler(0, -visionAngle * 0.5f, 0) * transform.forward * visionRange;
            Vector3 rightBound = Quaternion.Euler(0,  visionAngle * 0.5f, 0) * transform.forward * visionRange;

            Gizmos.DrawLine(eyePos, eyePos + leftBound);
            Gizmos.DrawLine(eyePos, eyePos + rightBound);
            Gizmos.DrawLine(eyePos, eyePos + transform.forward * visionRange);
        }
    }
}
