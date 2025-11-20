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
        public float duration = 3f; // segundos por slide
    }

    [Header("UI")]
    public Image imageUI;
    public TMP_Text textUI;

    [Header("Slides")]
    public IntroSlide[] slides;

    [Header("Escena siguiente")]
    public string nextSceneName = "GameScene";

    [Header("Música de fondo")]
    public AudioSource bgmSource;       // Fuente de audio para la música
    public AudioClip backgroundClip;    // Clip de música de fondo

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (slides == null || slides.Length == 0)
        {
            Debug.LogError("No hay slides asignados en el inspector.");
            return;
        }

        // 🎵 Inicia la música de fondo
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

        if (currentIndex < slides.Length && timer >= slides[currentIndex].duration)
        {
            currentIndex++;

            if (currentIndex < slides.Length)
            {
                ShowSlide(currentIndex);
            }
            else
            {
                // 🎵 Detener música antes de cargar la escena
                if (bgmSource != null && bgmSource.isPlaying)
                {
                    bgmSource.Stop();
                }

                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void ShowSlide(int index)
    {
        if (index >= 0 && index < slides.Length)
        {
            imageUI.sprite = slides[index].image;
            textUI.text = slides[index].text;
            timer = 0f;
        }
    }
}
