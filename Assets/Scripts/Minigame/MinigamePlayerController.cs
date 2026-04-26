using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigame
{
    /// <summary>
    /// Simple character controller for the mini-game player.
    /// Camera-relative movement on the XZ plane using the new Input System.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class MinigamePlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float gravity = -20f;

        private CharacterController _characterController;
        private Transform           _cameraTransform;
        private Vector2             _moveInput;
        private float               _verticalVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraTransform     = Camera.main != null ? Camera.main.transform : transform;
        }

        /// <summary>Input System callback — bound automatically via Player Input component.</summary>
        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        private void Update()
        {
            Vector3 forward = _cameraTransform.forward;
            Vector3 right   = _cameraTransform.right;
            forward.y = 0f;
            right.y   = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;

            if (_characterController.isGrounded)
                _verticalVelocity = -2f;
            else
                _verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = moveDirection * moveSpeed + Vector3.up * _verticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
            }
        }
    }
}
