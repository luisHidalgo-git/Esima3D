using UnityEngine;
using TMPro;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("UI")]
    public TextMeshProUGUI BooksCounterText;

    [Header("Configuración")]
    public string bookTag = "Book";

    [Header("Fantasma")]
    public GhostSpawner ghostSpawner; // referencia al spawner
    public int booksToTriggerGhost = 2; // número de libros necesarios

    private int totalBooks = 0;
    private int collectedBooks = 0;
    private bool ghostSpawned = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject[] books = GameObject.FindGameObjectsWithTag(bookTag);
        totalBooks = books.Length;
        UpdateUI();
    }

    public void RegisterCollection()
    {
        collectedBooks++;
        UpdateUI();

        // 🔊 Sonido de recoger libro
        AudioManager.Instance.PlayPaper();

        // 👻 Aparición del fantasma después del segundo libro
        if (!ghostSpawned && collectedBooks >= booksToTriggerGhost)
        {
            ghostSpawned = true;
            if (ghostSpawner != null)
            {
                ghostSpawner.SpawnGhost();
            }
            else
            {
                Debug.LogWarning("GhostSpawner no asignado en BookManager.");
            }
        }
    }

    void UpdateUI()
    {
        if (BooksCounterText != null)
            BooksCounterText.text = $"{collectedBooks}/{totalBooks}";
    }

    public bool InstanceLibrosCompletados()
    {
        return collectedBooks >= totalBooks;
    }
}
