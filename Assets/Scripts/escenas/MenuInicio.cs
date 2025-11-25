using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    // Arrastra tu archivo de audio aquí desde el inspector
    public AudioClip musicaDeFondo;
    private AudioSource audioSource;

    void Start()
    {
        // Crear o usar un AudioSource en el mismo objeto
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicaDeFondo;
        audioSource.loop = true; // Para que se repita
        audioSource.playOnAwake = true; // Que empiece al iniciar
        audioSource.volume = 0.5f; // Ajusta volumen a tu gusto
        audioSource.Play();
    }

    public void Jugar()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
