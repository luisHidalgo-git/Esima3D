using UnityEngine;
using System.Collections; // Necesario para usar corutinas

public class DoorInteraction : MonoBehaviour
{
    public Transform pivot; // Empty que rota
    public Transform handle; // Chapa o manija
    public float interactionDistance = 2.5f;
    public float rotationAngle = 90f;
    public float rotationSpeed = 3f;
    public LayerMask interactionLayer;
    public float autoCloseDelay = 3f; // Tiempo en segundos antes de que se cierre sola

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = pivot.rotation;
        openRotation = closedRotation * Quaternion.Euler(rotationAngle, 0, 0);
    }

    void Update()
    {
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

        // Debug.Log("Rotando hacia: " + (isOpen ? "abierto" : "cerrado"));
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
                // Solo abrir la puerta
                isOpen = true;
                isMoving = true;

                // Iniciar corutina para cerrarla automáticamente
                StartCoroutine(AutoCloseDoor());
            }
        }
    }

    IEnumerator AutoCloseDoor()
    {
        // Espera unos segundos antes de cerrar
        yield return new WaitForSeconds(autoCloseDelay);

        // Cierra la puerta automáticamente
        isOpen = false;
        isMoving = true;
    }
}
