using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GhostAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public LayerMask playerMask;
    public GhostSpawner ghostSpawner;
    public CameraLook playerCamera;
    public Transform faceTarget;

    [Header("Detección")]
    public float detectionRadius = 5f;
    public float attackRadius = 1.5f;

    [Header("Movimiento")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float idleDurationMin = 1f;
    public float idleDurationMax = 2f;
    public float wanderRadius = 5f;
    public float wanderIntervalMin = 2f;
    public float wanderIntervalMax = 5f;

    [Header("Ataque")]
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    [Header("Protección")]
    public float freezeBeforeDestruction = 0.5f;
    public float protectionCooldown = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextWanderTime;
    private float idleTimer;
    private float lastAttackTime;
    private bool isWalkingRandom;
    private bool hasPlayedDetectSound = false;
    private bool wasPlayerDetectedLastFrame = false;
    private float lastProtectionCheckTime = -999f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        idleTimer = Random.Range(idleDurationMin, idleDurationMax);
        nextWanderTime = Time.time + Random.Range(wanderIntervalMin, wanderIntervalMax);
        agent.stoppingDistance = attackRadius * 0.9f;
    }

    void Update()
    {
        bool playerDetected = Physics.CheckSphere(transform.position, detectionRadius, playerMask);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerDetected)
        {
            if (Time.time - lastProtectionCheckTime >= protectionCooldown)
            {
                if (BookManager.Instance != null && BookManager.Instance.TryConsumeProtection())
                {
                    if (ghostSpawner != null)
                    {
                        ghostSpawner.RespawnGhostFarthestFromPlayer(player);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    lastProtectionCheckTime = Time.time;
                    wasPlayerDetectedLastFrame = false;
                    hasPlayedDetectSound = false;
                    return;
                }
            }

            if (!hasPlayedDetectSound)
            {
                AudioManager.Instance.PlayGhostDetect();
                hasPlayedDetectSound = true;
            }

            if (distanceToPlayer <= attackRadius)
            {
                HandleAttack();
            }
            else
            {
                ChasePlayer();
            }

            wasPlayerDetectedLastFrame = true;
        }
        else
        {
            hasPlayedDetectSound = false;
            wasPlayerDetectedLastFrame = false;
            WanderOrIdle();
        }
    }

    void WanderOrIdle()
    {
        agent.speed = walkSpeed;

        if (Time.time >= nextWanderTime)
        {
            isWalkingRandom = Random.value > 0.5f;

            if (isWalkingRandom)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius + transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.isStopped = true;
            }

            nextWanderTime = Time.time + Random.Range(wanderIntervalMin, wanderIntervalMax);
        }

        animator.SetBool("Idle", !isWalkingRandom);
        animator.SetBool("Walk", isWalkingRandom);
        animator.SetBool("Run", false);
    }

    void ChasePlayer()
    {
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        agent.isStopped = false;

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);
    }

    void HandleAttack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("El fantasma ataca y causa " + attackDamage + " de daño.");
            lastAttackTime = Time.time;

            if (playerCamera != null)
            {
                Transform targetPoint = faceTarget != null ? faceTarget : transform;
                playerCamera.LookAtTarget(targetPoint, 0.3f, 1.5f);
            }

            StartCoroutine(TriggerSceneChange());
        }

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);
    }

    IEnumerator TriggerSceneChange()
    {
        yield return new WaitForSeconds(0.5f); // espera breve para que se vea el ataque
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeAndLoadScene("LostScene");
        }
        else
        {
            Debug.LogWarning("ScreenFader.Instance es null. No se puede cambiar de escena.");
        }
    }

    public void DestroyGhost()
    {
        StartCoroutine(FreezeAndDestroy());
    }

    private System.Collections.IEnumerator FreezeAndDestroy()
    {
        agent.isStopped = true;
        animator.enabled = false;
        yield return new WaitForSeconds(freezeBeforeDestruction);
        Destroy(gameObject);
    }
}
