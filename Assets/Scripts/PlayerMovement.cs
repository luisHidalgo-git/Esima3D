using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 2.5f;
    public float gravity = -9.81f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    public LayerMask wallMask;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRecoveryRate = 0.5f;
    public float requiredStaminaToStartRun = 0.25f; // Umbral mínimo para poder iniciar el sprint
    public Image staminaBar; // Asignar en el inspector

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentStamina;
    private bool isRunning;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Detección de suelo
        RaycastHit hit;
        bool grounded = Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            out hit,
            groundDistance + 0.1f,
            groundMask
        );

        isGrounded = grounded && Vector3.Angle(hit.normal, Vector3.up) < 45f;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movimiento
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = move.magnitude > 0.1f;

        // Solo corre si:
        // - está presionando Shift
        // - se está moviendo
        // - tiene stamina por encima del umbral para iniciar (evita correr con 0)
        // Mientras corre, si llega a 0, se corta inmediatamente.
        if (isRunning)
        {
            // Si se agotó, cortar el sprint
            if (currentStamina <= 0f || !wantsToRun || !isMoving)
                isRunning = false;
        }
        else
        {
            // Intento de iniciar sprint
            isRunning = wantsToRun && isMoving && currentStamina >= requiredStaminaToStartRun;
        }

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Stamina (drenaje solo cuando realmente está corriendo)
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Actualizar barra visual
        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * (groundDistance + 0.1f));
    }
}
