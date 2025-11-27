using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [Header("Fade Settings")]
    public Image fadeImage; // UI Image negro que cubre toda la pantalla
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: si quieres que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicia el fade-in al cargar la escena
        StartFadeIn();
    }

    /// <summary>
    /// Inicia el efecto de fade-in desde negro al comenzar la escena.
    /// </summary>
    public void StartFadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Inicia el efecto de fade-out y carga la escena indicada.
    /// </summary>
    public void FadeAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (elapsed < fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(sceneName);
    }
}
