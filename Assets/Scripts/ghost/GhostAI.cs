using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent agent;
    private Animator animator;

    private float nextWanderTime;
    private float nextIdleEndTime;
    private bool isIdling;
    private bool playerInRange;
    private bool isAttacking;
    private float lastAttackTime;

    private enum GhostState { Wander, Chase, Attack }
    private GhostState state = GhostState.Wander;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Ajustes iniciales del agente
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.stoppingDistance = attackRadius * 0.9f;
        SetWalkMode();

        ScheduleNextWander();
        StartIdlePhase();
    }

    private void Update()
    {
        if (player == null)
        {
            // Intenta encontrar el jugador por tag, si no fue asignado
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Calcular distancia y rangos
        float distToPlayer = player ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;
        playerInRange = distToPlayer <= detectionRadius;

        // Actualizar Animator con parámetros simples
        animator.SetBool("PlayerInRange", playerInRange);
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // FSM
        switch (state)
        {
            case GhostState.Wander:
                HandleWander();
                if (playerInRange)
                {
                    state = GhostState.Chase;
                    SetRunMode();
                }
                break;

            case GhostState.Chase:
                HandleChase(distToPlayer);
                if (!playerInRange)
                {
                    state = GhostState.Wander;
                    SetWalkMode();
                    StartIdlePhase(); // al salir de persecución, regresa a patrón idle/walk
                }
                else if (distToPlayer <= attackRadius)
                {
                    state = GhostState.Attack;
                    BeginAttack();
                }
                break;

            case GhostState.Attack:
                HandleAttack(distToPlayer);
                break;
        }
    }

    // --- Wander (idle/walk aleatorio) ---
    private void HandleWander()
    {
        // Alterna entre pequeñas fases de idle y destinos aleatorios de walk
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

        // Caminando hacia un punto aleatorio
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            // Decide si entra en otra fase idle o elige nuevo destino
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
        // Speed ~ 0, el Animator irá a Idle por la condición de Speed
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
            // Si falla, intenta cerca del propio agente
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

    // --- Chase ---
    private void HandleChase(float distToPlayer)
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.SetDestination(player.position);
        // Animator: PlayerInRange = true, Speed = agent.velocity.magnitude → Run por transición
    }

    // --- Attack ---
    private void BeginAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        agent.isStopped = true;
        lastAttackTime = Time.time; // para cooldown
    }

    private void HandleAttack(float distToPlayer)
    {
        // Si el jugador está fuera de rango de ataque, vuelve a perseguir
        if (distToPlayer > attackRadius)
        {
            EndAttack();
            state = GhostState.Chase;
            agent.isStopped = false;
            SetRunMode();
            return;
        }

        // Gestiona el cooldown para repetir ataques si sigues cerca
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Reinicia el clip de ataque
            animator.Play("Attack", 0, 0f);
            lastAttackTime = Time.time;
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    // Llamado por Animation Event en el clip Attack (en el frame de impacto)
    // Añade un evento en la animación que llame a "DealDamage"
    public void DealDamage()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRadius + 0.2f)
        {
            // Aquí llamarías a tu sistema de vida del jugador
            // Ejemplo:
            // player.GetComponent<PlayerHealth>()?.ApplyDamage(attackDamage);
        }
    }

    // Gizmos para depuración visual
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
