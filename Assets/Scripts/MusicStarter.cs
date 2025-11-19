using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBackgroundMusic();
            Debug.Log("🎵 Música de fondo iniciada");
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado");
        }
    }
}
