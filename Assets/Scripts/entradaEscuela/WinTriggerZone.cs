using UnityEngine;

public class WinTriggerZone : MonoBehaviour
{
    private FadeController fadeController;

    private void Start()
    {
        fadeController = FindObjectOfType<FadeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en la zona de victoria. Iniciando fade...");
            if (fadeController != null)
                fadeController.FadeToScene("WinScene");
        }
    }
}
