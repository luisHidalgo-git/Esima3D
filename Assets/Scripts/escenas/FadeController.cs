using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("Referencia al panel de fade")]
    public Image fadeImage;

    [Header("Duración del fade")]
    public float fadeDuration = 1f;

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        // Fade in (pantalla se oscurece)
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Cargar la nueva escena
        SceneManager.LoadScene(sceneName);
    }
}
