using UnityEngine;
using TMPro;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("UI")]
    public TextMeshProUGUI BooksCounterText;

    [Header("Configuración")]
    public string bookTag = "Book";

    private int totalBooks = 0;
    private int collectedBooks = 0;

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
