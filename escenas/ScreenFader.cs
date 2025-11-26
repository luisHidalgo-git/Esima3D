using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    public Image fadeImage; // un UI Image negro que cubra toda la pantalla
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

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
