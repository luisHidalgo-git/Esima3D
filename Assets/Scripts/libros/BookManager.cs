using UnityEngine;
using TMPro;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("UI")]
    public TextMeshProUGUI BooksCounterText;

    [Header("Configuración")]
    public string bookTag = "Book"; // O usa Layer si prefieres
    private int totalBooks = 0;
    private int collectedBooks = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Detectar todos los libros por Tag
        GameObject[] books = GameObject.FindGameObjectsWithTag(bookTag);
        totalBooks = books.Length;

        UpdateUI();
    }

    public void RegisterCollection()
    {
        collectedBooks++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (BooksCounterText != null)
        {
            BooksCounterText.text = $"BOOKS: {collectedBooks}/{totalBooks}";
        }
    }
}
