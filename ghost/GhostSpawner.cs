using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GhostAI ghostAI;
    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (ghostAI == null)
        {
            ghostAI = GetComponentInChildren<GhostAI>();
        }

        if (ghostAI != null)
        {
            ghostAI.gameObject.SetActive(false);
        }
    }

    public void SpawnGhost()
    {
        if (ghostAI == null)
        {
            Debug.LogWarning("Ghost AI no configurado");
            return;
        }

        if (ghostAI.gameObject.activeSelf)
        {
            DespawnGhost();
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        ghostAI.transform.position = spawnPoint.position;
        ghostAI.transform.rotation = spawnPoint.rotation;
        ghostAI.gameObject.SetActive(true);
    }

    public void SpawnGhostFarthestFromPlayer(Transform player)
    {
        if (ghostAI == null)
        {
            Debug.LogWarning("Ghost AI no configurado");
            return;
        }

        if (ghostAI.gameObject.activeSelf)
        {
            DespawnGhost();
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay puntos de spawn configurados");
            return;
        }

        Transform farthestSpawn = GetFarthestSpawnPoint(player);

        if (farthestSpawn != null)
        {
            ghostAI.transform.position = farthestSpawn.position;
            ghostAI.transform.rotation = farthestSpawn.rotation;
            ghostAI.gameObject.SetActive(true);
        }
    }

    public void DespawnGhost()
    {
        if (ghostAI != null && ghostAI.gameObject.activeSelf)
        {
            ghostAI.gameObject.SetActive(false);
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
