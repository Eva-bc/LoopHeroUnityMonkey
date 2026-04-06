using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace HideAndSeek
{
    /// <summary>
    /// Controls Johnny Kiki's behaviour: random patrol near the banana → chase → punch.
    /// Animator is driven by fixed Speed constants per state (not velocity.magnitude)
    /// to guarantee stable Walk / Run / Idle transitions.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyVision))]
    public class EnemyController : MonoBehaviour
    {
        private enum EnemyState { Patrolling, Chasing, Attacking }

        // ── Patrol ──────────────────────────────────────────────────────────────
        [Header("Patrol")]
        [SerializeField] private float patrolRadius      = 5f;
        [SerializeField] private float patrolSpeed       = 2f;
        [SerializeField] private float waypointThreshold = 0.5f;
        [SerializeField] private float pauseAtWaypoint   = 1.2f;

        // ── Chase ───────────────────────────────────────────────────────────────
        [Header("Chase")]
        [SerializeField] private float chaseSpeed        = 5f;

        // ── Attack ──────────────────────────────────────────────────────────────
        [Header("Attack")]
        [SerializeField] private float attackRange       = 1.5f;
        [SerializeField] private float attackCooldown    = 1.2f;

        // Animator parameter hashes — must match JohnnyKikiAnimator.controller
        private static readonly int AnimSpeed  = Animator.StringToHash("Speed");
        private static readonly int AnimAttack = Animator.StringToHash("Attack");

        // Fixed Speed values sent to the Animator (avoids velocity.magnitude spikes)
        private const float SpeedIdle = 0f;
        private const float SpeedWalk = 1.5f;  // triggers Walk state (threshold > 0.1)
        private const float SpeedRun  = 5f;    // triggers Run  state (threshold > 3.5)

        // ── Chase-loss settings ──────────────────────────────────────────────────
        [Header("Chase Loss")]
        [Tooltip("Seconds the enemy keeps chasing after losing sight of the player.")]
        [SerializeField] private float chaseLostTimeout = 3f;

        private NavMeshAgent _agent;
        private EnemyVision  _vision;
        private Animator     _animator;
        private Transform    _playerTransform;
        private Vector3      _bananaPosition;

        private EnemyState _state            = EnemyState.Patrolling;
        private float      _attackTimer;
        private bool       _hasAttacked;
        private float      _chaseLostTimer;

        // ── Lifecycle ────────────────────────────────────────────────────────────

        private void Awake()
        {
            _agent  = GetComponent<NavMeshAgent>();
            _vision = GetComponent<EnemyVision>();

            // Animator lives on the visual child (JohnnyKiki_Visual), not on the root
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _playerTransform = player.transform;

            GameObject banana = GameObject.FindGameObjectWithTag("Banana");
            _bananaPosition = banana != null ? banana.transform.position : transform.position;

            StartCoroutine(WaitForNavMeshThenPatrol());
        }

        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.IsGameOver)
            {
                _agent.isStopped = true;
                SetSpeed(SpeedIdle);
                return;
            }

            switch (_state)
            {
                case EnemyState.Patrolling: CheckVisionDuringPatrol(); break;
                case EnemyState.Chasing:   HandleChase();              break;
                case EnemyState.Attacking: HandleAttack();             break;
            }
        }

        // ── NavMesh boot ─────────────────────────────────────────────────────────

        private IEnumerator WaitForNavMeshThenPatrol()
        {
            float timeout = 3f;
            while (!_agent.isOnNavMesh && timeout > 0f)
            {
                timeout -= Time.deltaTime;
                yield return null;
            }

            if (!_agent.isOnNavMesh)
            {
                Debug.LogError("[EnemyController] NavMesh not baked — bake it via the " +
                               "NavMeshSurface component on the Plane GameObject.");
                yield break;
            }

            StartCoroutine(PatrolRoutine());
        }

        // ── Patrol (random positions near banana) ────────────────────────────────

        private IEnumerator PatrolRoutine()
        {
            while (_state == EnemyState.Patrolling)
            {
                Vector3 destination = GetRandomNavMeshPoint(_bananaPosition, patrolRadius);

                _agent.isStopped = false;
                _agent.speed     = patrolSpeed;
                _agent.SetDestination(destination);
                SetSpeed(SpeedWalk);

                // Walk until destination reached or state changes
                while (_state == EnemyState.Patrolling)
                {
                    if (!_agent.pathPending && _agent.remainingDistance < waypointThreshold)
                        break;
                    yield return null;
                }

                if (_state != EnemyState.Patrolling) yield break;

                // Brief pause at waypoint
                _agent.isStopped = true;
                SetSpeed(SpeedIdle);
                yield return new WaitForSeconds(pauseAtWaypoint);
                _agent.isStopped = false;
            }
        }

        private void CheckVisionDuringPatrol()
        {
            if (_vision.CanSeePlayer())
                EnterChase();
        }

        // ── Chase ────────────────────────────────────────────────────────────────

        private void EnterChase()
        {
            _state           = EnemyState.Chasing;
            _chaseLostTimer  = chaseLostTimeout;
            _agent.isStopped = false;
            _agent.speed     = chaseSpeed;
            SetSpeed(SpeedRun);
        }

        private void HandleChase()
        {
            if (_playerTransform == null) return;

            bool canSee = _vision.CanSeePlayer();

            if (canSee)
            {
                // Reset lost-sight timer while player is visible
                _chaseLostTimer = chaseLostTimeout;
                _agent.SetDestination(_playerTransform.position);
            }
            else
            {
                // Count down — if timer expires, return to patrol
                _chaseLostTimer -= Time.deltaTime;
                if (_chaseLostTimer <= 0f)
                {
                    ReturnToPatrol();
                    return;
                }
                // Keep moving toward last known position
                _agent.SetDestination(_playerTransform.position);
            }

            if (Vector3.Distance(transform.position, _playerTransform.position) <= attackRange)
                EnterAttack();
        }

        private void ReturnToPatrol()
        {
            _state           = EnemyState.Patrolling;
            _agent.isStopped = false;
            _agent.speed     = patrolSpeed;
            SetSpeed(SpeedWalk);
            StartCoroutine(PatrolRoutine());
        }

        // ── Attack ───────────────────────────────────────────────────────────────

        private void EnterAttack()
        {
            _state           = EnemyState.Attacking;
            _agent.isStopped = true;
            _attackTimer     = 0f;
            _hasAttacked     = false;
            SetSpeed(SpeedIdle);
        }

        private void HandleAttack()
        {
            // Face the player
            if (_playerTransform != null)
            {
                Vector3 dir = _playerTransform.position - transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
            }

            _attackTimer += Time.deltaTime;

            if (!_hasAttacked && _attackTimer >= 0.05f)
            {
                _hasAttacked = true;
                _animator?.SetTrigger(AnimAttack);
            }

            if (_attackTimer >= attackCooldown)
                GameManager.Instance.OnPlayerCaught();
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        /// <summary>Sets the Animator Speed parameter to a fixed value per state.</summary>
        private void SetSpeed(float speed)
        {
            _animator?.SetFloat(AnimSpeed, speed);
        }

        /// <summary>
        /// Returns a random valid NavMesh position within radius of center.
        /// Falls back to center if no valid point is found after 10 attempts.
        /// </summary>
        private static Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 rnd2D     = Random.insideUnitCircle * radius;
                Vector3 candidate = center + new Vector3(rnd2D.x, 0f, rnd2D.y);

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, radius, NavMesh.AllAreas))
                    return hit.position;
            }
            return center;
        }
    }
}
