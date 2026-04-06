using UnityEngine;
using UnityEngine.InputSystem;

namespace HideAndSeek
{
    /// <summary>
    /// Moves the player cube in 3D using the new Input System.
    /// Camera-relative movement on the XZ plane.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private Transform cameraTransform;

        private CharacterController _characterController;
        private Vector2 _moveInput;
        private float _verticalVelocity;
        private bool _isAlive = true;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            // Always resolve the main camera — the serialized field is a fallback override
            if (cameraTransform == null)
                cameraTransform = Camera.main != null ? Camera.main.transform : transform;
        }

        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        private void Update()
        {
            if (!_isAlive) return;
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            // Camera-relative movement on XZ plane
            Vector3 forward = cameraTransform.forward;
            Vector3 right   = cameraTransform.right;
            forward.y = 0f;
            right.y   = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;

            // Gravity
            if (_characterController.isGrounded)
                _verticalVelocity = -2f;   // small negative keeps the controller grounded
            else
                _verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = moveDirection * moveSpeed + Vector3.up * _verticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);

            // Rotate toward movement direction
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
            }
        }

        /// <summary>
        /// Freezes the player on game over or victory.
        /// </summary>
        public void Die()
        {
            _isAlive   = false;
            _moveInput = Vector2.zero;
        }
    }
}
