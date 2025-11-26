using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en la zona de victoria. Cargando WinScene...");
            SceneManager.LoadScene("WinScene");
        }
    }
}
