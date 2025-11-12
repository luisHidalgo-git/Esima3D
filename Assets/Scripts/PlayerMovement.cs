using UnityEngine;

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
    public LayerMask groundMask;   // Suelo
    public LayerMask wallMask;     // Nueva capa para paredes/columnas

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Detección de suelo (solo hacia abajo, ignorando paredes)
        RaycastHit hit;
        bool grounded = Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            out hit,
            groundDistance + 0.1f,
            groundMask
        );

        // Confirmar que la superficie es suelo (normal apuntando hacia arriba)
        isGrounded = grounded && Vector3.Angle(hit.normal, Vector3.up) < 45f;

        // Reiniciar velocidad vertical si está en el suelo
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movimiento básico
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Saltar (solo si está en suelo, no en pared)
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * (groundDistance + 0.1f));
    }
}
