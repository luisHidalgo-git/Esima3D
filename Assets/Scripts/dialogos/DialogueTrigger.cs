using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Configuration")]
    [TextArea(3, 10)]
    public string dialogueMessage;
    public float dialogueDuration = 4f;
    public bool triggerOnce = true;

    [Header("Trigger Type")]
    public TriggerType triggerType = TriggerType.OnEnter;

    private bool hasTriggered = false;

    public enum TriggerType
    {
        OnEnter,
        OnStay,
        OnExit
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.OnEnter && other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerType == TriggerType.OnStay && other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerType == TriggerType.OnExit && other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (triggerOnce && hasTriggered)
            return;

        if (DialogueSystem.Instance != null && !string.IsNullOrEmpty(dialogueMessage))
        {
            DialogueSystem.Instance.ShowDialogue(dialogueMessage, dialogueDuration);
            hasTriggered = true;

            if (triggerOnce)
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
