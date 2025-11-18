using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject ghostPrefab; // Prefab del fantasma
    [SerializeField] private Transform[] spawnPoints; // Puntos de reaparición

    private GameObject currentGhost;

    // ❌ Eliminamos el SpawnGhost() en Start/Awake
    // El fantasma NO debe aparecer automáticamente al inicio

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
}
