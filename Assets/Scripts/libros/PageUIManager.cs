using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance;

    [Header("UI")]
    public GameObject pagePanel;
    public Image pageImage;

    [Header("Configuración")]
    public int totalPages = 8; // ✅ Número total de páginas

    private List<Sprite> orderedPages = new List<Sprite>();
    private int currentPageIndex = 0;
    private bool isPageOpen = false;

    void Awake()
    {
        Instance = this;

        if (pagePanel != null)
            pagePanel.SetActive(false);

        // ✅ Cargar automáticamente los sprites nombrados Pagina1, Pagina2, ..., Pagina8
        for (int i = 1; i <= totalPages; i++)
        {
            Sprite page = Resources.Load<Sprite>($"Pagina{i}");
            if (page != null)
            {
                orderedPages.Add(page);
            }
            else
            {
                Debug.LogWarning($"No se encontró el sprite Pagina{i} en Resources.");
            }
        }
    }

    void Update()
    {
        if (isPageOpen && Input.GetKeyDown(KeyCode.Space))
        {
            ClosePage();
        }
    }

    public void ShowNextPage()
    {
        if (currentPageIndex >= orderedPages.Count)
        {
            Debug.LogWarning("No hay más páginas para mostrar.");
            return;
        }

        Sprite nextPage = orderedPages[currentPageIndex];
        currentPageIndex++;

        ShowPage(nextPage);
    }

    private void ShowPage(Sprite page)
    {
        if (pagePanel == null || pageImage == null) return;

        pagePanel.SetActive(true);
        isPageOpen = true;
        pageImage.sprite = page;
    }

    public void ClosePage()
    {
        if (pagePanel == null) return;

        pagePanel.SetActive(false);
        isPageOpen = false;
    }

    public void ResetPages()
    {
        currentPageIndex = 0;
    }
}
