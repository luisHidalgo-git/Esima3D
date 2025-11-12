using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Transform pivot; // Empty que rota
    public Transform handle; // Chapa o manija
    public float interactionDistance = 2.5f;
    public float rotationAngle = 90f;
    public float rotationSpeed = 3f;
    public LayerMask interactionLayer;

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
        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
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
        Debug.Log("Rotando hacia: " + (isOpen ? "abierto" : "cerrado"));

    }

    void TryInteract()
    {
        // if (handle == null)
        // {
        //     Debug.LogError("Handle no está asignado en el Inspector.");
        //     return;
        // }
        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == handle || hit.transform.IsChildOf(handle))
            {
                isOpen = !isOpen;
                isMoving = true;
            }
        }
        // Debug.Log("Intentando interactuar con: " + hit.transform.name);
    }
}
