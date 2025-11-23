using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GhostAI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;

    [Header("Detección")]
    [SerializeField] private float detectionRadius = 12f;
    [SerializeField] private float attackRadius = 2.2f;
    [SerializeField] private LayerMask playerMask;

    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float idleDurationMin = 1.2f;
    [SerializeField] private float idleDurationMax = 3.5f;
    [SerializeField] private float wanderRadius = 8f;
    [SerializeField] private float wanderIntervalMin = 2f;
    [SerializeField] private float wanderIntervalMax = 5f;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int attackDamage = 10;

    [Header("Protección")]
    [SerializeField] private float freezeBeforeDespawnDuration = 0.5f;

    private NavMeshAgent agent;
    private Animator animator;

    private float nextWanderTime;
    private float nextIdleEndTime;
    private bool isIdling;
    private bool playerInRange;
    private bool isAttacking;
    private float lastAttackTime;

    private bool hasPlayedDetectSound = false;
    private bool isProcessingProtection = false;

    private enum GhostState { Wander, Chase, Attack }
    private GhostState state = GhostState.Wander;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.stoppingDistance = attackRadius * 0.9f;
        SetWalkMode();

        ScheduleNextWander();
        StartIdlePhase();
    }

    private void Update()
    {
        if (isProcessingProtection) return;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        float distToPlayer = player ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;
        playerInRange = distToPlayer <= detectionRadius;

        animator.SetBool("PlayerInRange", playerInRange);
        animator.SetFloat("Speed", agent.velocity.magnitude);

        switch (state)
        {
            case GhostState.Wander:
                HandleWander();
                if (playerInRange)
                {
                    if (CheckAndConsumeProtection())
                        return;

                    state = GhostState.Chase;
                    SetRunMode();

                    if (!hasPlayedDetectSound)
                    {
                        AudioManager.Instance?.PlayGhostDetect();
                        hasPlayedDetectSound = true;
                    }
                }
                break;

            case GhostState.Chase:
                HandleChase(distToPlayer);
                if (!playerInRange)
                {
                    state = GhostState.Wander;
                    SetWalkMode();
                    StartIdlePhase();
                    hasPlayedDetectSound = false;
                }
                else if (distToPlayer <= attackRadius)
                {
                    state = GhostState.Attack;
                }
                break;

            case GhostState.Attack:
                HandleAttack(distToPlayer);
                break;
        }
    }

    private void HandleWander()
    {
        if (isIdling)
        {
            agent.isStopped = true;
            if (Time.time >= nextIdleEndTime)
            {
                isIdling = false;
                agent.isStopped = false;
                PickRandomWanderDestination();
            }
            return;
        }

        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            if (Time.time >= nextWanderTime)
            {
                if (Random.value < 0.5f)
                    StartIdlePhase();
                else
                    PickRandomWanderDestination();

                ScheduleNextWander();
            }
        }
    }

    private void StartIdlePhase()
    {
        isIdling = true;
        agent.isStopped = true;
        float idleDur = Random.Range(idleDurationMin, idleDurationMax);
        nextIdleEndTime = Time.time + idleDur;
    }

    private void PickRandomWanderDestination()
    {
        isIdling = false;

        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir.y = 0f;
        Vector3 target = transform.position + randomDir;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(transform.position + (transform.forward * 1f));
        }
    }

    private void ScheduleNextWander()
    {
        nextWanderTime = Time.time + Random.Range(wanderIntervalMin, wanderIntervalMax);
    }

    private void SetWalkMode()
    {
        agent.speed = walkSpeed;
    }

    private void SetRunMode()
    {
        agent.speed = runSpeed;
    }

    private void HandleChase(float distToPlayer)
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private bool CheckAndConsumeProtection()
    {
        if (BookManager.Instance != null && BookManager.Instance.TryConsumeProtection())
        {
            StartCoroutine(RespawnAfterProtection());
            return true;
        }
        return false;
    }

    private void HandleAttack(float distToPlayer)
    {
        if (CheckAndConsumeProtection())
            return;

        if (distToPlayer > attackRadius)
        {
            EndAttack();
            state = GhostState.Chase;
            agent.isStopped = false;
            SetRunMode();
            return;
        }

        if (!isAttacking)
        {
            BeginAttack();
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.Play("Attack", 0, 0f);
            lastAttackTime = Time.time;
        }
    }

    private void BeginAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        agent.isStopped = true;
        lastAttackTime = Time.time;
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    public void DealDamage()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRadius + 0.2f)
        {
            // player.GetComponent<PlayerHealth>()?.ApplyDamage(attackDamage);
        }
    }

    private IEnumerator RespawnAfterProtection()
    {
        isProcessingProtection = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(freezeBeforeDespawnDuration);

        if (BookManager.Instance != null && BookManager.Instance.ghostSpawner != null)
        {
            BookManager.Instance.ghostSpawner.DespawnGhost();
            BookManager.Instance.ghostSpawner.SpawnGhostFarthestFromPlayer(player);
        }

        isProcessingProtection = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
// 