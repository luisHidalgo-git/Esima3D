using UnityEngine;

public class BookRespawnManager : MonoBehaviour
{
    [Header("Grupos de libros")]
    public Transform libros1;
    public Transform libros2;
    public Transform libros3;

    void Start()
    {
        RespawnBooks();
        BookManager.Instance?.RecalculateTotalBooks(); // Actualiza el contador después del respawn
    }

    void RespawnBooks()
    {
        int totalLibros = libros1.childCount;

        for (int i = 0; i < totalLibros; i++)
        {
            GameObject libro1 = libros1.GetChild(i).gameObject;
            GameObject libro2 = libros2.GetChild(i).gameObject;
            GameObject libro3 = libros3.GetChild(i).gameObject;

            libro1.SetActive(false);
            libro2.SetActive(false);
            libro3.SetActive(false);

            int randomIndex = Random.Range(0, 3);
            if (randomIndex == 0) libro1.SetActive(true);
            else if (randomIndex == 1) libro2.SetActive(true);
            else libro3.SetActive(true);
        }
    }
}
