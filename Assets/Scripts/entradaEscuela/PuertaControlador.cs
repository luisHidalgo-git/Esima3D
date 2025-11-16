using UnityEngine;

public class PuertaControlador : MonoBehaviour
{
    [Header("Referencia a la puerta")]
    public GameObject puertaEscuela;

    private void Start()
    {
        // Asegurarse que la puerta esté activa al inicio si ya fue activada por el trigger
        if (puertaEscuela != null)
            puertaEscuela.SetActive(false); // Opcional si ya se activa por trigger
    }

    private void Update()
    {
        if (BookManager.Instance != null && puertaEscuela.activeSelf)
        {
if (BookManager.Instance != null && BookManager.Instance.InstanceLibrosCompletados())
            {
                puertaEscuela.SetActive(false); // Desaparece la puerta
                enabled = false; // Desactiva este script para evitar chequeos innecesarios
            }
        }
    }
}
