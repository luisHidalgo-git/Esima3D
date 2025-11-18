using UnityEngine;
using UnityEngine.AI;

public class GhostSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject ghostPrefab; // Prefab del fantasma
    [SerializeField] private Transform[] spawnPoints; // Puntos de reaparición

    [Header("Respawn Settings")]
    [SerializeField] private float respawnDelay = 5f; // Tiempo antes de reaparecer
    private GameObject currentGhost;

    private void Start()
    {
        SpawnGhost();
    }

    public void SpawnGhost()
    {
        // Si ya existe un fantasma, destrúyelo antes de reaparecer
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }

        // Elige un punto aleatorio
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        // Instancia el fantasma en ese punto
        currentGhost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // Ejemplo: llamar este método cuando el fantasma muera
    public void RespawnGhost()
    {
        Invoke(nameof(SpawnGhost), respawnDelay);
    }
}
