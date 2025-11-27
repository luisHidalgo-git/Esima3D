using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public float dialogueDuration = 4f;

    private bool isTyping = false;
    private bool isShowingDialogue = false;
    private Coroutine currentDialogueCoroutine;

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

        // Asegura que panel y texto estén desactivados al inicio
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }

    public void ShowDialogue(string message, float duration = -1f)
    {
        if (currentDialogueCoroutine != null)
            StopCoroutine(currentDialogueCoroutine);

        // Activar panel y texto si están desactivados
        if (dialoguePanel != null && !dialoguePanel.activeSelf)
            dialoguePanel.SetActive(true);

        if (dialogueText != null && !dialogueText.gameObject.activeSelf)
            dialogueText.gameObject.SetActive(true);

        dialogueText.text = "";

        float finalDuration = duration > 0 ? duration : dialogueDuration;
        currentDialogueCoroutine = StartCoroutine(DisplayDialogue(message, finalDuration));
    }

    private IEnumerator DisplayDialogue(string message, float duration)
    {
        isShowingDialogue = true;

        yield return StartCoroutine(TypeText(message));
        yield return new WaitForSeconds(duration);

        // Ocultar panel y texto al terminar
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        dialogueText.text = "";
        isShowingDialogue = false;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void ForceHideDialogue()
    {
        if (currentDialogueCoroutine != null)
            StopCoroutine(currentDialogueCoroutine);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        dialogueText.text = "";
        isShowingDialogue = false;
    }
}
