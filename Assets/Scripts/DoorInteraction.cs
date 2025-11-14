using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivot;   // Empty que rota
    public Transform handle;  // Manija

    [Header("Configuración")]
    public float interactionDistance = 2.5f;
    public float rotationAngle = 90f;
    public float rotationSpeed = 3f;
    public LayerMask interactionLayer; // Debe incluir DoorHandle
    public float autoCloseDelay = 3f;

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    // Estado local: ¿esta puerta es la dueña actual del prompt?
    private bool ownsPrompt = false;

    void Start()
    {
        closedRotation = pivot.rotation;
        openRotation = closedRotation * Quaternion.Euler(rotationAngle, 0, 0);
    }

    void Update()
    {
        MostrarMensaje();

        if (Input.GetKeyDown(KeyCode.E) && !isMoving && !isOpen)
        {
            TryInteract();
        }

        if (isMoving)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(pivot.rotation, targetRotation) < 0.5f)
            {
                pivot.rotation = targetRotation;
                isMoving = false;
            }
        }
    }

    void MostrarMensaje()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        bool lookingAtHandle = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == handle || hit.transform.IsChildOf(handle))
            {
                lookingAtHandle = true;
            }
        }

        if (lookingAtHandle && !isOpen)
        {
            // Mostrar y marcar propiedad si no la teníamos
            if (!ownsPrompt && OpenDoorPromptManager.Instance != null)
            {
                OpenDoorPromptManager.Instance.ShowPrompt(this, "Presiona [E] para abrir");
                ownsPrompt = true;
            }
        }
        else
        {
            // Si esta puerta era la dueña, ahora libera y oculta
            if (ownsPrompt && OpenDoorPromptManager.Instance != null)
            {
                OpenDoorPromptManager.Instance.HidePrompt(this);
                ownsPrompt = false;
            }
        }
    }

    void TryInteract()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == handle || hit.transform.IsChildOf(handle))
            {
                isOpen = true;
                isMoving = true;

                // Al abrir, ocultar prompt global
                if (OpenDoorPromptManager.Instance != null)
                {
                    OpenDoorPromptManager.Instance.ForceHide();
                }

                // Esta puerta ya no posee el prompt
                ownsPrompt = false;

                StartCoroutine(AutoCloseDoor());
            }
        }
    }

    IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        isOpen = false;
        isMoving = true;
    }
}
