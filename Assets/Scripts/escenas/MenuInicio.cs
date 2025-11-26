using UnityEngine;

public class MenuInicio : MonoBehaviour
{
    public AudioClip musicaDeFondo;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicaDeFondo;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void Jugar()
    {
        GameManager.Instance.LoadScene("Gameplay");
    }

    public void Salir()
    {
        GameManager.Instance.QuitGame();
    }
}
