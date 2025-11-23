using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBackgroundMusic();
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado");
        }
    }
}
