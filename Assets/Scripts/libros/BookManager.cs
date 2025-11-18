using UnityEngine;
using TMPro;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("UI")]
    public TextMeshProUGUI BooksCounterText;
    public TextMeshProUGUI ProtectionCounterText; // 👈 Nuevo contador en UI

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
        GameObject[] books = GameObject.FindGameObjectsWithTag(bookTag);
        totalBooks = books.Length;
        UpdateUI();
    }

    public void RegisterCollection()
    {
        collectedBooks++;
        protectionCharges++; // 👻 Gana una carga de protección
        UpdateUI();

        AudioManager.Instance.PlayPaper();

        if (!ghostSpawned && collectedBooks >= booksToTriggerGhost)
        {
            ghostSpawned = true;
            if (ghostSpawner != null)
                ghostSpawner.SpawnGhost();
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
}
