using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float speedWalk = 1.5f;
    public float speedRun = 3.5f;

    private Animator anim;
    private NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Usamos las velocidades del agente para Run/Walk
        agent.speed = speedWalk;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // --- Ataque ---
        if (distance <= attackRange)
        {
            agent.isStopped = true;  // detener movimiento
            anim.SetBool("Attack", true);
            anim.SetFloat("Speed", 0);
            transform.LookAt(player);
            return;
        }
        else
        {
            anim.SetBool("Attack", false);
        }

        // --- Persecución ---
        if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Elegimos caminar o correr dependiendo de la distancia
            if (distance > 5f)
            {
                agent.speed = speedRun;
                anim.SetFloat("Speed", speedRun);
            }
            else
            {
                agent.speed = speedWalk;
                anim.SetFloat("Speed", speedWalk);
            }
        }
        else
        {
            // Idle si no detecta al jugador
            agent.isStopped = true;
            anim.SetFloat("Speed", 0);
        }
    }
}
