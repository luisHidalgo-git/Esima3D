using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivot;       // Empty que rota
    public Transform handle;      // Manija
    public Transform lockPiece;   // Chapa

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

        bool lookingAtHandleOrLock = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (IsValidInteractor(hit.transform))
            {
                lookingAtHandleOrLock = true;
            }
        }

        if (lookingAtHandleOrLock && !isOpen)
        {
            if (!ownsPrompt && OpenDoorPromptManager.Instance != null)
            {
                OpenDoorPromptManager.Instance.ShowPrompt(this, "Presiona [E] para abrir");
                ownsPrompt = true;
            }
        }
        else
        {
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
            if (IsValidInteractor(hit.transform))
            {
                isOpen = true;
                isMoving = true;

                if (OpenDoorPromptManager.Instance != null)
                {
                    OpenDoorPromptManager.Instance.ForceHide();
                }

                ownsPrompt = false;
                StartCoroutine(AutoCloseDoor());
            }
        }
    }

    bool IsValidInteractor(Transform target)
    {
        return target == handle || target.IsChildOf(handle) ||
               target == lockPiece || target.IsChildOf(lockPiece);
    }

    IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        isOpen = false;
        isMoving = true;
    }
}
