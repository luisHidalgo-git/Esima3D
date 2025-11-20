using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public PlayerInteractor interactor;
    public TMP_Text promptText;
    public CanvasGroup group;

    void Awake()
    {
        interactor.onPromptChanged += HandlePrompt;
    }

    private void HandlePrompt(string text, bool visible)
    {
        if (promptText) promptText.text = text;
        if (group) group.alpha = visible ? 1f : 0f;
        promptText.enabled = visible;
    }
}
