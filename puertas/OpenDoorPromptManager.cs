using UnityEngine;
using TMPro;

public class OpenDoorPromptManager : MonoBehaviour
{
    public static OpenDoorPromptManager Instance;

    [Header("UI")]
    public TextMeshProUGUI DoorPromptText; // Debe ser TextMeshProUGUI (UI en Canvas)

    // Puerta que actualmente "posee" el prompt
    private DoorInteraction currentOwner = null;

    void Awake()
    {
        Instance = this;

        if (DoorPromptText != null)
            DoorPromptText.gameObject.SetActive(false); // Oculto al inicio
    }

    // Mostrar el prompt para un dueño concreto
    public void ShowPrompt(DoorInteraction owner, string message)
    {
        currentOwner = owner;

        if (DoorPromptText != null)
        {
            DoorPromptText.text = message;
            DoorPromptText.gameObject.SetActive(true);
        }
    }

    // Ocultar el prompt solo si lo pide el dueño actual
    public void HidePrompt(DoorInteraction owner)
    {
        if (currentOwner != owner)
            return; // Otra puerta no puede ocultar el prompt si no es la dueña

        currentOwner = null;

        if (DoorPromptText != null)
            DoorPromptText.gameObject.SetActive(false);
    }

    // Forzar ocultar (por ejemplo, al abrir la puerta)
    public void ForceHide()
    {
        currentOwner = null;

        if (DoorPromptText != null)
            DoorPromptText.gameObject.SetActive(false);
    }
}
