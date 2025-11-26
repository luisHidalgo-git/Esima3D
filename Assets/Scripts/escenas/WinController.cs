using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    void Start()
{
    Debug.Log("WinController activo");
}

    public void Jugar()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
