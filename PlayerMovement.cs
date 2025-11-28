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

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRecoveryRate = 0.5f;
    public float requiredStaminaToStartRun = 0.25f;
    public Image staminaBar;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentStamina;
    private bool isRunning;
    private bool hasMoved = false;

    private float footstepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    void Update()
    {
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

        bool canMove = GameplayDialogueTriggers.Instance != null && GameplayDialogueTriggers.Instance.CanPlayerMove();

        if (!canMove)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (staminaBar != null)
                staminaBar.fillAmount = currentStamina / maxStamina;

            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = move.magnitude > 0.1f;

        if (!hasMoved && isMoving)
        {
            hasMoved = true;
        }

        if (isRunning)
        {
            if (currentStamina <= 0f || !wantsToRun || !isMoving)
                isRunning = false;
        }
        else
        {
            isRunning = wantsToRun && isMoving && currentStamina >= requiredStaminaToStartRun;
        }

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        if (isGrounded && isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.Instance.PlayFootstep();
                footstepTimer = isRunning ? 0.25f : 0.4f;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isRunning)
            currentStamina -= staminaDrainRate * Time.deltaTime;
        else
            currentStamina += staminaRecoveryRate * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
