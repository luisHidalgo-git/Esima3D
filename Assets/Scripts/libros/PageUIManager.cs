using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance;

    [Header("UI")]
    public GameObject pagePanel;   // Panel o Image que muestra la hoja
    public Image pageImage;        // La imagen de la hoja
    public Sprite defaultPage;     // Sprite por defecto si no asignas otro

    private bool isPageOpen = false;

    void Awake()
    {
        Instance = this;
        if (pagePanel != null)
            pagePanel.SetActive(false);
    }

    void Update()
    {
        if (isPageOpen && Input.GetKeyDown(KeyCode.Space)) // tecla para cerrar
        {
            ClosePage();
        }
    }

    public void ShowPage(Sprite customPage = null)
    {
        if (pagePanel == null) return;

        pagePanel.SetActive(true);
        isPageOpen = true;

        if (pageImage != null)
        {
            pageImage.sprite = customPage != null ? customPage : defaultPage;
        }

        // Opcional: pausar el juego mientras lees
        // Time.timeScale = 0f;
    }

    public void ClosePage()
    {
        if (pagePanel == null) return;

        pagePanel.SetActive(false);
        isPageOpen = false;

        // Opcional: reanudar el juego
        // Time.timeScale = 1f;
    }
}
