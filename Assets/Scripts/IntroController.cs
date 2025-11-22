using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [System.Serializable]
    public class IntroSlide
    {
        public Sprite image;
        public string text;
        public float duration = 3f;
    }

    [Header("UI")]
    public Image imageUI;
    public TMP_Text textUI;
    public CanvasGroup canvasGroup;
    public GameObject textBubble;

    [Header("Slides")]
    public IntroSlide[] slides;

    [Header("Escena siguiente")]
    public string nextSceneName = "GameScene";

    [Header("Música de fondo")]
    public AudioSource bgmSource;
    public AudioClip backgroundClip;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isFading = false;
    private bool isTyping = false;

    void Start()
    {
        if (slides == null || slides.Length == 0)
        {
            Debug.LogError("No hay slides asignados en el inspector.");
            return;
        }

        if (bgmSource != null && backgroundClip != null)
        {
            bgmSource.clip = backgroundClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        ShowSlide(0);
    }

    void Update()
    {
        if (slides == null || slides.Length == 0 || isTyping) return;

        timer += Time.deltaTime;

        if (!isFading && currentIndex < slides.Length && timer >= slides[currentIndex].duration)
        {
            currentIndex++;
            if (currentIndex < slides.Length)
            {
                StartCoroutine(FadeToNextSlide(currentIndex));
            }
            else
            {
                StartCoroutine(FadeOutAndLoadScene());
            }
        }
    }

    void ShowSlide(int index)
    {
        imageUI.sprite = slides[index].image;
        canvasGroup.alpha = 1f;
        timer = 0f;

        string currentText = slides[index].text?.Trim();

        if (string.IsNullOrEmpty(currentText))
        {
            textUI.text = "";
            if (textBubble != null) textBubble.SetActive(false);
        }
        else
        {
            if (textBubble != null) textBubble.SetActive(true);
            StartCoroutine(TypeText(currentText));
        }
    }

    System.Collections.IEnumerator FadeToNextSlide(int index)
    {
        isFading = true;

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        ShowSlide(index);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        isFading = false;
    }

    System.Collections.IEnumerator FadeOutAndLoadScene()
    {
        isFading = true;

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        if (bgmSource != null)
        {
            while (bgmSource.volume > 0f)
            {
                bgmSource.volume -= Time.deltaTime * 0.5f;
                yield return null;
            }
            bgmSource.Stop();
        }

        SceneManager.LoadScene(nextSceneName);
    }

    System.Collections.IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        textUI.text = "";
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }
}