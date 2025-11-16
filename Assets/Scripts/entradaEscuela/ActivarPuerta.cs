using UnityEngine;

public class ActivarPuerta : MonoBehaviour
{
    public GameObject puertaEscuela; // Asigna en el inspector

    private bool puertaActivada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!puertaActivada && other.CompareTag("Player"))
        {
            puertaEscuela.SetActive(true);
            puertaActivada = true; // Evita que se active más de una vez
        }
    }
}
