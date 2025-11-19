using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private GameObject currentGhost;

    public void SpawnGhost()
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        currentGhost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnGhostFarthestFromPlayer(Transform player)
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay puntos de spawn configurados");
            return;
        }

        Transform farthestSpawn = GetFarthestSpawnPoint(player);

        if (farthestSpawn != null)
        {
            currentGhost = Instantiate(ghostPrefab, farthestSpawn.position, farthestSpawn.rotation);
        }
    }

    private Transform GetFarthestSpawnPoint(Transform player)
    {
        if (player == null || spawnPoints.Length == 0)
            return spawnPoints[0];

        Transform farthest = spawnPoints[0];
        float maxDistance = Vector3.Distance(player.position, spawnPoints[0].position);

        for (int i = 1; i < spawnPoints.Length; i++)
        {
            float distance = Vector3.Distance(player.position, spawnPoints[i].position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthest = spawnPoints[i];
            }
        }

        return farthest;
    }
}
