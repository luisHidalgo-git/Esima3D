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
    public Image imageUI;               // Imagen del slide
    public TMP_Text textUI;            // Texto del slide
    public CanvasGroup canvasGroup;    // CanvasGroup del SlideGroup (imagen + texto)

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

    void Start()
    {
        if (slides == null || slides.Length == 0)
        {
            Debug.LogError("No hay slides asignados en el inspector.");
            return;
        }

        // 🎵 Inicia música de fondo
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
        if (slides == null || slides.Length == 0) return;

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
        textUI.text = slides[index].text;
        timer = 0f;
        canvasGroup.alpha = 1f;
    }

    System.Collections.IEnumerator FadeToNextSlide(int index)
    {
        isFading = true;

        // Fade out slide
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        ShowSlide(index);

        // Fade in slide
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

        // Fade out slide
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        // 🎵 Fade out música (opcional)
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
}
