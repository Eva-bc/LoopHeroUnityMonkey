using UnityEngine;
using UnityEngine.AI;

namespace HideAndSeek
{
    /// <summary>
    /// Controls Johnny Kiki: Patrol → Chase → Attack state machine.
    /// Uses NavMeshAgent for 3D pathfinding and an Animator for animations.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyVision))]
    public class EnemyController : MonoBehaviour
    {
        private enum EnemyState { Patrolling, Chasing, Attacking }

        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float waypointReachThreshold = 0.4f;

        [Header("Chase")]
        [SerializeField] private float chaseSpeed = 5f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.2f;
        [SerializeField] private float attackCooldown = 1.0f;

        [Header("LookAround")]
        [SerializeField] private float lookAroundDuration = 2f;

        // Animator parameter hashes — must match the Animator Controller state names
        private static readonly int AnimHashSpeed      = Animator.StringToHash("Speed");
        private static readonly int AnimHashAttack     = Animator.StringToHash("Attack");
        private static readonly int AnimHashLookAround = Animator.StringToHash("LookAround");

        private NavMeshAgent _agent;
        private EnemyVision  _vision;
        private Animator     _animator;
        private Transform    _playerTransform;

        private EnemyState _state = EnemyState.Patrolling;
        private int   _currentPatrolIndex;
        private float _attackTimer;
        private float _lookAroundTimer;
        private bool  _hasAttacked;

        private void Awake()
        {
            _agent    = GetComponent<NavMeshAgent>();
            _vision   = GetComponent<EnemyVision>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _playerTransform = player.transform;

            _agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameOver)
            {
                _agent.isStopped = true;
                SetAnimatorSpeed(0f);
                return;
            }

            switch (_state)
            {
                case EnemyState.Patrolling: HandlePatrol();  break;
                case EnemyState.Chasing:   HandleChase();   break;
                case EnemyState.Attacking: HandleAttack();  break;
            }
        }

        // ─── Patrol ────────────────────────────────────────────────────────────────

        private void HandlePatrol()
        {
            if (_vision.CanSeePlayer())
            {
                EnterChase();
                return;
            }

            SetAnimatorSpeed(_agent.velocity.magnitude);

            if (!_agent.pathPending && _agent.remainingDistance < waypointReachThreshold)
            {
                // Brief look-around before moving to next point
                _lookAroundTimer += Time.deltaTime;
                SetAnimatorSpeed(0f);

                if (_lookAroundTimer < lookAroundDuration)
                {
                    TriggerLookAround();
                }
                else
                {
                    _lookAroundTimer = 0f;
                    _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                    GoToNextPatrolPoint();
                }
            }
        }

        private void GoToNextPatrolPoint()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;
            _agent.isStopped = false;
            _agent.speed = patrolSpeed;
            _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
        }

        // ─── Chase ─────────────────────────────────────────────────────────────────

        private void EnterChase()
        {
            _state = EnemyState.Chasing;
            _agent.speed = chaseSpeed;
            _agent.isStopped = false;
        }

        private void HandleChase()
        {
            if (_playerTransform == null) return;

            _agent.SetDestination(_playerTransform.position);
            SetAnimatorSpeed(_agent.velocity.magnitude);

            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                _state = EnemyState.Attacking;
                _agent.isStopped = true;
                _attackTimer = 0f;
                _hasAttacked = false;
                SetAnimatorSpeed(0f);
            }
        }

        // ─── Attack ────────────────────────────────────────────────────────────────

        private void HandleAttack()
        {
            // Face the player
            if (_playerTransform != null)
            {
                Vector3 lookDir = (_playerTransform.position - transform.position);
                lookDir.y = 0f;
                if (lookDir.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
            }

            _attackTimer += Time.deltaTime;

            if (!_hasAttacked && _attackTimer >= 0.1f)
            {
                _hasAttacked = true;
                if (_animator != null)
                    _animator.SetTrigger(AnimHashAttack);
            }

            if (_attackTimer >= attackCooldown)
                GameManager.Instance.OnPlayerCaught();
        }

        // ─── Helpers ───────────────────────────────────────────────────────────────

        private void SetAnimatorSpeed(float speed)
        {
            if (_animator != null)
                _animator.SetFloat(AnimHashSpeed, speed);
        }

        private void TriggerLookAround()
        {
            if (_animator != null)
                _animator.SetTrigger(AnimHashLookAround);
        }
    }
}
