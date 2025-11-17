using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public Transform player;

    [Header("Rangos")]
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Velocidades")]
    public float speedWalk = 1.5f;
    public float speedRun = 3.5f;

    [Header("Movimiento aleatorio")]
    public float wanderRadius = 5f;
    public float wanderInterval = 4f;

    private Animator anim;
    private NavMeshAgent agent;

    private float nextWanderTime;

    private enum State { Idle, Wander, Chase, Attack }
    private State currentState = State.Idle;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speedWalk;
        nextWanderTime = Time.time + wanderInterval;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // --- Selección de estado ---
        if (distance <= attackRange)
            currentState = State.Attack;

        else if (distance <= detectionRange)
            currentState = State.Chase;

        else
        {
            // solo hacer wander/idle si NO ve al jugador
            if (Time.time >= nextWanderTime)
            {
                currentState = (Random.value > 0.5f) ? State.Wander : State.Idle;
                nextWanderTime = Time.time + wanderInterval;
            }
        }

        // --- Ejecutar lógica del estado ---
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;

            case State.Wander:
                Wander();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    // -------------------- ESTADOS --------------------

    void Idle()
    {
        agent.isStopped = true;
        anim.SetFloat("Speed", 0);
        anim.SetBool("Attack", false);
    }

    void Wander()
    {
        agent.isStopped = false;
        agent.speed = speedWalk;
        anim.SetBool("Attack", false);

        // si ya llegó al destino, volverá a idle o cambiará destino
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
        }

        anim.SetFloat("Speed", speedWalk);
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.speed = speedRun;
        anim.SetBool("Attack", false);

        agent.SetDestination(player.position);
        anim.SetFloat("Speed", speedRun);
    }

    void Attack()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", true);
        anim.SetFloat("Speed", 0);

        transform.LookAt(player);
    }

    // -------------------- UTILITY --------------------

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }
}
