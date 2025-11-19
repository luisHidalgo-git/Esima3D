using UnityEngine;
using TMPro;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("UI")]
    public TextMeshProUGUI BooksCounterText;
    public TextMeshProUGUI ProtectionCounterText;

    [Header("Configuración")]
    public string bookTag = "Book";
    public int booksToTriggerGhost = 2;

    [Header("Fantasma")]
    public GhostSpawner ghostSpawner;

    private int totalBooks = 0;
    private int collectedBooks = 0;
    private int protectionCharges = 0;
    private bool ghostSpawned = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RecalculateTotalBooks();
    }

    public void RegisterCollection()
    {
        collectedBooks++;
        protectionCharges++;
        UpdateUI();

        AudioManager.Instance.PlayPaper();

        if (!ghostSpawned && collectedBooks >= booksToTriggerGhost)
        {
            ghostSpawned = true;
            ghostSpawner?.SpawnGhost();
        }
    }

    public bool TryConsumeProtection()
    {
        if (protectionCharges > 0)
        {
            protectionCharges--;
            UpdateUI();
            return true;
        }
        return false;
    }

    void UpdateUI()
    {
        if (BooksCounterText != null)
            BooksCounterText.text = $"{collectedBooks}/{totalBooks}";

        if (ProtectionCounterText != null)
            ProtectionCounterText.text = $"{protectionCharges}";
    }

    public bool InstanceLibrosCompletados()
    {
        return collectedBooks >= totalBooks;
    }

    public void RecalculateTotalBooks()
    {
        GameObject[] books = GameObject.FindGameObjectsWithTag(bookTag);
        totalBooks = 0;

        foreach (GameObject book in books)
        {
            if (book.activeInHierarchy)
                totalBooks++;
        }

        UpdateUI();
    }
}
