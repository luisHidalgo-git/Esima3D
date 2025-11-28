using UnityEngine;
using System.Collections;

public class StoryDialogues : MonoBehaviour
{
    public static StoryDialogues Instance;

    [Header("Timing")]
    public float dialogueDuration = 5f;
    public float delayBetweenDialogues = 1f;
    public float initialDelay = 2f;

    private bool hasShownInitialDialogues = false;
    private bool hasShownTwoBookDialogue = false;
    private bool hasShownHalfBookDialogue = false;

    private Coroutine dialogueQueueCoroutine;

    public bool IsShowingInitialDialogues { get; private set; } = true;

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
        StartCoroutine(ShowInitialDialogues());
    }

    void Update()
    {
        CheckBookProgress();
    }

    private IEnumerator ShowInitialDialogues()
    {
        IsShowingInitialDialogues = true;
        yield return new WaitForSeconds(initialDelay);

        if (DialogueSystem.Instance != null)
        {
            yield return StartCoroutine(ShowDialogueWithWait("Ya es tarde... Todos se fueron. Pero olvide mis libros por toda la escuela.", dialogueDuration));
            yield return new WaitForSeconds(delayBetweenDialogues);

            yield return StartCoroutine(ShowDialogueWithWait("Tengo un mal presentimiento sobre esto. La escuela se siente... diferente de noche.", dialogueDuration));
            yield return new WaitForSeconds(delayBetweenDialogues);

            yield return StartCoroutine(ShowDialogueWithWait("Debo apresurarme. Cuanto antes encuentre mis libros, antes podre salir de aqui.", dialogueDuration));
        }

        hasShownInitialDialogues = true;
        IsShowingInitialDialogues = false;

        if (GameplayDialogueTriggers.Instance != null)
        {
            GameplayDialogueTriggers.Instance.StartTutorialSequence();
        }
    }

    private IEnumerator ShowDialogueWithWait(string message, float duration)
    {
        DialogueSystem.Instance.ShowDialogue(message, duration);

        float typingTime = message.Length * DialogueSystem.Instance.typingSpeed;
        yield return new WaitForSeconds(typingTime + duration + 0.5f);
    }

    private void CheckBookProgress()
    {
        if (BookManager.Instance == null || DialogueSystem.Instance == null || !hasShownInitialDialogues)
            return;

        int currentBooks = 0;
        int totalBooks = 8;

        if (BookManager.Instance.BooksCounterText != null)
        {
            string[] parts = BookManager.Instance.BooksCounterText.text.Split('/');
            if (parts.Length == 2)
            {
                int.TryParse(parts[0], out currentBooks);
                int.TryParse(parts[1], out totalBooks);
            }
        }

        if (!hasShownTwoBookDialogue && currentBooks >= 2)
        {
            if (dialogueQueueCoroutine != null)
                StopCoroutine(dialogueQueueCoroutine);
            dialogueQueueCoroutine = StartCoroutine(ShowDialogueWithWait("Estos pasillos parecen mas largos de noche. O sera mi imaginacion?", dialogueDuration));
            hasShownTwoBookDialogue = true;
        }

        if (!hasShownHalfBookDialogue && currentBooks >= (totalBooks / 2))
        {
            if (dialogueQueueCoroutine != null)
                StopCoroutine(dialogueQueueCoroutine);
            dialogueQueueCoroutine = StartCoroutine(ShowDialogueWithWait("No puedo dejar de pensar en las historias que contaban sobre esta escuela...", dialogueDuration));
            hasShownHalfBookDialogue = true;
        }
    }
}
