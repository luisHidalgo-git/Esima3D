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
    private string currentDialogueId = "";

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

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }

    public bool IsShowingDialogue()
    {
        return isShowingDialogue;
    }

    public void ShowDialogue(string message, float duration = -1f, string dialogueId = "")
    {
        if (currentDialogueCoroutine != null)
            StopCoroutine(currentDialogueCoroutine);

        currentDialogueId = dialogueId;

        if (dialoguePanel != null && !dialoguePanel.activeSelf)
            dialoguePanel.SetActive(true);

        if (dialogueText != null && !dialogueText.gameObject.activeSelf)
            dialogueText.gameObject.SetActive(true);

        dialogueText.text = "";

        float finalDuration = duration > 0 ? duration : dialogueDuration;
        currentDialogueCoroutine = StartCoroutine(DisplayDialogue(message, finalDuration, dialogueId));
    }

    private IEnumerator DisplayDialogue(string message, float duration, string dialogueId)
    {
        isShowingDialogue = true;

        yield return StartCoroutine(TypeText(message, dialogueId));

        if (currentDialogueId != dialogueId)
            yield break;

        yield return new WaitForSeconds(duration);

        if (currentDialogueId != dialogueId)
            yield break;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        dialogueText.text = "";
        isShowingDialogue = false;
        currentDialogueId = "";
    }

    private IEnumerator TypeText(string text, string dialogueId)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            if (currentDialogueId != dialogueId)
            {
                isTyping = false;
                yield break;
            }

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
        currentDialogueId = "";
    }
}
