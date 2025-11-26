using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;   // Fuente para efectos de sonido
    public AudioSource bgmSource;   // Fuente para música de fondo

    [Header("Clips - SFX")]
    public AudioClip pasosClip;
    public AudioClip papelClip;
    public AudioClip puertaClip;
    public AudioClip ghostDetectClip;
    public AudioClip puertaEntradaClip;

    [Header("Clips - Fantasma")]
    public AudioClip ghostRespawnClip; // 🔊 Nuevo clip para respawn

    [Header("Clips - Linterna")]
    public AudioClip flashlightOnClip;
    public AudioClip flashlightOffClip;
    public AudioClip batteryPickupClip;

    [Header("Clips - BGM")]
    public AudioClip backgroundClip; // 🎵 Música de fondo

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔊 Reproduce un clip de efecto
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    // 🎵 Reproduce música de fondo
    public void PlayBackgroundMusic()
    {
        if (backgroundClip != null && bgmSource != null)
        {
            bgmSource.clip = backgroundClip;
            bgmSource.loop = true;   // Que se repita
            bgmSource.Play();
        }
    }

    // 🎵 Detener música de fondo
    public void StopBackgroundMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
    }

    // 🔊 Métodos específicos para tus SFX
    public void PlayFootstep() => PlaySound(pasosClip);
    public void PlayPaper() => PlaySound(papelClip);
    public void PlayDoor() => PlaySound(puertaClip);
    public void PlayGhostDetect() => PlaySound(ghostDetectClip);
    public void PlayDoorEntrance() => PlaySound(puertaEntradaClip);

    // 🔊 Fantasma
    public void PlayGhostRespawn() => PlaySound(ghostRespawnClip);

    // 🔦 Métodos específicos para la linterna
    public void PlayFlashlightOn() => PlaySound(flashlightOnClip);
    public void PlayFlashlightOff() => PlaySound(flashlightOffClip);
    public void PlayBatteryPickup() => PlaySound(batteryPickupClip);
}
