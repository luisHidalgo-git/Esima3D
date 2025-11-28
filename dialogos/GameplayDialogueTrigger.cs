using UnityEngine;
using System.Collections;

public class GameplayDialogueTriggers : MonoBehaviour
{
    public static GameplayDialogueTriggers Instance;

    [Header("Player References")]
    public FlashlightBattery flashlightBattery;
    public PlayerMovement playerMovement;
    public FlashlightController flashlightController;

    [Header("Trigger Flags")]
    private bool hasShownFlashlightLowBattery = false;
    private bool hasShownFlashlightPickup = false;
    private bool hasShownFirstBook = false;
    private bool hasShownFirstDoor = false;
    private bool hasShownGhostWarning = false;
    private bool hasShownRunTip = false;
    private bool hasShownProtectionInfo = false;

    [Header("Battery Thresholds")]
    public float lowBatteryThreshold = 25f;

    [Header("Tutorial Control")]
    public float tutorialDialogueDuration = 5f;

    private int lastBookCount = 0;
    private bool tutorialInProgress = false;
    private bool movementTutorialShown = false;
    private bool flashlightTutorialShown = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tutorialInProgress = true;
    }

    void Update()
    {
        CheckFlashlightBattery();
        CheckBookCollection();
    }

    private void CheckFlashlightBattery()
    {
        if (flashlightBattery == null || DialogueSystem.Instance == null)
            return;

        if (!hasShownFlashlightLowBattery && flashlightBattery.currentBattery <= lowBatteryThreshold && flashlightBattery.currentBattery > 0)
        {
            DialogueSystem.Instance.ShowDialogue("La bateria esta baja... Necesito encontrar mas pilas rapido.", 4f);
            hasShownFlashlightLowBattery = true;
        }
    }

    private void CheckBookCollection()
    {
        if (BookManager.Instance == null || DialogueSystem.Instance == null)
            return;

        int currentBooks = BookManager.Instance.InstanceLibrosCompletados() ? 8 : 0;

        if (!hasShownFirstBook && currentBooks > lastBookCount && lastBookCount == 0)
        {
            DialogueSystem.Instance.ShowDialogue("Un libro mas recuperado. Debo encontrar el resto antes de que sea demasiado tarde...", 4f);
            hasShownFirstBook = true;
        }

        lastBookCount = currentBooks;
    }

    public void OnFlashlightFirstUse()
    {
        if (!hasShownFlashlightPickup && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Al menos tengo mi linterna. Presiona F para encenderla o apagarla.", 4f);
            hasShownFlashlightPickup = true;
        }
    }

    public void OnFirstDoorInteraction()
    {
        if (!hasShownFirstDoor && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Debo revisar todas las aulas.", 4f);
            hasShownFirstDoor = true;
        }
    }

    public void OnGhostSpawn()
    {
        if (!hasShownGhostWarning && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Que fue eso? Algo no esta bien aqui... Siento una presencia extraña.", 5f);
            hasShownGhostWarning = true;
        }
    }

    public void OnProtectionActivated()
    {
        if (!hasShownProtectionInfo && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Los libros me protegieron! Cada libro que recoja me dara una oportunidad mas.", 5f);
            hasShownProtectionInfo = true;
        }
    }

    public void OnAllBooksCollected()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Tengo todos mis libros! Ahora debo salir de aqui lo mas rapido posible!", 5f);
        }
    }

    public void OnBatteryPickup()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Pilas! Justo lo que necesitaba.", 3f);
        }
    }
    public void OnMovementTutorial()
    {
        if (!hasShownRunTip && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Usa W, A, S, D para moverte. Mantén Shift para correr si tienes energía.", 5f);
            hasShownRunTip = true;
        }
    }

    public void OnFlashlightTutorial()
    {
        if (!hasShownFlashlightPickup && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Presiona F para encender o apagar tu linterna. ¡No te quedes en la oscuridad!", 5f);
            hasShownFlashlightPickup = true;
        }
    }

    public bool CanPlayerMove()
    {
        if (StoryDialogues.Instance != null && StoryDialogues.Instance.IsShowingInitialDialogues)
            return false;

        if (tutorialInProgress && !movementTutorialShown)
            return false;

        return true;
    }

    public bool CanUseFlashlight()
    {
        if (StoryDialogues.Instance != null && StoryDialogues.Instance.IsShowingInitialDialogues)
            return false;

        if (tutorialInProgress && !flashlightTutorialShown)
            return false;

        return true;
    }

    public void StartTutorialSequence()
    {
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        tutorialInProgress = true;

        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Usa W, A, S, D para moverte. Mantén Shift para correr si tienes energía.", tutorialDialogueDuration);
        }

        movementTutorialShown = true;
        hasShownRunTip = true;

        float typingTime = "Usa W, A, S, D para moverte. Mantén Shift para correr si tienes energía.".Length * DialogueSystem.Instance.typingSpeed;
        yield return new WaitForSeconds(typingTime + tutorialDialogueDuration + 0.5f);

        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue("Presiona F para encender o apagar tu linterna. ¡No te quedes en la oscuridad!", tutorialDialogueDuration);
        }

        flashlightTutorialShown = true;
        hasShownFlashlightPickup = true;

        tutorialInProgress = false;
    }

}
