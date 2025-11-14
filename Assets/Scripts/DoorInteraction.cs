using UnityEngine;
using TMPro;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivot; // Empty que rota
    public Transform handle; // Chapa o manija
    public TextMeshProUGUI promptText; // Texto en pantalla

    [Header("Configuración")]
    public float interactionDistance = 2.5f;
    public float rotationAngle = 90f;
    public float rotationSpeed = 3f;
    public LayerMask interactionLayer;
    public float autoCloseDelay = 3f;

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = pivot.rotation;
        openRotation = closedRotation * Quaternion.Euler(rotationAngle, 0, 0);

        if (promptText != null)
            promptText.gameObject.SetActive(false); // Ocultar al inicio
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
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == handle || hit.transform.IsChildOf(handle))
            {
                if (promptText != null && !isOpen)
                    promptText.gameObject.SetActive(true);
                return;
            }
        }

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void TryInteract()
    {
        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == handle || hit.transform.IsChildOf(handle))
            {
                isOpen = true;
                isMoving = true;

                if (promptText != null)
                    promptText.gameObject.SetActive(false);

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
