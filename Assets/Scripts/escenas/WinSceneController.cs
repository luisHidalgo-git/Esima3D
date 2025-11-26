using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup hasGanadoText;
    public CanvasGroup jugarBtn;
    public CanvasGroup menuBtn;
    public CanvasGroup salirBtn;

    [Header("Audio")]
    public AudioSource victoryMusic;

    [Header("Timings")]
    public float blackScreenDuration = 4f; // total tiempo de pantalla negra
    public float fadeDuration = 1f;        // duración de cada transición

    void Start()
    {
        StartCoroutine(WinSequence());
    }

    IEnumerator WinSequence()
    {
        // 1. Pantalla negra (ya está activa)
        yield return new WaitForSeconds(blackScreenDuration / 2);

        // 2. Inicia música
        victoryMusic.Play();

        // 3. Espera 2 segundos después de música
        yield return new WaitForSeconds(2f);

        // 4. Fade in "Has ganado"
        yield return StartCoroutine(FadeIn(hasGanadoText));

        // 5. Espera 1 segundo
        yield return new WaitForSeconds(1f);

        // 6. Fade in botones
        yield return StartCoroutine(FadeIn(jugarBtn));
        yield return StartCoroutine(FadeIn(menuBtn));
        yield return StartCoroutine(FadeIn(salirBtn));
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1;
    }
}
