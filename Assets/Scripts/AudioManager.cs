using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource; // Fuente para efectos de sonido

    [Header("Clips")]
    public AudioClip pasosClip;    // Pasos (caminar/correr)
    public AudioClip papelClip;    // Papel (recoger libro, abrir/cerrar página)
    public AudioClip puertaClip;   // Puerta (abrir/cerrar puerta)
    public AudioClip ghostDetectClip; // 👻 Fantasma detecta al jugador

    void Awake()
    {
        // Singleton: asegura que solo haya un AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔊 Reproduce un clip específico
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    // 🔊 Pasos
    public void PlayFootstep()
    {
        PlaySound(pasosClip);
    }

    // 🔊 Papel
    public void PlayPaper()
    {
        PlaySound(papelClip);
    }

    // 🔊 Puerta
    public void PlayDoor()
    {
        PlaySound(puertaClip);
    }

    // 🔊 Fantasma detecta al jugador
    public void PlayGhostDetect()
    {
        PlaySound(ghostDetectClip);
    }
}
