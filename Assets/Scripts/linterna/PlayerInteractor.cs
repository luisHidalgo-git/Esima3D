using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerInteractor : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public float maxDistance = 2f;
    public LayerMask interactableMask;

    public System.Action<string, bool> onPromptChanged;

    private IInteractable current;

    void Update()
    {
        FindInteractable();
        HandleInteract();
    }

    private void FindInteractable()
    {
        current = null;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableMask))
        {
            current = hit.collider.GetComponent<IInteractable>();
        }

        if (current != null)
            onPromptChanged?.Invoke(current.GetPrompt(), true);
        else
            onPromptChanged?.Invoke(string.Empty, false);
    }

    private void HandleInteract()
    {
        if (current == null) return;
        if (Input.GetKeyDown(interactKey))
        {
            current.Interact(gameObject);
        }
    }
}
