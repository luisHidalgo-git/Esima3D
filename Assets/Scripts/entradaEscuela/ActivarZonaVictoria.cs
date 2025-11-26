using UnityEngine;

public class ActivarZonaVictoria : MonoBehaviour
{
    [Header("Referencia a la zona de victoria (GameObject vacío)")]
    public GameObject zonaVictoria;

    private void Start()
    {
        if (zonaVictoria != null)
            zonaVictoria.SetActive(false); // La zona inicia desactivada
    }

    private void Update()
    {
        if (BookManager.Instance != null && BookManager.Instance.InstanceLibrosCompletados())
        {
            if (zonaVictoria != null && !zonaVictoria.activeSelf)
            {
                zonaVictoria.SetActive(true); // Activa la zona de victoria
                Debug.Log("Zona de victoria activada. ¡Dirígete ahí para ganar!");
                enabled = false; // Desactiva este script para optimizar
            }
        }
    }
}
