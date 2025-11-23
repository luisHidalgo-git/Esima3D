using UnityEngine;
using TMPro;

public class CollectPromptManager : MonoBehaviour
{
    public static CollectPromptManager Instance;

    [Header("UI")]
    public TextMeshProUGUI CollectPromptText;

    private BookInteraction currentOwner = null;

    void Awake()
    {
        Instance = this;

        if (CollectPromptText != null)
            CollectPromptText.gameObject.SetActive(false);
    }

    public void ShowPrompt(BookInteraction owner, string message)
    {
        currentOwner = owner;

        if (CollectPromptText != null)
        {
            CollectPromptText.text = message;
            CollectPromptText.gameObject.SetActive(true);
        }
    }

    public void HidePrompt(BookInteraction owner)
    {
        if (currentOwner != owner)
            return;

        currentOwner = null;

        if (CollectPromptText != null)
            CollectPromptText.gameObject.SetActive(false);
    }

    public void ForceHide()
    {
        currentOwner = null;

        if (CollectPromptText != null)
            CollectPromptText.gameObject.SetActive(false);
    }
}
