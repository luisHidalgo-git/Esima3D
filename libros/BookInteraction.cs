using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    [Header("Configuración")]
    public float interactionDistance = 2.5f;
    public LayerMask interactionLayer;
    public Sprite pageSprite;

    private bool ownsPrompt = false;
    private bool isCollected = false;

    void Update()
    {
        MostrarMensaje();

        if (Input.GetKeyDown(KeyCode.E) && !isCollected)
            TryCollect();
    }

    void MostrarMensaje()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        bool lookingAtBook = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == transform)
                lookingAtBook = true;
        }

        if (lookingAtBook && !isCollected)
        {
            if (!ownsPrompt && CollectPromptManager.Instance != null)
            {
                CollectPromptManager.Instance.ShowPrompt(this, "Presiona [E] para recoger");
                ownsPrompt = true;
            }
        }
        else
        {
            if (ownsPrompt && CollectPromptManager.Instance != null)
            {
                CollectPromptManager.Instance.HidePrompt(this);
                ownsPrompt = false;
            }
        }
    }

    void TryCollect()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.transform == transform)
            {
                isCollected = true;

                if (CollectPromptManager.Instance != null)
                    CollectPromptManager.Instance.ForceHide();

                ownsPrompt = false;

                if (BookManager.Instance != null)
                    BookManager.Instance.RegisterCollection();

                if (PageUIManager.Instance != null)
                    PageUIManager.Instance.ShowNextPage();

                // 🔊 Sonido al recoger con E
                AudioManager.Instance.PlayPaper();

                gameObject.SetActive(false);
            }
        }
    }
}
