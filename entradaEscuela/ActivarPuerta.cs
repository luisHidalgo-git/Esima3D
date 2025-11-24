using UnityEngine;

public class ActivarPuerta : MonoBehaviour
{
    public GameObject puertaEscuela;

    private bool puertaActivada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!puertaActivada && other.CompareTag("Player"))
        {
            puertaEscuela.SetActive(true);
            AudioManager.Instance.PlayDoorEntrance();
            puertaActivada = true;
        }
    }
}
